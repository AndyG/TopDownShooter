using UnityEngine;

public class BarneyRenderer : MonoBehaviour
{
  [Header("Sprites")]

  [SerializeField]
  private GameObject legSprite;

  [SerializeField]
  private GameObject topSprite;

  private GameObject gun;

  private Material defaultMaterial;

  [SerializeField]
  private Material hitFlashMaterial;

  private SpriteRenderer topSpriteRenderer;
  private SpriteRenderer legSpriteRenderer;
  private SpriteRenderer gunSpriteRenderer;

  private Animator topAnimator;
  private Animator legAnimator;
  private Animator gunAnimator;

  void Start()
  {
    topAnimator = topSprite.GetComponent<Animator>();
    topSpriteRenderer = topSprite.GetComponent<SpriteRenderer>();

    legAnimator = legSprite.GetComponent<Animator>();
    legSpriteRenderer = legSprite.GetComponent<SpriteRenderer>();

    gun = GameObject.Find("Gun");
    gunSpriteRenderer = gun.GetComponentInChildren<SpriteRenderer>();
    gunAnimator = gun.GetComponent<Animator>();

    defaultMaterial = topSpriteRenderer.material;
  }

  public void update(Vector2 aimDirection, Vector2 moveDirection)
  {
    float threshold = 0.5f;
    bool isAiming = aimDirection.sqrMagnitude >= threshold;

    Vector2 resolvedAimDirection = isAiming ? aimDirection : moveDirection;
    float angleDegrees = DirectionToAngleDegrees(resolvedAimDirection);
    Vector2 bucketedAimDirection = getBucketedAimDirection(angleDegrees);
    topAnimator.SetFloat("AimDirectionX", bucketedAimDirection.x);
    topAnimator.SetFloat("AimDirectionY", bucketedAimDirection.y);
    legAnimator.SetFloat("MoveX", moveDirection.x);
    legAnimator.SetFloat("MoveY", moveDirection.y);

    // float bucketedAngle = Vector2.SignedAngle(Vector2.right, bucketedAimDirection);
    // gun.transform.rotation = Quaternion.AngleAxis(bucketedAngle, Vector3.forward);
    SetGunSprite(bucketedAimDirection);
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

  private void SetGunSprite(Vector2 direction)
  {
    if (direction == Vector2.up)
    {
      gunAnimator.SetInteger("GunState", 1);
    }
    else if (direction == Vector2.down)
    {
      gunAnimator.SetInteger("GunState", 2);
    }
    else
    {
      gunAnimator.SetInteger("GunState", 0);
    }

    float yRotation = (direction.x > 0) ? 0f : 180f;
    float zRotation = 0f;
    if (direction.x != 0 && direction.y != 0)
    {
      zRotation = direction.y > 0 ? 45f : -45f;
    }

    gun.transform.rotation = Quaternion.Euler(0, yRotation, zRotation);
  }

  private float DirectionToAngleDegrees(Vector2 direction)
  {
    float x = direction.x;
    float y = direction.y;

    // actual conversion code:
    float angle = Mathf.Atan2(y, x);
    float angleDegrees = RadianToDegree(angle);
    if (angleDegrees < 0)
    {
      angleDegrees += 360;
    }

    return angleDegrees;
  }

  private Vector2 getBucketedAimDirection(float angleDegrees)
  {
    if (angleDegrees >= 337.5 || angleDegrees < 22.5)
    {
      return Vector2.right;
    }
    else if (angleDegrees >= 22.5 && angleDegrees < 67.5)
    {
      return new Vector2(1, 1);
    }
    else if (angleDegrees >= 67.5 && angleDegrees < 112.5)
    {
      return Vector2.up;
    }
    else if (angleDegrees >= 112.5 && angleDegrees < 157.5)
    {
      return new Vector2(-1, 1);
    }
    else if (angleDegrees >= 157.5 && angleDegrees < 202.5)
    {
      return Vector2.left;
    }
    else if (angleDegrees >= 202.5 && angleDegrees < 247.5)
    {
      return new Vector2(-1, -1);
    }
    else if (angleDegrees >= 247.5 && angleDegrees < 292.5)
    {
      return Vector2.down;
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