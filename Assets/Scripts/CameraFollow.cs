using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to Player
    private Vector3 offset;

    void Start()
    {
        // Calculate initial offset to maintain Ghost & Player visibility
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Move camera at the exact same speed as the player (Z-Axis only)
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);
        }
    }

    public void SetPlayerReference(Transform playerRef)
    {
        player = playerRef;
    }
}
