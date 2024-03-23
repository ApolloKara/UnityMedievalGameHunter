using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Patrolling")]
    public float walkPointRange;
    bool walkPointSet;
    Vector3 walkPoint;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("States")]
    public float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void Patrolling()
    {
        if (!walkPointSet || Vector3.Distance(transform.position, walkPoint) < 1f)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
    }

    void SearchWalkPoint()
    {
        // Calculate random point within range, but also based on the current position and direction of the enemy
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float randomRadius = Random.Range(walkPointRange * 0.5f, walkPointRange);
        Vector3 randomDirection = new Vector3(Mathf.Sin(randomAngle), 0f, Mathf.Cos(randomAngle));
        Vector3 randomOffset = randomDirection * randomRadius;

        // Set walk point
        walkPoint = transform.position + randomOffset;

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    void ChasePlayer()
    {
        // Calculate a random offset within a range around the player's position
        Vector3 randomOffset = Random.insideUnitSphere * 5f;
        randomOffset.y = 0f; // Ensure no vertical movement

        // Set the destination to a point near the player, with some randomness
        Vector3 targetPosition = player.position + randomOffset;

        // Set the destination for the agent
        agent.SetDestination(targetPosition);
    }

    void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack Code
            // ...

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
