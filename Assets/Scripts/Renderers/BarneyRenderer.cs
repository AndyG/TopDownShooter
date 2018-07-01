using UnityEngine;

public class BarneyRenderer : MonoBehaviour
{
  [Header("Sprites")]
  [SerializeField]
  private GameObject legSprite;
  [SerializeField]
  private GameObject topSprite;
  [SerializeField]
  private GameObject fullBodySprite;

  [Header("Materials")]
  [SerializeField]
  private Material hitFlashMaterial;
  private Material defaultMaterial;

  private SpriteRenderer topSpriteRenderer;
  private SpriteRenderer legSpriteRenderer;

  private Animator topAnimator;
  private Animator legAnimator;

  void Start()
  {
    topAnimator = topSprite.GetComponent<Animator>();
    topSpriteRenderer = topSprite.GetComponent<SpriteRenderer>();
    legAnimator = legSprite.GetComponent<Animator>();
    legSpriteRenderer = legSprite.GetComponent<SpriteRenderer>();

    defaultMaterial = topSpriteRenderer.material;
  }

  public void update(Vector2 aimDirection, Vector2 moveDirection, bool isDashing)
  {
    if (isDashing)
    {
      UpdateDash(moveDirection);
      fullBodySprite.SetActive(true);
      topSprite.SetActive(false);
      legSprite.SetActive(false);
    }
    else
    {
      UpdateNormalState(aimDirection, moveDirection);
      fullBodySprite.SetActive(false);
      topSprite.SetActive(true);
      legSprite.SetActive(true);
    }
  }

  public void setColorFilter(UnityEngine.Color color)
  {
    topSpriteRenderer.color = color;
    legSpriteRenderer.color = color;
  }

  public void SetEnabled(bool enabled)
  {
    topSpriteRenderer.enabled = true;
    legSpriteRenderer.enabled = true;
  }

  public void ToggleEnabled()
  {
    topSpriteRenderer.enabled = !topSpriteRenderer.enabled;
    legSpriteRenderer.enabled = !legSpriteRenderer.enabled;
  }

  public void SetNormal()
  {
    topSpriteRenderer.material = defaultMaterial;
    legSpriteRenderer.material = defaultMaterial;
  }

  public void FlashWhite()
  {
    topSpriteRenderer.material = hitFlashMaterial;
    legSpriteRenderer.material = hitFlashMaterial;
  }

  private void UpdateNormalState(Vector2 aimDirection, Vector2 moveDirection)
  {
    float threshold = 0.5f;
    bool isAiming = aimDirection.sqrMagnitude >= threshold;

    Vector2 resolvedAimDirection = isAiming ? aimDirection : moveDirection;
    Vector2 bucketedAimDirection = getBucketedAimDirection(resolvedAimDirection);
    topAnimator.SetFloat("AimDirectionX", bucketedAimDirection.x);
    topAnimator.SetFloat("AimDirectionY", bucketedAimDirection.y);
    legAnimator.SetFloat("MoveX", moveDirection.x);
    legAnimator.SetFloat("MoveY", moveDirection.y);
  }

  private void UpdateDash(Vector2 moveDirection)
  {
    Vector2 bucketedMoveDirection = getBucketedAimDirection(moveDirection);
  }

  private Vector2 getBucketedAimDirection(Vector2 rawDirection)
  {
    float y = rawDirection.y;
    float x = rawDirection.x;

    // actual conversion code:
    float angle = Mathf.Atan2(y, x);
    float angleDegrees = RadianToDegree(angle);
    if (angleDegrees < 0)
    {
      angleDegrees += 360;
    }

    if (angleDegrees >= 337.5 || angleDegrees < 22.5)
    {
      return new Vector2(1, 0);
    }
    else if (angleDegrees >= 22.5 && angleDegrees < 67.5)
    {
      return new Vector2(1, 1);
    }
    else if (angleDegrees >= 67.5 && angleDegrees < 112.5)
    {
      return new Vector2(0, 1);
    }
    else if (angleDegrees >= 112.5 && angleDegrees < 157.5)
    {
      return new Vector2(-1, 1);
    }
    else if (angleDegrees >= 157.5 && angleDegrees < 202.5)
    {
      return new Vector2(-1, 0);
    }
    else if (angleDegrees >= 202.5 && angleDegrees < 247.5)
    {
      return new Vector2(-1, -1);
    }
    else if (angleDegrees >= 247.5 && angleDegrees < 292.5)
    {
      return new Vector2(0, -1);
    }
    // else if (angleDegrees >= 292.5 && angleDegrees < 337.5)
    else
    {
      return new Vector2(1, -1);
    }
  }

  private float RadianToDegree(float angle)
  {
    return angle * (180.0f / Mathf.PI);
  }

  private float DegreesToRadians(float angle)
  {
    return angle / 180.0f * Mathf.PI;
  }
}