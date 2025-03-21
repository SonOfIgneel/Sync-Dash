using UnityEngine;
using System.Collections.Generic;

public class GroundManager : MonoBehaviour
{
    public GameObject groundPrefab; // Assign in Inspector
    public int poolSize = 5; // Number of grounds to cycle
    public float groundLength;
    private Queue<GameObject> playerGroundQueue = new Queue<GameObject>();
    private Queue<GameObject> ghostGroundQueue = new Queue<GameObject>();

    public Transform player; // Assign Player in Inspector

    void Start()
    {
        // Initialize the queue with pre-spawned ground pieces
        for (int i = 0; i < poolSize; i++)
        {
            // Spawn Player Ground
            Vector3 playerGroundPos = new Vector3(0, 0, i * groundLength);
            GameObject playerGround = Instantiate(groundPrefab, playerGroundPos, Quaternion.identity);
            playerGroundQueue.Enqueue(playerGround);

            // Spawn Ghost Ground (Shifted Left)
            Vector3 ghostGroundPos = new Vector3(-50, 0, i * groundLength);
            GameObject ghostGround = Instantiate(groundPrefab, ghostGroundPos, Quaternion.identity);
            ghostGroundQueue.Enqueue(ghostGround);
        }
    }

    void Update()
    {
        // Get the first ground piece in the queue
        GameObject firstGround = playerGroundQueue.Peek();

        // If the player has moved past the first ground piece, recycle it
        if (player.position.z > firstGround.transform.position.z + groundLength)
        {
            RecycleGround();
        }
    }

    void RecycleGround()
    {
        float playerRecycleLength = (playerGroundQueue.Count * groundLength) - groundLength; 
        // Take the oldest Player Ground and move it forward
        GameObject oldPlayerGround = playerGroundQueue.Dequeue();
        Vector3 newPlayerPos = new Vector3(0, 0, playerGroundQueue.Peek().transform.position.z + playerRecycleLength);
        oldPlayerGround.transform.position = newPlayerPos;
        playerGroundQueue.Enqueue(oldPlayerGround);

        // Take the oldest Ghost Ground and move it forward
        float ghostRecycleLength = (ghostGroundQueue.Count * groundLength) - groundLength;
        GameObject oldGhostGround = ghostGroundQueue.Dequeue();
        Vector3 newGhostPos = new Vector3(-50, 0, ghostGroundQueue.Peek().transform.position.z + ghostRecycleLength);
        oldGhostGround.transform.position = newGhostPos;
        ghostGroundQueue.Enqueue(oldGhostGround);
    }

    public void SetPlayerReference(Transform playerRef)
    {
        player = playerRef;
    }
}
