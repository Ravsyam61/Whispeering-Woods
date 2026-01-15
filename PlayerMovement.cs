using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float rotationSpeed = 720f;

    [Header("References")]
    public Transform cameraTransform; // Kamera player
    public Animator animator;         // Animator player

    private Rigidbody rb;
    private GameManager gameManager;
    private Vector3 moveDirection;
    private bool isGrounded = true;
    private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (animator == null)
            animator = GetComponent<Animator>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Input pergerakan (WASD / Arrow)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Arah kamera (biar gerak sesuai pandangan kamera)
        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        moveDirection = move.normalized;

        // Cek lari (hold Shift)
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Animasi jalan/lari
        if (animator != null)
        {
            float speedPercent = moveDirection.magnitude * (isRunning ? 2f : 1f);
            animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);
            animator.SetBool("IsGrounded", isGrounded);
        }

        // Lompat (Space)
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            if (animator != null)
                animator.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            // Rotasi menghadap arah gerak

            // Pilih kecepatan jalan/lari
            float currentSpeed = isRunning ? runSpeed : moveSpeed;

            // Gerak
            rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Deteksi tanah (biar bisa lompat lagi)
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.1f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Koin"))
        {
            gameManager.AddScore(1);
            gameManager.PlayCoinSFX();

            // if (animator != null)
            //     animator.SetTrigger("Pickup"); // optional animasi ambil koin

            Destroy(other.gameObject);
        }
        // else if (other.CompareTag("Enemy"))
        // {
        //     gameManager.AddScore(-5);
        //     gameManager.PlayObstacleSFX();

        //     if (animator != null)
        //         animator.SetTrigger("Hit"); // optional animasi kena musuh

        //     Destroy(other.gameObject);
        // }
    }
}
