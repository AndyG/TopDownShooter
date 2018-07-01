using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class GameSystem : MonoBehaviour
{

  private float timeAlive;

  [SerializeField]
  private GameObject inputManagerProvider;

  [SerializeField]
  private Canvas pauseCanvas;

  [SerializeField]
  private Canvas deathScreenCanvas;

  [SerializeField]
  private Canvas uiCanvas;

  [SerializeField]
  private GameState gameState = GameState.RUNNING;

  [SerializeField]
  private BasicPlayer basicPlayer;

  private Player player;

  // Use this for initialization
  void Start()
  {
    timeAlive = 0f;
    player = ReInput.players.GetPlayer(0);
  }

  void OnEnable()
  {
    basicPlayer.OnPlayerDeathEvent += HandlePlayerDeath;
  }

  void OnDisable()
  {
    basicPlayer.OnPlayerDeathEvent -= HandlePlayerDeath;
  }

  // Update is called once per frame
  void Update()
  {
    timeAlive += Time.deltaTime;
    if (gameState != GameState.DEAD)
    {
      if (player.GetButtonDown("Pause"))
      {
        if (gameState == GameState.PAUSED)
        {
          unpauseGame();
        }
        else if (gameState == GameState.RUNNING)
        {
          pauseGame();
        }
      }
    }
    else if (player.GetButtonDown("Confirm"))
    {
      reloadScene();
    }
  }

  public void HandlePlayerDeath()
  {
    gameState = GameState.DEAD;
    deathScreenCanvas.gameObject.SetActive(true);
    deathScreenCanvas.gameObject.GetComponentInChildren<DeathScreen>().setDead(timeAlive);
  }

  private void pauseGame()
  {
    Time.timeScale = 0;
    gameState = GameState.PAUSED;
    pauseCanvas.gameObject.SetActive(true);
  }

  private void unpauseGame()
  {
    Time.timeScale = 1;
    gameState = GameState.RUNNING;
    pauseCanvas.gameObject.SetActive(false);
  }

  private void reloadScene()
  {
    SceneManager.LoadScene("TestScene");
  }

  public enum GameState
  {
    RUNNING,
    PAUSED,
    DEAD
  }
}
