using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference

    [Header("UI Elements")]
    public GameObject mainMenuUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI scoreText, mainMenuHighScore, gameOverScore, gameOverHighScore;

    [Header("Prefabs")]
    public GameObject groundPrefab;
    public GameObject playerPrefab;
    public GameObject ghostPrefab;

    [Header("Hierarchy Elements")]
    public CameraFollow cameraFollow;
    public GroundManager groundManager;
    public ObstacleManager obstacleManager;
    public OrbManager orbManager;
    public AudioSource collectSound, gameSound;
    public AudioClip gameOverClip;
    private GameObject player;
    private GameObject ghost;
    public float score;
    public bool isGameRunning = false;
    public GameObject dummyObjects;
    public List<GameObject> instantiatedObjects;

    void OnEnable()
    {
        instance = this;
        if (PlayerPrefs.HasKey("score"))
        {
            mainMenuHighScore.text = "Current High score: " + PlayerPrefs.GetFloat("score").ToString("F2");
        }
    }

    #region Start Game, End game Functions
    public void StartGame()
    {
        gameSound.Play();
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        isGameRunning = true;
        score = 0;
        Time.timeScale = 1;
        scoreText.gameObject.SetActive(true);
        dummyObjects.SetActive(false);
        SpawnEverything();
    }

    void SpawnEverything()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 2.5f, 0), Quaternion.identity);
        ghost = Instantiate(ghostPrefab, new Vector3(-50, 2.5f, 0), Quaternion.identity);
        instantiatedObjects.Add(player);
        instantiatedObjects.Add(ghost);
        ghost.GetComponent<GhostController>().SetPlayerReference(player.transform);
        player.GetComponent<PlayerController>().manager = this;
        cameraFollow.SetPlayerReference(player.transform);
        cameraFollow.enabled = true;
        cameraFollow.Start();
        obstacleManager.SetPlayerReference(player.transform);
        obstacleManager.enabled = true;
        obstacleManager.Spawn();
        orbManager.SetPlayerReference(player.transform, ghost.transform);
        orbManager.enabled = true;
        orbManager.Spawn();
        groundManager.SetPlayerReference(player.transform);
        groundManager.gameObject.SetActive(true);
        groundManager.Spawn();
    }

    public void GameOver()
    {
        cameraFollow.Shake();
        collectSound.clip = gameOverClip;
        collectSound.Play();
        isGameRunning = false;
        gameSound.Stop();
        scoreText.gameObject.SetActive(false);

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerController>().enabled = false;

        if (PlayerPrefs.HasKey("score"))
        {
            if (score > PlayerPrefs.GetFloat("score"))
            {
                PlayerPrefs.SetFloat("score", score);
                gameOverScore.text = "Score: " + score.ToString("F2");
                gameOverHighScore.text = "Current High score: " + score.ToString("F2");
            }
            else
            {
                gameOverScore.text = "Score: " + score.ToString("F2");
                gameOverHighScore.text = "Current High score: " + PlayerPrefs.GetFloat("score").ToString("F2");
            }
        }
        else
        {
            PlayerPrefs.SetFloat("score", score);
            gameOverScore.text = "Score: " + score.ToString("F2");
            gameOverHighScore.text = "Current High score: " + score.ToString("F2");
        }

        gameOverUI.SetActive(true);
    }
    #endregion

    #region Restart, Main menu button functions
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        ResetGame();
        StartGame();
    }

    public void MainMenu()
    {
        dummyObjects.SetActive(true);
        mainMenuHighScore.text = "Current High score: " + PlayerPrefs.GetFloat("score").ToString("F2");
        ResetGame();
        gameOverUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ResetGame()
    {
        Destroy(player);
        Destroy(ghost);
        cameraFollow.transform.position = new Vector3(-25, 20, -45);
        groundManager.playerGroundQueue.Clear();
        groundManager.ghostGroundQueue.Clear();
        orbManager.playerOrbQueue.Clear();
        orbManager.ghostOrbQueue.Clear();
        orbManager.orbPairs.Clear();
        obstacleManager.playerObstacleQueue.Clear();
        obstacleManager.ghostObstacleQueue.Clear();
        foreach (var obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
    }
    #endregion
}
