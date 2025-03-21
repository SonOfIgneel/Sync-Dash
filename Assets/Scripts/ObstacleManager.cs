using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs; // List of different obstacles
    public int poolSize = 5; // Number of obstacles in the queue
    public float spawnDistance = 50f; // Distance between obstacles

    private Queue<GameObject> playerObstacleQueue = new Queue<GameObject>();
    private Queue<GameObject> ghostObstacleQueue = new Queue<GameObject>();

    public Transform player; // Reference to Player
    public float lastSpawnZ;
    public int passedObstacles = 0; // Track how many obstacles the player has passed

    void Start()
    {
        lastSpawnZ = player.position.z + spawnDistance;

        // Pre-instantiate obstacle objects
        for (int i = 0; i < poolSize; i++)
        {
            // Select a random obstacle from the list
            GameObject playerObstacle = Instantiate(GetRandomObstacle(), new Vector3(0, 1, lastSpawnZ), Quaternion.identity);
            GameObject ghostObstacle = Instantiate(playerObstacle, new Vector3(-50, 1, lastSpawnZ), Quaternion.identity); // Same obstacle on Ghost track

            playerObstacleQueue.Enqueue(playerObstacle);
            ghostObstacleQueue.Enqueue(ghostObstacle);

            if (i != poolSize - 1)
                lastSpawnZ += spawnDistance;
        }
    }

    void Update()
    {
        // If player moves past an obstacle, count it
        if ((player.position.z - 50f) > lastSpawnZ - (spawnDistance * (poolSize - 1)))
        {
            RecycleObstacle();
        }
    }

    void RecycleObstacle()
    {
        Debug.Log("Recycle");
        lastSpawnZ += spawnDistance;

        // Take oldest player obstacle and move it forward
        GameObject oldPlayerObstacle = playerObstacleQueue.Dequeue();
        oldPlayerObstacle.transform.position = new Vector3(0, 1, lastSpawnZ);
        playerObstacleQueue.Enqueue(oldPlayerObstacle);

        // Move the corresponding ghost obstacle
        GameObject oldGhostObstacle = ghostObstacleQueue.Dequeue();
        oldGhostObstacle.transform.position = new Vector3(-50, 1, lastSpawnZ);
        ghostObstacleQueue.Enqueue(oldGhostObstacle);
    }

    GameObject GetRandomObstacle()
    {
        return obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
    }

    public void SetPlayerReference(Transform playerRef)
    {
        player = playerRef;
    }
}

