using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class GameSystem : MonoBehaviour
{

  private float timeAlive;

  [Header("Canvases")]
  [SerializeField]
  private Canvas pauseCanvas;

  [SerializeField]
  private Canvas deathScreenCanvas;

  [SerializeField]
  private Canvas uiCanvas;

  [Header("Input")]
  [SerializeField]
  private GameObject inputManagerProvider;

  [Header("Game State")]
  [SerializeField]
  private GameState gameState = GameState.RUNNING;

  [SerializeField]
  private BasicPlayer basicPlayer;

  [SerializeField]
  private PurchaseManager purchaseManager;

  private Player player;

  // Use this for initialization
  void Start()
  {
    timeAlive = 0f;
    player = ReInput.players.GetPlayer(0);
    purchaseManager = GameObject.FindObjectOfType<PurchaseManager>();
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
    if (gameState == GameState.RUNNING)
    {
      if (player.GetButtonDown("Pause"))
      {
        pauseGame();
      }
    }
    else if (gameState == GameState.PAUSED)
    {
      if (player.GetButtonDown("Pause"))
      {
        unpauseGame();
      }
    }
    else if (gameState == GameState.DEAD)
    {
      if (player.GetButtonDown("Confirm"))
      {
        Debug.Log("pressed confirm");
        reloadScene();
      }
    }
    else if (gameState == GameState.PURCHASING)
    {
      // if (player.GetButtonDown("Confirm"))
      // {
      //   purchaseManager.Close();
      //   Time.timeScale = 1;
      //   gameState = GameState.RUNNING;
      // }
    }
  }

  public void HandlePlayerDeath()
  {
    gameState = GameState.DEAD;
    deathScreenCanvas.gameObject.SetActive(true);
    deathScreenCanvas.gameObject.GetComponentInChildren<DeathScreen>().setDead(timeAlive);
  }

  public void BeginPurchase()
  {
    Time.timeScale = 0f;
    purchaseManager.Open();
    gameState = GameState.PURCHASING;
  }

  public void EndPurchase()
  {
    Time.timeScale = 1f;
    purchaseManager.Close();
    gameState = GameState.RUNNING;
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
    SceneManager.LoadScene("GrassArena1");
  }

  public enum GameState
  {
    RUNNING,
    PAUSED,
    PURCHASING,
    DEAD
  }
}
