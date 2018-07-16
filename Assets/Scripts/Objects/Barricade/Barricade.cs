using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{

  public static int GONE = 0;
  public static int IDLE = 1;
  public static int APPEARING = 2;
  public static int DISAPPEARING = 3;

  private BoxCollider2D collider;
  private Animator animator;

  private int state = GONE;

  // Use this for initialization
  void Start()
  {
    collider = GetComponent<BoxCollider2D>();
    animator = GetComponent<Animator>();
  }

  public void Appear()
  {
    SetState(APPEARING);
  }

  public void Disappear()
  {
    SetState(DISAPPEARING);
  }

  public void OnAppearCompleted()
  {
    SetState(IDLE);
  }

  public void OnDisappearCompleted()
  {
    SetState(GONE);
  }

  private void SetState(int state)
  {
    this.state = state;
    animator.SetInteger("State", state);
    collider.enabled = state != GONE;
  }
}
