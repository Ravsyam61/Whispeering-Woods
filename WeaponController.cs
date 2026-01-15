using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;       // Titik di mana peluru ditembakkan
    public GameObject bulletPrefab;   // Prefab peluru
    public float bulletForce = 20f;   // Kecepatan peluru
    public AudioClip shootSound;      // Sound effect tembakan
    
    private AudioSource audioSource;
    private PlayerHealth playerHealth; // Reference ke PlayerHealth
    private bool canShoot = true;      // Flag untuk cek apakah bisa menembak

    void Start()
    {
        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Cari component PlayerHealth di GameObject yang sama
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Cek apakah player masih hidup sebelum bisa menembak
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            canShoot = false;
        }

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
        }

        // Mainkan sound effect tembakan
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Destroy(bullet, 2.0f); // Hapus peluru setelah 2 detik
    }

    // Fungsi untuk disable shooting dari luar (opsional)
    public void DisableShooting()
    {
        canShoot = false;
    }

    // Fungsi untuk enable shooting dari luar (opsional)
    public void EnableShooting()
    {
        canShoot = true;
    }
}