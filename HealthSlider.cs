using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider healthSlider;

    // Fungsi untuk memperbarui nilai kesehatan pada Slider
    public void UpdateHealth(float healthValue)
    {
        healthSlider.value = healthValue;
    }
}