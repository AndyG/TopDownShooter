using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseSelector : MonoBehaviour
{
  public delegate void OnSelected(PowerupChoiceDisplay choice);
  private OnSelected onSelectedCallback;

  private List<PowerupChoiceDisplay> choices;

  [SerializeField]
  private int focusedChoice = 0;

  private MainMenuInput menuInput;

  [SerializeField]
  private float movementDebounceSecs = 0.125f;

  [SerializeField]
  private float buttonPressTimeSecs = 0.125f;

  private bool canMove = true;

  private IEnumerator unlockMoveCoroutine;

  [SerializeField]
  private bool isActive;

  void Start()
  {
    menuInput = GetComponent<MainMenuInput>();
  }

  void Update()
  {
    if (!isActive)
    {
      return;
    }
    menuInput.GatherInput();

    bool didPressConfirm = menuInput.DidPressConfirm();
    if (didPressConfirm)
    {
      SelectCurrentChoice();
    }
    else
    {
      int navDirection = menuInput.GetNavDirection();
      if (canMove && navDirection != 0)
      {
        focusedChoice = Mathf.Clamp(focusedChoice + navDirection, 0, choices.Count - 1);
        OnButtonFocusChanged();
        DebounceMoves();
      }
    }
  }

  public void Activate(List<PowerupChoiceDisplay> choices, OnSelected onSelectedCallback)
  {
    this.choices = choices;
    this.onSelectedCallback = onSelectedCallback;
    OnButtonFocusChanged();
    this.isActive = true;
    if (unlockMoveCoroutine != null)
    {
      StopCoroutine(unlockMoveCoroutine);
    }
    canMove = true;
  }

  public void Deactivate()
  {
    this.choices = null;
    this.onSelectedCallback = null;
    this.isActive = false;
  }

  private void OnButtonFocusChanged()
  {
    for (int i = 0; i < choices.Count; i++)
    {
      bool isFocused = focusedChoice == i;
      choices[i].SetFocused(isFocused);
    }
  }

  private void DebounceMoves()
  {
    StopUnlockMovesCoroutine();

    unlockMoveCoroutine = _DebounceMovesCoroutine();
    StartCoroutine(unlockMoveCoroutine);
  }

  private IEnumerator _DebounceMovesCoroutine()
  {
    canMove = false;
    yield return new WaitForSecondsRealtime(movementDebounceSecs);
    canMove = true;
  }

  private void SelectCurrentChoice()
  {
    StopUnlockMovesCoroutine();

    canMove = false;
    if (onSelectedCallback != null)
    {
      onSelectedCallback(choices[focusedChoice]);
    }

    // TODO
  }

  private void StopUnlockMovesCoroutine()
  {
    if (unlockMoveCoroutine != null)
    {
      StopCoroutine(unlockMoveCoroutine);
    }
  }
}
