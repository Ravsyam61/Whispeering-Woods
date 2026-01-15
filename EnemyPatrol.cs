using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Waypoint Settings")]
    public Transform[] waypoints; // Array untuk menyimpan daftar waypoint
    public float moveSpeed = 3f;  // Kecepatan gerak enemy

    private int currentWaypointIndex = 0; // Menyimpan indeks waypoint saat ini

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        // Pastikan waypoint tersedia
        if (waypoints.Length == 0) return;

        // Dapatkan waypoint target
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Hitung arah menuju target
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Gerakkan enemy menuju waypoint
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Jika posisi sudah dekat dengan waypoint, lanjut ke waypoint berikutnya
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
