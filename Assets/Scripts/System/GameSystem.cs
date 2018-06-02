using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{

  [SerializeField]
  private GameObject inputManagerProvider;

  private InputManager inputManager;

  [SerializeField]
  private Canvas pauseCanvas;

  [SerializeField]
  private Canvas deathScreenCanvas;

  [SerializeField]
  private GameState gameState = GameState.RUNNING;


  // Use this for initialization
  void Start()
  {
    inputManager = inputManagerProvider.GetComponent<InputManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (gameState != GameState.DEAD)
    {
      if (inputManager.isPausePressed())
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
    else
    {
      if (inputManager.isButton1Pressed() || inputManager.isPausePressed())
      {
        reloadScene();
      }
    }
  }

  public void HandlePlayerDeath()
  {
    gameState = GameState.DEAD;
    deathScreenCanvas.gameObject.SetActive(true);
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
