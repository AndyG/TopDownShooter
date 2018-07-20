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

  [SerializeField]
  private float warpOutAnimDurationSecs = 1f;

  private IEnumerator forceStateEnumerator;

  // Use this for initialization
  void Start()
  {
    collider = GetComponent<BoxCollider2D>();
    animator = GetComponent<Animator>();
    SetState(GONE);
  }

  public void Appear()
  {
    SetState(APPEARING);
  }

  public void Disappear()
  {
    SetState(DISAPPEARING);
    // hack to get around the fact that animation events sometimes fail to fire.
    forceStateEnumerator = ForceState(GONE, warpOutAnimDurationSecs);
    StartCoroutine(forceStateEnumerator);
  }

  public void OnAppearCompleted()
  {
    Debug.Log("appear completed");
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
    if (forceStateEnumerator != null)
    {
      StopCoroutine(forceStateEnumerator);
      forceStateEnumerator = null;
    }
  }

  private IEnumerator ForceState(int state, float delaySecs)
  {
    yield return new WaitForSecondsRealtime(delaySecs);
    SetState(state);
  }
}
