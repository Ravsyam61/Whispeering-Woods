using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public TextMeshProUGUI healthText;  // Reference untuk UI Text
    public AudioClip sound75;           // Sound untuk 75% HP
    public AudioClip sound30;           // Sound untuk 30% HP
    
    private AudioSource audioSource;
    private bool triggered75 = false;   // Flag untuk memastikan hanya trigger sekali
    private bool triggered30 = false;

    private void Start()
    {
        currentHealth = maxHealth;
        
        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Sembunyikan text di awal
        if (healthText != null)
        {
            healthText.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        float hpPercent = (float)currentHealth / maxHealth;

        // Trigger saat HP <= 75%
        if (hpPercent <= 0.75f && hpPercent > 0.30f && !triggered75)
        {
            triggered75 = true;
            StartCoroutine(ShowHealthWarning("Darah Musuh: 75%!", sound75));
        }

        // Trigger saat HP <= 30%
        if (hpPercent <= 0.30f && !triggered30)
        {
            triggered30 = true;
            StartCoroutine(ShowHealthWarning("Darah Musuh: 30%!", sound30));
        }

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator ShowHealthWarning(string message, AudioClip sound)
    {
        // Tampilkan text
        if (healthText != null)
        {
            healthText.text = message;
            healthText.gameObject.SetActive(true);
        }

        // Mainkan sound
        if (sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }

        // Tunggu 2 detik
        yield return new WaitForSeconds(2f);

        // Sembunyikan text
        if (healthText != null)
        {
            healthText.gameObject.SetActive(false);
        }
    }

    void Die()
    {
        if (healthText != null)
        {
            healthText.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}