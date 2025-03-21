using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OrbManager : MonoBehaviour
{
    public GameObject orbPrefab;
    public int poolSize = 4;
    public float spawnDistance = 30f;

    private Queue<GameObject> playerOrbQueue = new Queue<GameObject>();
    private Queue<GameObject> ghostOrbQueue = new Queue<GameObject>();
    private Dictionary<GameObject, GameObject> orbPairs = new Dictionary<GameObject, GameObject>();

    public Transform player, ghost;
    private float lastSpawnZ;

    void Start()
    {
        lastSpawnZ = player.position.z + spawnDistance;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject playerOrb = Instantiate(orbPrefab, new Vector3(0, 1.5f, lastSpawnZ), Quaternion.identity);
            GameObject ghostOrb = Instantiate(orbPrefab, new Vector3(-50, 1.5f, lastSpawnZ), Quaternion.identity);

            playerOrbQueue.Enqueue(playerOrb);
            ghostOrbQueue.Enqueue(ghostOrb);
            orbPairs[playerOrb] = ghostOrb;

            lastSpawnZ += spawnDistance;
        }
    }

    void Update()
    {
        if (playerOrbQueue.Count > 0)
        {
            GameObject nextOrb = playerOrbQueue.Peek();

            // 🚀 If the player has passed the orb and didn't collect it, recycle it
            if (nextOrb.activeSelf && player.position.z > nextOrb.transform.position.z + 5f)
            {
                Debug.Log("Player missed the orb, recycling it.");
                RecycleOrb();
            }
        }
    }

    void RecycleOrb()
    {
        lastSpawnZ += spawnDistance;

        if (playerOrbQueue.Count > 0 && ghostOrbQueue.Count > 0)
        {
            GameObject oldPlayerOrb = playerOrbQueue.Dequeue();
            GameObject oldGhostOrb = ghostOrbQueue.Dequeue();

            Vector3 newPosition = new Vector3(0, 1.5f, lastSpawnZ);
            oldPlayerOrb.transform.position = newPosition;
            oldGhostOrb.transform.position = new Vector3(-50, 1.5f, lastSpawnZ);

            oldPlayerOrb.SetActive(true); // Ensure orb is visible again

            playerOrbQueue.Enqueue(oldPlayerOrb);
            ghostOrbQueue.Enqueue(oldGhostOrb);

            // ✅ Keep dictionary updated
            orbPairs[oldPlayerOrb] = oldGhostOrb;
        }
    }

    public void CollectOrb(GameObject orb)
    {
        GameObject orbParent = orb.transform.parent.gameObject;
        if (!orbPairs.ContainsKey(orbParent)) return;

        GameObject linkedGhostOrb = orbPairs[orbParent];

        // ✅ Ensure correct ghost orb is dequeued and enqueued
        if (ghostOrbQueue.Contains(linkedGhostOrb))
        {
            ghostOrbQueue = ReorderQueue(ghostOrbQueue, linkedGhostOrb);
        }
        else
        {
            Debug.LogWarning("Ghost orb not found in queue! Fixing queue...");
            FixOrbQueue();
        }

        // 🚀 Move the player orb immediately
        playerOrbQueue.Dequeue();
        playerOrbQueue.Enqueue(orbParent);

        ghost.GetComponent<GhostController>().particles.Play();

        linkedGhostOrb.transform.position = new Vector3(-50, 1.5f, lastSpawnZ);
        orbParent.transform.position = new Vector3(0, 1.5f, lastSpawnZ);

    }

    private Queue<GameObject> ReorderQueue(Queue<GameObject> queue, GameObject targetOrb)
    {
        Queue<GameObject> tempQueue = new Queue<GameObject>();

        while (queue.Count > 0)
        {
            GameObject orb = queue.Dequeue();

            if (orb == targetOrb)
            {
                tempQueue.Enqueue(orb); // ✅ Place the correct orb first
                break;
            }

            tempQueue.Enqueue(orb);
        }

        while (queue.Count > 0)
        {
            tempQueue.Enqueue(queue.Dequeue()); // ✅ Add the rest back
        }

        return tempQueue;
    }

    private void FixOrbQueue()
    {
        ghostOrbQueue.Clear();

        foreach (var pair in orbPairs)
        {
            ghostOrbQueue.Enqueue(pair.Value);
        }

        Debug.LogWarning("Ghost queue fixed! Now fully synced.");
    }

    public void SetPlayerReference(Transform playerRef, Transform ghostRef)
    {
        player = playerRef;
        ghost = ghostRef;
    }
}
