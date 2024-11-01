using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Skeleton : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    public LayerMask whatIsGround, whatIsPlayer;
    private EnemyList enemyList;
    // Health
    public float skeletonHealth;
    private PlayerHealth playerHealth;
    // Animation
    private Animator animator;
    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake() {
        enemyList = GameObject.FindGameObjectWithTag("UI Enemy List").GetComponent<EnemyList>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update() {
        // check sight/attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if(skeletonHealth <= 0) {
            return;
        }
        if (!playerInSightRange && !playerInAttackRange) {
            animator.SetBool("isMoving", true);
            patrol();
        }
        if (playerInSightRange && !playerInAttackRange) {
            animator.SetBool("isMoving", true);
            chasePlayer();
        }
        if (playerInAttackRange && playerInSightRange) {
            animator.SetBool("isMoving", false);
            attackPlayer();
        }
    }
    private void patrol() {
        if (!walkPointSet) {
            searchWalkPoint();
        }
        if (walkPointSet) {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
    }
    private void searchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true;
        }
    }
    private void chasePlayer() {
        agent.SetDestination(playerTransform.position);
    }
    private void attackPlayer() {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);

        if (!alreadyAttacked) {
            // attack code
            agent.isStopped = true;
            animator.SetTrigger("attacking");
            if(playerHealth != null) {
                Invoke(nameof(skeletonAttack), 1.5f);
            }
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
        }
    }
    private void resetAttack() {
        alreadyAttacked = false;
        agent.isStopped = false;
    }

    public void takeDamage(float damage) {
        skeletonHealth -= damage;
        Debug.Log("Skeleton Takes Damage: " + skeletonHealth);
        if (skeletonHealth <= 0) {
            animator.SetTrigger("death");
            Invoke(nameof(skeletonDie), 3f);
        }
    }

    private void skeletonDie() {
        enemyList.enemyKilled();
        Destroy(gameObject);
    }

    private void skeletonAttack() {
        playerHealth.takeDamage(20);
    }


}
