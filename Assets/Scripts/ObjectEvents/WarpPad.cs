using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPad : MonoBehaviour
{

  [SerializeField]
  private int destinationScene;

  void OnTriggerEnter2D(Collider2D other)
  {
    BasicPlayer player = other.gameObject.GetComponent<BasicPlayer>();
    if (player != null)
    {
      player.BeginWarpOut(this);
    }
  }

  public void Warp()
  {
    LevelChanger levelChanger = GameObject.FindObjectOfType<LevelChanger>();
    levelChanger.FadeToScene(destinationScene);
  }
}
