using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelection : MonoBehaviour
{

  public void OnPlayClicked()
  {
    SceneManager.LoadScene("GrassArena1");
  }

  public void OnQuitClicked()
  {
    Application.Quit();
  }
}
