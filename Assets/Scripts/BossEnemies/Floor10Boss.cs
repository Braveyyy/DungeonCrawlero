using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Floor10Boss : MonoBehaviour
{
    // AI
    private NavMeshAgent agent;
    private Transform playerTransform;
    public LayerMask whatIsGround, whatIsPlayer;

    // Health
    public float bossHealth;
    private PlayerHealth playerHealth;

    // Animation
    private Animator animator;

    // Patroll
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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if(bossHealth <= 0) {
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
                Invoke(nameof(bossAttack), 1.5f);
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
        bossHealth -= damage;
        if (bossHealth <= 0) {
            animator.SetTrigger("death");
            Invoke(nameof(bossDie), 3f);
        }
    }

    private void bossAttack() {
        playerHealth.takeDamage(40);
    }

    private void bossDie() {
        Destroy(gameObject);
    }
}
