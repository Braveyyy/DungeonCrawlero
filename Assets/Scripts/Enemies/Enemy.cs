using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private bool waitForNextWalkingIteration = false;
    // Differentiate between Enemy Types
    public bool isSkeleton;
    public bool isFloor10Boss;
    // Specific timers for each enemy
    public float deathTimer = 1f;

    // Audio
    private AudioSource audioSource;
    public AudioClip floor10BossFootsteps;
    public AudioClip skeletonFootsteps; 
    public AudioClip floor10BossAttack;
    public AudioClip skeletonAttack;
    public AudioClip floor10BossDeath;
    public AudioClip skeletonDeath;
    private AudioClip attackSFX;
    private AudioClip deathSFX;

    // AI
    private NavMeshAgent agent;
    private Transform playerTransform;
    public LayerMask whatIsGround, whatIsPlayer;

    // UI
    private EnemyList enemyList;

    // Health
    public float enemyHealth;
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
    public bool isWalking;

    private void Awake() {
        enemyList = GameObject.FindGameObjectWithTag("UI Enemy List").GetComponent<EnemyList>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        if(isSkeleton) {
            audioSource.clip = skeletonFootsteps;
            attackSFX = skeletonAttack;
            deathSFX = skeletonDeath;

        }
        else if(isFloor10Boss) {
            audioSource.clip = floor10BossFootsteps;
            attackSFX = floor10BossAttack;
            deathSFX = floor10BossDeath;
        }

    }
    private void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        if(enemyHealth <= 0) {
            return;
        }
        if (!playerInSightRange && !playerInAttackRange) {
            animator.SetBool("isMoving", true);
            isWalking = true;
            patrol();
        }
        if (playerInSightRange && !playerInAttackRange) {
            animator.SetBool("isMoving", true);
            isWalking = true;
            chasePlayer();
        }
        if (playerInAttackRange && playerInSightRange) {
            animator.SetBool("isMoving", false);
            isWalking = false;
            attackPlayer();
        }
        if(isWalking && !waitForNextWalkingIteration) {
            footstepAudio();
            waitForNextWalkingIteration = true;
        }
    }

    // Enemy Movement
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

    // Enemy Attack / Damage Taken
    private void attackPlayer() {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);

        if (!alreadyAttacked) {
            // attack code
            agent.isStopped = true;
            audioSource.Stop();
            animator.SetTrigger("attacking");
            if(playerHealth != null) {
                Invoke(nameof(enemyAttack), 1f);
            }
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
        }
    }
    private void resetAttack() {
        alreadyAttacked = false;
        agent.isStopped = false;
        waitForNextWalkingIteration = false;
    }

    public void takeDamage(float damage) {
        enemyHealth -= damage;
        if (enemyHealth <= 0) {
            audioSource.Stop();
            animator.SetTrigger("death");
            playDeathAudio();
            Invoke(nameof(enemyDie), deathTimer);
        }
    }

    private void enemyDie() {
        enemyList.enemyKilled();
        Destroy(gameObject);
    }

    private void enemyAttack() {
        playAttackAudio();
        playerHealth.takeDamage(20);
    }

    // Enemy Audio
    private void footstepAudio() {
        audioSource.Play();
    }

    private void playAttackAudio() {
        audioSource.PlayOneShot(attackSFX);
    }

    private void playDeathAudio() {
        audioSource.PlayOneShot(deathSFX);
    }

}
