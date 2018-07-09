using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{

  private Animator animator;
  private PurchaseSelector purchaseSelector;

  [Header("State")]
  [SerializeField]
  private State state = State.CLOSED;

  // Use this for initialization
  void Start()
  {
    animator = GetComponent<Animator>();
    purchaseSelector = GetComponent<PurchaseSelector>();
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
    purchaseSelector.Deactivate();
  }

  public void OnFinishedOpening()
  {
    SetState(State.OPEN);
    List<PowerupChoiceDisplay> choices = new List<PowerupChoiceDisplay>();
    PowerupChoiceDisplay[] choicesArray = GetComponentsInChildren<PowerupChoiceDisplay>();
    foreach (PowerupChoiceDisplay choice in choicesArray)
    {
      choices.Add(choice);
    }

    purchaseSelector.Activate(choices, OnSelected);
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

  private void OnSelected(PowerupChoiceDisplay choice)
  {
    Debug.Log("selected an item");
  }

  public enum State
  {
    CLOSED,
    OPEN,
    OPENING,
    CLOSING
  }
}
