using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public GameObject enemy;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetActive(true);
            Debug.Log("Setan diaktifkan!");
        }
    }
}
