using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{

  private Animator animator;

  private bool destroyed;

  void Awake()
  {
    this.animator = gameObject.GetComponent<Animator>();
  }

  public void Destroy()
  {
    if (!destroyed)
    {
      destroyed = true;
      animator.SetTrigger("Destroy");
    }
  }

  public void Recover()
  {
    if (destroyed)
    {
      destroyed = false;
      animator.SetTrigger("Recover");
    }
  }
}
