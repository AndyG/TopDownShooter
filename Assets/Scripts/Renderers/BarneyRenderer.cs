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

  void Start()
  {
    topAnimator = topSprite.GetComponent<Animator>();
    legAnimator = legSprite.GetComponent<Animator>();
  }

  public void update(Vector2 aimDirection, Vector2 moveDirection)
  {
    float threshold = 0.5f;
    bool isAiming = aimDirection.sqrMagnitude >= threshold;

    Vector2 resolvedAimDirection = isAiming ? aimDirection : moveDirection;
    topAnimator.SetFloat("AimDirectionX", resolvedAimDirection.x);
    topAnimator.SetFloat("AimDirectionY", resolvedAimDirection.y);
    legAnimator.SetFloat("MoveX", moveDirection.x);
    legAnimator.SetFloat("MoveY", moveDirection.y);
    // if (runDirection != null)
    // {
    //   lastRunDirection = runDirection.Value;
    //   legAnimator.SetInteger("Direction", (int)runDirection.Value);
    // }
  }
}