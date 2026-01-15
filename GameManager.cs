using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI & Game State")]
    public TextMeshProUGUI scoreText;
    public GameObject winCanvas;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip coinSFX;

    [Header("Portal Settings")]
    public GameObject portalObject; // Portal di-scene (bukan prefab)
    
    private int score = 0;
    private int coinsCollected = 0;
    private bool gameEnded = false;

    void Start()
    {
        winCanvas.SetActive(false);
        UpdateScoreUI();

        // Pastikan portal tidak aktif di awal permainan
        if (portalObject != null)
            portalObject.SetActive(false);
    }

    public void AddScore(int value)
    {
        if (gameEnded) return;

        score += value;
        UpdateScoreUI();

        if (value > 0)
        {
            coinsCollected++;
            PlayCoinSFX();

            // Jika sudah kumpul 10 koin, munculkan portal
            if (coinsCollected >= 10 && portalObject != null && !portalObject.activeSelf)
            {
                portalObject.SetActive(true);
                Debug.Log("Portal telah muncul!");
            }
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Coin : " + score;
    }

    public void PlayerEnterPortal()
    {
        if (!gameEnded)
        {
            GameWin();
        }
    }

    void GameWin()
    {
        gameEnded = true;
        winCanvas.SetActive(true);
        Time.timeScale = 0f;
        
        // Tampilkan cursor saat menang
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Debug.Log("Game Menang!");
    }

    public void PlayCoinSFX()
    {
        if (coinSFX != null)
            audioSource.PlayOneShot(coinSFX);
    }
}