using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : MonoBehaviour
{

  [SerializeField]
  private bool lockInputAfterPress = true;

  [SerializeField]
  private List<Button> buttons;

  private int focusedButton = 0;

  private MainMenuInput menuInput;

  [SerializeField]
  private float movementDebounceSecs = 0.125f;

  [SerializeField]
  private float buttonPressTimeSecs = 0.125f;

  private bool canMove = true;
  private bool canInput = true;

  private IEnumerator unlockMoveCoroutine;

  // Use this for initialization
  void Start()
  {
    menuInput = GameObject.FindObjectOfType<MainMenuInput>();
    OnButtonFocusChanged();
  }

  // Update is called once per frame
  void Update()
  {
    if (!canInput)
    {
      return;
    }

    menuInput.GatherInput();

    bool didPressConfirm = menuInput.DidPressConfirm();
    if (didPressConfirm)
    {
      OnButtonPressed();
    }

    int navDirection = menuInput.GetNavDirection();
    if (canMove && navDirection != 0)
    {
      // Subtract because navDirection up is positive but buttons are
      // indexed top-to-bottom.
      focusedButton = Mathf.Clamp(focusedButton - navDirection, 0, buttons.Count - 1);
      OnButtonFocusChanged();
      DebounceMoves();
    }
  }

  private void OnButtonPressed()
  {
    Debug.Log("clicked button " + focusedButton);
    StartCoroutine(ClickButton());
    GetButtonAnimator(focusedButton).SetInteger("ButtonState", 2);
  }

  private void OnButtonFocusChanged()
  {
    for (int i = 0; i < buttons.Count; i++)
    {
      int buttonState = focusedButton == i ? 1 : 0;
      GetButtonAnimator(i).SetInteger("ButtonState", buttonState);
    }
  }

  private void DebounceMoves()
  {
    if (unlockMoveCoroutine != null)
    {
      StopCoroutine(unlockMoveCoroutine);
    }

    unlockMoveCoroutine = _DebounceMovesCoroutine();
    StartCoroutine(unlockMoveCoroutine);
  }

  private IEnumerator _DebounceMovesCoroutine()
  {
    canMove = false;
    yield return new WaitForSeconds(movementDebounceSecs);
    canMove = true;
  }

  private IEnumerator ClickButton()
  {
    if (unlockMoveCoroutine != null)
    {
      StopCoroutine(unlockMoveCoroutine);
    }

    canMove = false;

    Animator buttonAnimator = GetButtonAnimator(focusedButton);
    buttonAnimator.SetInteger("ButtonState", 2);

    yield return new WaitForSeconds(buttonPressTimeSecs);

    buttonAnimator.SetInteger("ButtonState", 1);
    if (lockInputAfterPress)
    {
      canInput = false;
    }
    else
    {
      canMove = true;
    }

    buttons[focusedButton].onClick.Invoke();
  }

  private Animator GetButtonAnimator(int buttonIndex)
  {
    return buttons[buttonIndex].GetComponent<Animator>();
  }
}
