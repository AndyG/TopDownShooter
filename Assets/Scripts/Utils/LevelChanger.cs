using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

  private Animator animator;

  private int sceneIndex;

  void Start()
  {
    animator = gameObject.GetComponent<Animator>();
  }

  public void FadeToScene(int sceneIndex)
  {
    this.sceneIndex = sceneIndex;
    animator.SetTrigger("FadeOut");
  }

  private void OnFadeComplete()
  {
    SceneManager.LoadScene(sceneIndex);
  }
}
