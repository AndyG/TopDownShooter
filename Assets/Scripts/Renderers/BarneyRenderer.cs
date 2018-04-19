using UnityEngine;

public class BarneyRenderer : MonoBehaviour
{
  [Header("Sprites")]

  [SerializeField]
  private GameObject legSprite;

  [SerializeField]
  private GameObject topSprite;

  private Animator topAnimator;
  private Animator legAnimator;

  private RunDirection lastRunDirection = RunDirection.UP;

  void Start()
  {
    topAnimator = topSprite.GetComponent<Animator>();
    legAnimator = legSprite.GetComponent<Animator>();
  }

  public void update(AimDirection? aimDirection, RunDirection? runDirection)
  {
    if (aimDirection != null)
    {
      topAnimator.SetInteger("Direction", (int)aimDirection);
    }
    if (runDirection != null)
    {
      lastRunDirection = runDirection.Value;
      legAnimator.SetInteger("Direction", (int)runDirection.Value);
    }
  }

  public enum AimDirection
  {
    UP,
    DOWN,
    LEFT,
    RIGHT,
    UP_LEFT,
    UP_RIGHT,
    DOWN_LEFT,
    DOWN_RIGHT
  }

  public enum RunDirection
  {
    UP,
    DOWN,
    LEFT,
    RIGHT
  }
}