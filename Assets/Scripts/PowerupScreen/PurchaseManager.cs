using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{

  private Animator animator;

  [Header("State")]
  [SerializeField]
  private State state = State.CLOSED;

  // Use this for initialization
  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Open()
  {
    SetState(State.OPENING);
  }

  public void Close()
  {
    SetState(State.CLOSING);
  }

  public void OnFinishedOpening()
  {
    SetState(State.OPEN);
  }

  public void OnFinishedClosing()
  {
    SetState(State.CLOSED);
  }

  private void SetState(State state)
  {
    this.state = state;
    animator.SetInteger("State", (int)state);
  }

  public enum State
  {
    CLOSED,
    OPEN,
    OPENING,
    CLOSING
  }
}
