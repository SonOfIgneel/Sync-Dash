using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs;
    public int poolSize = 5;
    public float spawnDistance = 50f;
    public GameManager manager;

    public Queue<GameObject> playerObstacleQueue = new Queue<GameObject>();
    public Queue<GameObject> ghostObstacleQueue = new Queue<GameObject>();

    public Transform player;
    public float lastSpawnZ;
    public int passedObstacles = 0;

    #region  Spawn
    public void Spawn()
    {
        lastSpawnZ = player.position.z + spawnDistance;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject playerObstacle = Instantiate(GetRandomObstacle(), new Vector3(0, 1, lastSpawnZ), Quaternion.identity);
            GameObject ghostObstacle = Instantiate(playerObstacle, new Vector3(-50, 1, lastSpawnZ), Quaternion.identity);
            manager.instantiatedObjects.Add(playerObstacle);
            manager.instantiatedObjects.Add(ghostObstacle);
            playerObstacleQueue.Enqueue(playerObstacle);
            ghostObstacleQueue.Enqueue(ghostObstacle);

            if (i != poolSize - 1)
                lastSpawnZ += spawnDistance;
        }
    }

    GameObject GetRandomObstacle()
    {
        return obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
    }
    #endregion

    #region  Reuse the obstacles after the player crosses them
    void Update()
    {
        if ((player.position.z - 50f) > lastSpawnZ - (spawnDistance * (poolSize - 1)))
        {
            RecycleObstacle();
        }
    }

    void RecycleObstacle()
    {
        Debug.Log("Recycle");
        lastSpawnZ += spawnDistance;

        GameObject oldPlayerObstacle = playerObstacleQueue.Dequeue();
        oldPlayerObstacle.transform.position = new Vector3(0, 1, lastSpawnZ);
        playerObstacleQueue.Enqueue(oldPlayerObstacle);

        GameObject oldGhostObstacle = ghostObstacleQueue.Dequeue();
        oldGhostObstacle.transform.position = new Vector3(-50, 1, lastSpawnZ);
        ghostObstacleQueue.Enqueue(oldGhostObstacle);
    }
    #endregion

    public void SetPlayerReference(Transform playerRef)
    {
        player = playerRef;
    }
}

