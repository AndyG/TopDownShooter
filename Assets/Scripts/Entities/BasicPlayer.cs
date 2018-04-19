using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicPlayer : MonoBehaviour
{

  public GameObject SpawnObject;

  public Sprite defaultSprite;
  public Sprite dashingSprite;

  public float topSpeedX = 50f;
  public float topSpeedY = 50f;
  public float acceleration = 10f;

  public float dashDuration = 10f;
  public float dashChargeDuration = 30f;

  public float dashSpeed = 30f;

  public bool afterImages = true;
  public GameObject weaponSupplier;
  public GameObject inputManagerSupplier;
  private Rigidbody2D rigidBody;

  private float timeSinceDashStart = 0f;
  private float dashChargeTime = 0f;

  private DashState dashState = DashState.READY;

  private Animator animator;
  // private SpriteRenderer spriteRenderer;
  private InputManager inputManager;

  [SerializeField]
  private GameObject barneyRendererSupplier;

  private BarneyRenderer barneyRenderer;

  // Use this for initialization
  void Start()
  {
    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

    // spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    // spriteRenderer.sprite = defaultSprite;

    inputManager = inputManagerSupplier.GetComponent<InputManager>();
    barneyRenderer = barneyRendererSupplier.GetComponent<BarneyRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    switch (dashState)
    {
      case DashState.READY:
        if (!performDash())
        {
          processMove();
          processShoot();
          barneyRenderer.update(getBucketedAimDirection(getRunDirection()), getBucketedRunDirection(getRunDirection()));
        }
        break;
      case DashState.DASHING:
        timeSinceDashStart += Time.deltaTime * 60;
        if (timeSinceDashStart > dashDuration)
        {
          dashState = DashState.READY;
          // spriteRenderer.sprite = defaultSprite;
          Debug.Log("setting dash state READY");
          setVelocity(0, 0);
          processMove();
        }
        else
        {
          leaveAfterImage();
        }
        break;
    }
  }

  public void onHit()
  {
    if (dashState == DashState.DASHING)
    {
      // invincible
      return;
    }

    SceneManager.LoadScene("TestScene");
  }

  private void leaveAfterImage()
  {
    GameObject tempGo = GameObject.Instantiate(SpawnObject, Vector3.zero, Quaternion.identity) as GameObject;
    tempGo.transform.position = transform.position;
    // tempGo.transform.parent = transform;
    // tempGo.transform.localPosition = transform.forward;
  }

  private bool performDash()
  {
    if (inputManager.isDashPressed())
    {
      // spriteRenderer.sprite = dashingSprite;
      float horizInput = inputManager.getMoveAxisHorizontal();
      float verticalInput = inputManager.getMoveAxisVertical();

      float dashXSpeed = 0;
      if (horizInput < 0)
      {
        dashXSpeed = dashSpeed * -1;
      }
      else if (horizInput > 0)
      {
        dashXSpeed = dashSpeed;
      }

      float dashYSpeed = 0;
      if (verticalInput < 0)
      {
        dashYSpeed = dashSpeed * -1;
      }
      else if (verticalInput > 0)
      {
        dashYSpeed = dashSpeed;
      }

      setVelocity(dashXSpeed, dashYSpeed);

      dashState = DashState.DASHING;
      timeSinceDashStart = 0f;

      return true;
    }
    else
    {
      return false;
    }
  }

  private void processMove()
  {
    float horizInput = inputManager.getMoveAxisHorizontal();
    float verticalInput = inputManager.getMoveAxisVertical();

    if (horizInput == 0)
    {
      setVelocityX(0);
    }
    else
    {
      float force = (horizInput) * acceleration;
      rigidBody.AddForce(Vector2.right * force, ForceMode2D.Impulse);
    }

    if (verticalInput == 0)
    {
      setVelocityY(0);
    }
    else
    {
      float force = (verticalInput) * acceleration;
      rigidBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    capVelocityX();
    capVelocityY();
  }

  /**
   * Various helper methods for updating position or velocity.
   */
  private void setVelocityX(float x)
  {
    setVelocity(x, rigidBody.velocity.y);
  }

  private void setVelocityY(float y)
  {
    setVelocity(rigidBody.velocity.x, y);
  }

  private void setVelocity(float x, float y)
  {
    rigidBody.velocity = new Vector2(x, y);
  }

  private void capVelocityX()
  {
    float currentHorizVelocity = rigidBody.velocity.x;
    if (Mathf.Abs(currentHorizVelocity) > topSpeedX)
    {
      int multiplier = currentHorizVelocity > 0 ? 1 : -1;
      setVelocityX(topSpeedX * multiplier);
    }
  }

  private void capVelocityY()
  {
    float currentVelocityY = rigidBody.velocity.y;
    if (Mathf.Abs(currentVelocityY) > topSpeedY)
    {
      int multiplier = currentVelocityY > 0 ? 1 : -1;
      setVelocityY(topSpeedY * multiplier);
    }
  }

  private void updateAnimator()
  {
    animator.SetBool("Dashing", dashState == DashState.DASHING);
  }

  private void processShoot()
  {
    Vector2? aimDirection = getAimDirection();
    if (!aimDirection.HasValue)
    {
      return;
    }

    Vector2 aimDirectionResolved = aimDirection.Value;
    Debug.DrawRay(transform.position, aimDirectionResolved, Color.green);
    Weapon weapon = weaponSupplier.GetComponent<Weapon>();
    weapon.use(aimDirectionResolved);
  }

  private Vector2? getAimDirection()
  {
    float horizInput = inputManager.getAimAxisHorizontal();
    float verticalInput = inputManager.getAimAxisVertical();
    float threshold = 0.5f;
    Vector2 direction = new Vector2(horizInput, verticalInput);
    if (direction.sqrMagnitude > threshold)
    {
      return direction.normalized;
    }
    else
    {
      return null;
    }
  }

  private Vector2? getRunDirection()
  {
    return new Vector2(inputManager.getMoveAxisHorizontal(), inputManager.getMoveAxisVertical());
  }

  private BarneyRenderer.RunDirection? getBucketedRunDirection(Vector2? rawDirection)
  {
    if (rawDirection == null)
    {
      return null;
    }

    float y = rawDirection.Value.y;
    float x = rawDirection.Value.x;

    // actual conversion code:
    float angle = Mathf.Atan2(y, x);
    float angleDegrees = RadianToDegree(angle);

    BarneyRenderer.RunDirection runDirection = BarneyRenderer.RunDirection.UP;

    if (angleDegrees >= -45 && angleDegrees < 45)
    {
      runDirection = BarneyRenderer.RunDirection.RIGHT;
    }
    else if (angleDegrees >= 45 && angleDegrees < 135)
    {

      runDirection = BarneyRenderer.RunDirection.UP;
    }
    else if (angleDegrees >= -135 && angleDegrees < -45)
    {

      runDirection = BarneyRenderer.RunDirection.DOWN;
    }
    else
    {
      runDirection = BarneyRenderer.RunDirection.LEFT;
    }

    return runDirection;
  }

  private float RadianToDegree(float angle)
  {
    return angle * (180.0f / Mathf.PI);
  }

  private float DegreesToRadians(float angle)
  {
    return angle / 180.0f * Mathf.PI;
  }

  private BarneyRenderer.AimDirection? getBucketedAimDirection(Vector2? rawDirection)
  {
    if (rawDirection == null)
    {
      return null;
    }

    float y = rawDirection.Value.y;
    float x = rawDirection.Value.x;

    // actual conversion code:
    float angle = Mathf.Atan2(y, x);
    float angleDegrees = RadianToDegree(angle);
    if (angleDegrees < 0)
    {
      angleDegrees += 360;
    }

    BarneyRenderer.AimDirection aimDirection = BarneyRenderer.AimDirection.UP;

    if (angleDegrees >= 337.5 || angleDegrees < 22.5)
    {
      aimDirection = BarneyRenderer.AimDirection.RIGHT;
    }
    else if (angleDegrees >= 22.5 && angleDegrees < 67.5)
    {
      aimDirection = BarneyRenderer.AimDirection.UP_RIGHT;
    }
    else if (angleDegrees >= 67.5 && angleDegrees < 112.5)
    {
      aimDirection = BarneyRenderer.AimDirection.UP;
    }
    else if (angleDegrees >= 112.5 && angleDegrees < 157.5)
    {
      aimDirection = BarneyRenderer.AimDirection.UP_LEFT;
    }
    else if (angleDegrees >= 157.5 && angleDegrees < 202.5)
    {
      aimDirection = BarneyRenderer.AimDirection.LEFT;
    }
    else if (angleDegrees >= 202.5 && angleDegrees < 247.5)
    {
      aimDirection = BarneyRenderer.AimDirection.DOWN_LEFT;
    }
    else if (angleDegrees >= 247.5 && angleDegrees < 292.5)
    {
      aimDirection = BarneyRenderer.AimDirection.DOWN;
    }
    else if (angleDegrees >= 292.5 && angleDegrees < 337.5)
    {
      aimDirection = BarneyRenderer.AimDirection.DOWN_RIGHT;
    }

    return aimDirection;
  }

  private enum DashState
  {
    READY,
    DASHING
  }
}
