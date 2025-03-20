using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference

    [Header("UI Elements")]
    public GameObject mainMenuUI;
    public GameObject gameOverUI;
    public Text scoreText;

    [Header("Prefabs")]
    public GameObject groundPrefab;
    public GameObject playerPrefab;
    public GameObject ghostPrefab;

    private GameObject player;
    private GameObject ghost;
    private int score = 0;
    private bool isGameRunning = false;

    void Awake()
    {
        instance = this; // Singleton pattern
    }

    void Start()
    {

    }

    void Update()
    {
        if (isGameRunning)
        {
            score += Mathf.FloorToInt(Time.deltaTime * 10);
            scoreText.text = "Score: " + score;
        }
    }

    // 🚀 Called when Start Button is clicked
    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        isGameRunning = true;
        Time.timeScale = 1;

        SpawnGround();
        SpawnPlayerAndGhost();
    }

    // 🚀 Spawns two connected ground pieces
    void SpawnGround()
    {
        Instantiate(groundPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(groundPrefab, new Vector3(-50, 0, 0), Quaternion.identity);
    }

    // 🚀 Spawns the player & ghost at their positions
    void SpawnPlayerAndGhost()
    {
        player = Instantiate(playerPrefab, new Vector3(5, 1, 0), Quaternion.identity);
        ghost = Instantiate(ghostPrefab, new Vector3(-5, 1, 0), Quaternion.identity);

        // Give Ghost the Player Reference
        ghost.GetComponent<GhostController>().SetPlayerReference(player.transform);
    }

    // 🚀 Called when player crashes
    public void GameOver()
    {
        isGameRunning = false;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    // 🚀 Called when Restart Button is clicked
    public void RestartGame()
    {
        // Reload Scene to Restart (Alternative: Reset Player/Ghost manually)
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
