using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference

    [Header("UI Elements")]
    public GameObject mainMenuUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI scoreText;

    [Header("Prefabs")]
    public GameObject groundPrefab;
    public GameObject playerPrefab;
    public GameObject ghostPrefab;

    [Header("Hierarchy Elements")]
    public CameraFollow cameraFollow;
    public GroundManager groundManager;
    public ObstacleManager obstacleManager;
    public OrbManager orbManager;

    private GameObject player;
    private GameObject ghost;
    public float score;
    public bool isGameRunning = false;

    void Awake()
    {
        instance = this; // Singleton pattern
    }

    // 🚀 Called when Start Button is clicked
    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        isGameRunning = true;
        Time.timeScale = 1;

        SpawnPlayerAndGhost();
    }

    // 🚀 Spawns the player & ghost at their positions
    void SpawnPlayerAndGhost()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 2.5f, 0), Quaternion.identity);
        ghost = Instantiate(ghostPrefab, new Vector3(-50, 2.5f, 0), Quaternion.identity);
        ghost.GetComponent<GhostController>().SetPlayerReference(player.transform);
        player.GetComponent<PlayerController>().manager = this;
        cameraFollow.SetPlayerReference(player.transform);
        cameraFollow.enabled = true;
        obstacleManager.SetPlayerReference(player.transform);
        obstacleManager.enabled = true;
        orbManager.SetPlayerReference(player.transform, ghost.transform);
        orbManager.enabled = true;
        groundManager.SetPlayerReference(player.transform);
        groundManager.gameObject.SetActive(true);
    }

    // 🚀 Called when player crashes
    public void GameOver()
    {
        isGameRunning = false;

        // Stop Player and Ghost Movement Instead of Time.timeScale = 0
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerController>().enabled = false;

        // Show Game Over UI
        gameOverUI.SetActive(true);
    }

    // 🚀 Called when Restart Button is clicked
    public void RestartGame()
    {
        // Reload Scene to Restart (Alternative: Reset Player/Ghost manually)
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
