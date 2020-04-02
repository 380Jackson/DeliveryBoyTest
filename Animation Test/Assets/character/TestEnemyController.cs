using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

public class TestEnemyController : MonoBehaviour
{
    // Initialize all referenced components
    private AudioSource audioSourceComponent;
    private NavMeshAgent navMeshAgentComponent;
    private Health healthComponent;

    // Count all enemies and enemy deaths in the scene
    public static int enemiesInScene;
    public static int enemiesKilled;

    private bool messageHasPlayed = false;
    [SerializeField]
    private AudioClip killMessage;
    [SerializeField]
    private AudioClip playerHurt;

    // Set up and initialize state enumerator
    public enum EnemyStates
    {
        Travelling = 0,
        Attacking = 1,
        Stopped = 2
    }
    public EnemyStates enemyState;

    // Initialize all variables
    [SerializeField]
    private List<GameObject> waypoints = new List<GameObject>();
    private int currentWaypoint = 0;

    private Health enemyHealthComponent;

    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private float attacksPerSecond;
    [SerializeField]
    private float minimumAttackDistance;

    private float timeBetweenAttacks;
    private float elapsingTimeBetweenAttacks;

    private void Awake()
    {
        //AnalyticsEvent.

        // Increment the number of enemies in the scene on spawn
        enemiesInScene++;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assign component variables to their corresponding component
        audioSourceComponent = gameObject.GetComponent<AudioSource>();
        navMeshAgentComponent = gameObject.GetComponent<NavMeshAgent>();
        healthComponent = gameObject.GetComponent<Health>();

        // Set timers
        timeBetweenAttacks = 1f / attacksPerSecond;
        elapsingTimeBetweenAttacks = timeBetweenAttacks;


        if (waypoints[0] != null)
        {
            navMeshAgentComponent.SetDestination(waypoints[currentWaypoint].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If there are no waypoints set, target the player by default
        if (waypoints[0] == null)
        {
            waypoints[0] = GameObject.FindGameObjectWithTag("Player");
        }

        if (waypoints[currentWaypoint] != null)
        {
            if (enemyHealthComponent == null)
            {
                enemyHealthComponent = waypoints[currentWaypoint].GetComponent<Health>();
            }
            else if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) <= minimumAttackDistance)
            {
                enemyState = EnemyStates.Attacking;
            }
        }

        // Switch between enemy states depending on state enumerator
        switch (enemyState)
        {
            case EnemyStates.Travelling:
                //Enemy is travelling to the next destination
                StartCoroutine(Travel());
                break;

            case EnemyStates.Attacking:
                // Attack the selected target
                AttackTarget();
                break;

            case EnemyStates.Stopped:
                // Stop moving
                navMeshAgentComponent.isStopped = true;
                break;
        }
    }


    // Travel towards the player (uses a coroutine so as not to set the destination every frame)
    IEnumerator Travel()
    {
        navMeshAgentComponent.SetDestination(waypoints[currentWaypoint].transform.position);
        yield return new WaitForSeconds(0.1f);
    }


    // Attack target at set rate and damage value
    private void AttackTarget()
    {
        StopCoroutine(Travel());
        navMeshAgentComponent.isStopped = true;

        elapsingTimeBetweenAttacks -= Time.deltaTime;

        if (elapsingTimeBetweenAttacks <= 0.0f)
        {
            Debug.Log("I'm attacking!");
            audioSourceComponent.PlayOneShot(playerHurt);
            enemyHealthComponent.health -= attackDamage;
            elapsingTimeBetweenAttacks = timeBetweenAttacks;
        }

        if (enemyHealthComponent.health <= 0)
        {
            if (messageHasPlayed == false)
            {
                audioSourceComponent.PlayOneShot(killMessage);
                messageHasPlayed = true;
            }

            if (waypoints.Count > 1)
            {
                waypoints.RemoveAt(currentWaypoint);
                GoToNextWaypoint();
                enemyState = EnemyStates.Travelling;
                messageHasPlayed = false;
            }
            else
            {
                enemyState = EnemyStates.Stopped;
            }

        }
        else if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) > minimumAttackDistance)
        {
            enemyState = EnemyStates.Travelling;
            navMeshAgentComponent.isStopped = false;
        }
    }

    // Set the next waypoint in the waypoints array
    private void GoToNextWaypoint()
    {
        navMeshAgentComponent.isStopped = false;

        if (currentWaypoint >= waypoints.Count - 1)
        {
            currentWaypoint = 0;
        }
        else
        {
            currentWaypoint++;
        }

        navMeshAgentComponent.SetDestination(waypoints[currentWaypoint].transform.position);
    }

    // If this game object collides with a projectile, reduce health
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            healthComponent.health--;
        }
    }

    // If health is depleted, destroy this game object
    public void OnHealthDepleted()
    {
        enemiesKilled++;

        Destroy(gameObject);
    }

    // Decrement total enemies in scene when this game object is destroyed
    private void OnDestroy()
    {
        enemiesInScene--;
    }
}
