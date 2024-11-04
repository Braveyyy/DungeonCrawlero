using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    // Movement Variables
    public float speed = 5.0f;        
    public float mouseSensitivity = 2.0f;
    public float headBobFrequency = 1.5f;
    public float headBobAmplitude = 0.05f;

    private CharacterController controller;
    private AudioSource audioSource;
    public Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalVelocity = 0.0f;
    public Transform cameraTransform;
    private float xRotation = 0.0f;
    private float bobbingTimer = 0.0f;

    // Attack Variables
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;
    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;
    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        //animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor to the screen
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyUp(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.Locked;
        }
 
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player body left/right with mouse X
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera up/down with mouse Y
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotating the camera
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Player movement
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow keys

        // Direction based on input
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply gravity (optional, adjust if you want jumping)
        if (controller.isGrounded)
        {
            verticalVelocity = -2f; // Keeps the player grounded
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        moveDirection = move * speed;
        moveDirection.y = verticalVelocity;

        // Move the player
        controller.Move(moveDirection * Time.deltaTime);

        // Apply head bobbing
        ApplyHeadBobbing(moveX, moveZ);

        // Attacking
        if(Input.GetMouseButtonDown(0)) { // on left click press down. check if holding works?
            Attack();
        }
    }

    void ApplyHeadBobbing(float moveX, float moveZ)
    {
        // If the player is moving, apply head bobbing
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            bobbingTimer += Time.deltaTime * headBobFrequency;

            // Calculate new camera Y position based on a sine wave for smooth bobbing
            float bobbingOffset = Mathf.Sin(bobbingTimer) * headBobAmplitude;

            // Adjust camera position
            Vector3 cameraPosition = cameraTransform.localPosition;
            cameraPosition.y = 1.75f + bobbingOffset; // Adjust base height if needed
            cameraTransform.localPosition = cameraPosition;
        }
        else
        {
            // Reset bobbing when player stops moving
            bobbingTimer = 0.0f;

            // Reset camera to default position
            Vector3 cameraPosition = cameraTransform.localPosition;
            cameraPosition.y = 1.75f; // Default Y position (adjust based on your setup)
            cameraTransform.localPosition = cameraPosition;
        }
    }

    public void Attack() {
        if(!readyToAttack || attacking) {
            return;
        }
        readyToAttack = false;
        attacking = true;
        animator.SetTrigger("attackTrigger");
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f); // varies pitch so sound effect isn't repetitive
        audioSource.PlayOneShot(swordSwing);
    }

    void ResetAttack() {
        attacking = false;
        readyToAttack = true;
    }
    void AttackRaycast() {
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, attackDistance, attackLayer)) {
            //HitTarget(hit.point);

            if(hit.transform.TryGetComponent<Skeleton>(out Skeleton T)) {
                T.takeDamage(attackDamage);
            }
            else if(hit.transform.TryGetComponent<Floor10Boss>(out Floor10Boss B)) {
                B.takeDamage(attackDamage);
            }
        }
    }
    void HitTarget(Vector3 pos) {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}
