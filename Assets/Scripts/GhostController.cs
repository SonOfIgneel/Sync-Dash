using UnityEngine;
using System.Collections.Generic;

public class GhostController : MonoBehaviour
{
    public Transform player;
    public float delay = 0.2f;
    private Queue<Vector3> positionHistory = new Queue<Vector3>();
    private float xOffset;
    public float ghostSpeedMultiplier = 1f; // Scale speed based on player's increase
    private bool hasCollided = false;
    public ParticleSystem particles;

    void Start()
    {
        xOffset = transform.position.x - player.position.x;
    }

    void FixedUpdate()
    {
        if (player == null || hasCollided) return; // Stop moving if collided

        // Store player's position over time
        Vector3 mirroredPosition = player.position;
        mirroredPosition.x += xOffset; // Keep ghost on the left side

        positionHistory.Enqueue(mirroredPosition);

        // Maintain delay buffer
        if (positionHistory.Count > Mathf.Round(delay / Time.fixedDeltaTime))
        {
            Vector3 targetPosition = positionHistory.Dequeue();

            // 🚀 Increase Ghost speed gradually to match the Player
            float dynamicSpeed = player.GetComponent<PlayerController>().moveSpeed * ghostSpeedMultiplier;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dynamicSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && player.GetComponent<PlayerController>().collided)
        {
            hasCollided = true;
        }
    }

    public void SetPlayerReference(Transform playerRef)
    {
        player = playerRef;
    }
}
