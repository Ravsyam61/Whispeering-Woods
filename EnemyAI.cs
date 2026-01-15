using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Pengaturan Musuh")]
    public float chaseRange = 10f;     
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.5f; // TAMBAHKAN INI - jarak berhenti dari player
    public Transform player;    
    public Transform model;

    [Header("Suara & Efek")]
    public AudioSource chaseSound;     
    public AudioSource jumpscareSound; 
    public GameObject jumpscareImage;  
    public float jumpscareDuration = 2f; 

    private bool hasTriggered = false; 
    private bool isChasing = false;
    private Rigidbody rb; // TAMBAHKAN INI
    private EnemyPatrol patrolScript; // TAMBAHKAN INI

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        patrolScript = GetComponent<EnemyPatrol>();
        
        // Freeze rotation agar tidak jatuh
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (hasTriggered) return; // Jangan gerak kalau sudah jumpscare

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            if (!isChasing)
            {
                StartChaseSound();
                
                // Matikan patrol saat chase
                if (patrolScript != null)
                    patrolScript.enabled = false;
            }

            isChasing = true;
            
            // Hanya kejar kalau masih jauh dari player
            if (distanceToPlayer > stoppingDistance)
            {
                ChasePlayer();
            }
        }
        else
        {
            if (isChasing)
            {
                StopChaseSound();
                
                // Nyalakan patrol lagi
                if (patrolScript != null)
                    patrolScript.enabled = true;
            }

            isChasing = false;
        }
    }

    void ChasePlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotasi menghadap player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Gerakkan pakai Rigidbody (lebih baik untuk collision)
        if (rb != null)
        {
            rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        // Pastikan modelnya tegak lurus
        if (model != null)
            model.localRotation = Quaternion.identity;
    }

    // GANTI ke OnTriggerEnter (lebih reliable)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            TriggerJumpscare();
        }
    }

    // Atau tetap pakai OnCollisionEnter tapi tambahkan ini:
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasTriggered)
        {
            TriggerJumpscare();
        }
    }

    void TriggerJumpscare()
    {
        hasTriggered = true;
        isChasing = false;

        // Stop semua movement
        if (rb != null)
            rb.velocity = Vector3.zero;

        StopChaseSound(); 

        if (jumpscareSound != null)
            jumpscareSound.Play();

        if (jumpscareImage != null)
            jumpscareImage.SetActive(true);

        Debug.Log("Jumpscare aktif!");

        StartCoroutine(HideJumpscare());
    }

    IEnumerator HideJumpscare()
    {
        yield return new WaitForSeconds(jumpscareDuration);

        if (jumpscareImage != null)
            jumpscareImage.SetActive(false);
    }

    void StartChaseSound()
    {
        if (chaseSound != null && !chaseSound.isPlaying)
        {
            chaseSound.loop = true;
            chaseSound.Play();
        }
    }

    void StopChaseSound()
    {
        if (chaseSound != null && chaseSound.isPlaying)
        {
            chaseSound.Stop();
        }
    }
}