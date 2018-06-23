using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
  void OnAnimationComplete()
  {
    Destroy(gameObject);
  }
}
