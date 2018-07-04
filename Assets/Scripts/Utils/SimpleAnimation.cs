using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
  public void OnAnimationEnded()
  {
    GameObject.Destroy(this.gameObject);
  }
}
