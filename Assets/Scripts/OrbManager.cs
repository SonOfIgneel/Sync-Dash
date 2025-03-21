using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OrbManager : MonoBehaviour
{
    public GameObject orbPrefab;
    public int poolSize = 4;
    public float spawnDistance = 30f;
    public GameManager manager;
    public Queue<GameObject> playerOrbQueue = new Queue<GameObject>();
    public Queue<GameObject> ghostOrbQueue = new Queue<GameObject>();
    public Dictionary<GameObject, GameObject> orbPairs = new Dictionary<GameObject, GameObject>();
    public Transform player, ghost;
    private float lastSpawnZ;

    #region  Spawn
    public void Spawn()
    {
        lastSpawnZ = player.position.z + spawnDistance;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject playerOrb = Instantiate(orbPrefab, new Vector3(0, 1.5f, lastSpawnZ), Quaternion.identity);
            GameObject ghostOrb = Instantiate(orbPrefab, new Vector3(-50, 1.5f, lastSpawnZ), Quaternion.identity);
            manager.instantiatedObjects.Add(playerOrb);
            manager.instantiatedObjects.Add(ghostOrb);
            playerOrbQueue.Enqueue(playerOrb);
            ghostOrbQueue.Enqueue(ghostOrb);
            orbPairs[playerOrb] = ghostOrb;

            lastSpawnZ += spawnDistance;
        }
    }
    #endregion

    #region Call reuse after player crosses the collectable Orbs
    void Update()
    {
        if (playerOrbQueue.Count > 0 && ghostOrbQueue.Count > 0)
        {
            GameObject nextPlayerOrb = playerOrbQueue.Peek();
            GameObject linkedGhostOrb = orbPairs[nextPlayerOrb];

            if (player.position.z > nextPlayerOrb.transform.position.z + 5f &&
                ghost.position.z > linkedGhostOrb.transform.position.z + 5f)
            {
                Debug.Log($"Both Player and Ghost crossed {nextPlayerOrb.name}, recycling it.");
                RecycleOrb(nextPlayerOrb);
            }
        }
    }
    #endregion

    #region Collect Function
    public void CollectOrb(GameObject collectedOrb)
    {
        GameObject playerOrb = collectedOrb.transform.parent.gameObject;
        if (!orbPairs.ContainsKey(playerOrb)) return;

        GameObject linkedGhostOrb = orbPairs[playerOrb];

        playerOrb.SetActive(false);

        Debug.Log($"Player collected {playerOrb.name}, waiting for Ghost to reach {linkedGhostOrb.name}.");

        StartCoroutine(WaitForGhostToReachOrb(linkedGhostOrb, playerOrb));
    }

    private IEnumerator WaitForGhostToReachOrb(GameObject ghostOrb, GameObject playerOrb)
    {
        float maxWaitTime = 3.0f;
        float elapsedTime = 0f;

        while (ghost.position.z < ghostOrb.transform.position.z)
        {
            if (elapsedTime > maxWaitTime) break;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ghost.GetComponent<GhostController>().particles.Play();
        playerOrb.SetActive(true);
        RecycleOrb(playerOrb);
    }
    #endregion

    #region  Reuse Orbs
    void RecycleOrb(GameObject playerOrb)
    {
        if (!orbPairs.ContainsKey(playerOrb)) return;

        GameObject ghostOrb = orbPairs[playerOrb];
        lastSpawnZ += spawnDistance;

        Vector3 newPosition = new Vector3(0, 1.5f, lastSpawnZ);
        playerOrb.transform.position = newPosition;
        ghostOrb.transform.position = new Vector3(-50, 1.5f, lastSpawnZ);

        playerOrb.SetActive(true);

        playerOrbQueue = ReorderQueue(playerOrbQueue, playerOrb);
        ghostOrbQueue = ReorderQueue(ghostOrbQueue, ghostOrb);

        orbPairs.Remove(playerOrb);
        orbPairs[playerOrb] = ghostOrb;

        Debug.Log($"Recycled Player Orb {playerOrb.name} and Ghost Orb {ghostOrb.name}.");
    }

    private Queue<GameObject> ReorderQueue(Queue<GameObject> queue, GameObject targetOrb)
    {
        Queue<GameObject> tempQueue = new Queue<GameObject>();

        while (queue.Count > 0)
        {
            GameObject orb = queue.Dequeue();
            if (orb == targetOrb)
            {
                tempQueue.Enqueue(orb);
                break;
            }
            tempQueue.Enqueue(orb);
        }

        while (queue.Count > 0)
        {
            tempQueue.Enqueue(queue.Dequeue());
        }

        return tempQueue;
    }
    #endregion

    public void SetPlayerReference(Transform playerRef, Transform ghostRef)
    {
        player = playerRef;
        ghost = ghostRef;
    }
}
