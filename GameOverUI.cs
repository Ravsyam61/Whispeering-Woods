using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void RestartGame()
    {
        // Kembalikan time scale ke normal
        Time.timeScale = 1f;
        
        // Reload scene yang sedang aktif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        // Kembalikan time scale ke normal
        Time.timeScale = 1f;
        
        // Load scene Main Menu (ganti "MainMenu" dengan nama scene Anda)
        SceneManager.LoadScene("MainMenu");
    }
}