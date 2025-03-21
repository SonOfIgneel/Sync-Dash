using UnityEngine;
using System.Collections.Generic;

public class GroundManager : MonoBehaviour
{
    public GameObject groundPrefab;
    public int poolSize = 5;
    public float groundLength;
    public Queue<GameObject> playerGroundQueue = new Queue<GameObject>();
    public Queue<GameObject> ghostGroundQueue = new Queue<GameObject>();
    public GameManager manager;
    public Transform player;

    #region Spawn 
    public void Spawn()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Vector3 playerGroundPos = new Vector3(0, 0, i * groundLength);
            GameObject playerGround = Instantiate(groundPrefab, playerGroundPos, Quaternion.identity);
            playerGroundQueue.Enqueue(playerGround);

            Vector3 ghostGroundPos = new Vector3(-50, 0, i * groundLength);
            GameObject ghostGround = Instantiate(groundPrefab, ghostGroundPos, Quaternion.identity);
            ghostGroundQueue.Enqueue(ghostGround);

            manager.instantiatedObjects.Add(playerGround);
            manager.instantiatedObjects.Add(ghostGround);
        }
    }
    #endregion

    #region Reuse Spawned Objects after player crosses
    void Update()
    {
        GameObject firstGround = playerGroundQueue.Peek();

        if (player.position.z > firstGround.transform.position.z + groundLength)
        {
            RecycleGround();
        }
    }

    void RecycleGround()
    {
        float playerRecycleLength = (playerGroundQueue.Count * groundLength) - groundLength;
        GameObject oldPlayerGround = playerGroundQueue.Dequeue();
        Vector3 newPlayerPos = new Vector3(0, 0, playerGroundQueue.Peek().transform.position.z + playerRecycleLength);
        oldPlayerGround.transform.position = newPlayerPos;
        playerGroundQueue.Enqueue(oldPlayerGround);

        float ghostRecycleLength = (ghostGroundQueue.Count * groundLength) - groundLength;
        GameObject oldGhostGround = ghostGroundQueue.Dequeue();
        Vector3 newGhostPos = new Vector3(-50, 0, ghostGroundQueue.Peek().transform.position.z + ghostRecycleLength);
        oldGhostGround.transform.position = newGhostPos;
        ghostGroundQueue.Enqueue(oldGhostGround);
    }
    #endregion

    public void SetPlayerReference(Transform playerRef)
    {
        player = playerRef;
    }
}
