using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInput : MonoBehaviour
{

  public bool gamePaused = false;

  [SerializeField]
  private GameObject inputManagerProvider;

  private InputManager inputManager;

  [SerializeField]
  private Canvas pauseCanvas;


  // Use this for initialization
  void Start()
  {
    inputManager = inputManagerProvider.GetComponent<InputManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (inputManager.isPausePressed())
    {
      if (gamePaused)
      {
        unpauseGame();
      }
      else
      {
        pauseGame();
        Time.timeScale = 0;
        gamePaused = true;
      }
    }
  }

  void pauseGame()
  {
    Time.timeScale = 0;
    gamePaused = true;
    pauseCanvas.gameObject.SetActive(true);
  }
  void unpauseGame()
  {
    Time.timeScale = 1;
    gamePaused = false;
    pauseCanvas.gameObject.SetActive(false);
  }
}
