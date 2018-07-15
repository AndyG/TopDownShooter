using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{

  private GameSystem gameSystem;
  private Animator animator;
  private PurchaseSelector purchaseSelector;
  private PlayerConfig playerConfig;

  [Header("State")]
  [SerializeField]
  private State state = State.CLOSED;

  private VendingMachine vendingMachineInUse;

  // Use this for initialization
  void Start()
  {
    animator = GetComponent<Animator>();
    purchaseSelector = GetComponent<PurchaseSelector>();
    playerConfig = GameObject.FindObjectOfType<PlayerConfig>();
    gameSystem = GameObject.FindObjectOfType<GameSystem>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void BeginPurchase(VendingMachine vendingMachine)
  {
    gameSystem.BeginPurchase();
    vendingMachineInUse = vendingMachine;
  }

  public void Open()
  {
    SetState(State.OPENING);
  }

  public void Close()
  {
    SetState(State.CLOSING);
    purchaseSelector.Deactivate();
    vendingMachineInUse = null;
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
    PowerupChoice powerupChoice = choice.GetPowerupChoice();
    if (powerupChoice.name == "InvincibleRoll")
    {
      playerConfig.SetHasInvincibleRoll();
    }
    else if (powerupChoice.name == "MissileCooldown")
    {
      playerConfig.SetHasGoodRockets();
    }

    StartCoroutine(EndPurchase());
  }

  /*
  We want to end the purchase on the next frame to clear the input buffer.
   */
  private IEnumerator EndPurchase()
  {
    vendingMachineInUse.OnPurchaseEnded();
    yield return null;
    gameSystem.EndPurchase();
    vendingMachineInUse = null;
  }

  public enum State
  {
    CLOSED,
    OPEN,
    OPENING,
    CLOSING
  }
}
