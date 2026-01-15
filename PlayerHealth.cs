using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public HealthSlider healthSlider;
    public float currentHealth = 100f;
    public GameObject gameOverUI;
    public GameObject hitImage; // Image yang muncul saat kena hit
    public float healAmount = 10f; // Jumlah heal dari medkit

    private void Start()
    {
        currentHealth = maxHealth;
        gameOverUI.SetActive(false);
        
        // Sembunyikan hit image di awal
        if (hitImage != null)
        {
            hitImage.SetActive(false);
        }

        // Sembunyikan cursor saat game start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        
        // Update nilai slider
        healthSlider.UpdateHealth(currentHealth);

        // Tampilkan hit effect
        if (hitImage != null)
        {
            StartCoroutine(ShowHitEffect());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ShowHitEffect()
    {
        hitImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        hitImage.SetActive(false);
    }

    void Die()
    {
        Time.timeScale = 0f; // Memberhentikan waktu permainan
        gameOverUI.SetActive(true); // Tampilkan Game Over UI
        
        // Sembunyikan hit image saat mati
        if (hitImage != null)
        {
            hitImage.SetActive(false);
        }

        // TAMPILKAN CURSOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Debug.Log("Player has died.");
    }

    // Fungsi untuk mendeteksi collision dengan medkit
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Medkit"))
        {
            Heal(healAmount);
            Destroy(other.gameObject); // Hancurkan medkit setelah diambil
        }
    }

    // Fungsi untuk memulihkan health
    public void Heal(float amount)
    {
        currentHealth += amount;
        
        // Pastikan health tidak melebihi maxHealth
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        // Update health slider
        healthSlider.UpdateHealth(currentHealth);
        
        Debug.Log("Player healed! Current health: " + currentHealth);
    }
}