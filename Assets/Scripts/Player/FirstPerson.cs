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
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor to the screen
    }

    void Update()
    {
        // Cursor Unlock for Menus
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyUp(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.Locked;
        }
 
        // Mouse Movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotating the camera
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Player Movement
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow keys

        // Input Direction
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

        controller.Move(moveDirection * Time.deltaTime);

        ApplyHeadBobbing(moveX, moveZ);

        // Attacking
        if(Input.GetMouseButtonDown(0)) { // on left click press down. check if holding works?
            Attack();
        }
    }

    void ApplyHeadBobbing(float moveX, float moveZ)
    {
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            bobbingTimer += Time.deltaTime * headBobFrequency;

            float bobbingOffset = Mathf.Sin(bobbingTimer) * headBobAmplitude;

            Vector3 cameraPosition = cameraTransform.localPosition;
            cameraPosition.y = 1.75f + bobbingOffset;
            cameraTransform.localPosition = cameraPosition;
        }
        else
        {
            bobbingTimer = 0.0f;

            Vector3 cameraPosition = cameraTransform.localPosition;
            cameraPosition.y = 1.75f; // Default Y position (adjust based on setup)
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
            if(hit.transform.TryGetComponent<Enemy>(out Enemy T)) {
                T.takeDamage(attackDamage);
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
