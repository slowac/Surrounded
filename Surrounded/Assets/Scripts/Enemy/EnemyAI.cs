using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum State // Enemy state: chasing, attacking, attack delay, changing obstacle avoidance priority, retreating
    {
        Chasing,
        Attacking,
        AttackCooldown
    }

    private NavMeshAgent agent; // Enemy's navigation agent
    private Transform playerTransform; // Player's Transform component
    private EnemyHealth enemyHealth; // Enemy's health component
    private PlayerHealth playerHealth; // Player's health component

    public float attackDistance = 2.0f; // Attack distance
    public float attackDamage = 12.0f; // Attack damage
    public float attackDelay = 2.0f; // Attack delay
    public bool isKnockedBack; // Add a new boolean to check if the enemy is being knocked back

    Animator anim;


    public State state; // Current state of the enemy

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get NavMeshAgent component

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Get the player's Transform component

        enemyHealth = GetComponent<EnemyHealth>(); // Get the EnemyHealth component

        playerHealth = playerTransform.GetComponent<PlayerHealth>(); // Get the player's PlayerHealth component

        state = State.Chasing; // Initially set the enemy state to chasing

        anim = GetComponent<Animator>();

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); // Calculate the distance between the player and the enemy

        // Always look at the player but only rotate around the y-axis
        Vector3 lookAtPosition = playerTransform.position;
        lookAtPosition.y = transform.position.y; // Keep the y position unchanged
        transform.LookAt(lookAtPosition);

        switch (state)
        {
            
            case State.Chasing: // If the enemy is in the chasing state
            anim.SetBool("isAttacking", false);

            if (distanceToPlayer <= attackDistance) // If the distance between the enemy and the player is less than or equal to the attack distance
            {
                state = State.Attacking; // Set the enemy state to attack
                anim.SetBool("isAttacking", true);
            }
            else
            {
                if (!isKnockedBack)// Only when the enemy is not knocked back and does not jump
                {
                    agent.SetDestination(playerTransform.position); // Set the target to the player's position
                }
            }
            break;

            case State.Attacking:
            if (distanceToPlayer > attackDistance)
            {
                state = State.Chasing;
            }
            else
            {
                AttackPlayer();
            }
            break;
            case State.AttackCooldown: // In AttackCooldown state, do nothing
            break;
        }
    }

    private void AttackPlayer() // Enemy attack player method
    {
        playerHealth.DealDamage(Mathf.RoundToInt(attackDamage)); // Cause damage
        state = State.AttackCooldown; // After the enemy attacks, it enters the attack cooling state
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay() // Attack delay coroutine
    {
        yield return new WaitForSeconds(attackDelay); // Wait for attack delay
        state = State.Chasing;
    }

    

    //private void OnTriggerEnter(Collider other)
    //{
      //  if (other.CompareTag("Player"))
        //{
          //  anim.SetBool("isAttacking", true);
        //}
    //}

    //private void OnTriggerExit(Collider other)
    //{
      //  if (other.CompareTag("Player"))
        //{
          //  anim.SetBool("isAttacking", false);
        //}
    //}
}