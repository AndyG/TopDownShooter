using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicPlayer : MonoBehaviour, PickupReceiver, WeaponUser
{
  [SerializeField]
  private int bombCount = 3;

  [SerializeField]
  private Camera camera;
  public GameObject SpawnObject;

  public Sprite defaultSprite;
  public Sprite dashingSprite;

  [SerializeField]
  private GameObject bomb;

  public float topSpeedX = 50f;
  public float topSpeedY = 50f;
  public float acceleration = 10f;

  public float dashDuration = 10f;
  public float dashChargeDuration = 30f;

  public float dashSpeed = 30f;

  public bool afterImages = true;
  public GameObject weaponSupplier;
  private Rigidbody2D rigidBody;

  private float timeSinceDashStart = 0f;
  private float dashChargeTime = 0f;

  private float timeSincePowerUp = 0f;

  private DashState dashState = DashState.READY;

  private Animator animator;
  private InputManager inputManager;

  [SerializeField]
  private GameObject inputManagerProvider;

  [SerializeField]
  private GameObject barneyRendererSupplier;

  private BarneyRenderer barneyRenderer;

  private OscillateSize oscillateSize;

  [SerializeField]
  private GameSystem system;

  [SerializeField]
  private GameObject explosion;

  [SerializeField]
  private bool isPoweredUp;

  // Use this for initialization
  void Start()
  {
    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

    inputManager = inputManagerProvider.GetComponent<InputManager>();
    barneyRenderer = barneyRendererSupplier.GetComponent<BarneyRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.timeScale <= 0)
    {
      return;
    }

    if (inputManager.isButton0Pressed())
    {
      dropBomb();
    }

    timeSincePowerUp += Time.deltaTime;
    if (isPoweredUp && timeSincePowerUp > 5)
    {
      barneyRenderer.setColorFilter(Color.white);
      isPoweredUp = false;
    }

    switch (dashState)
    {
      case DashState.READY:
        if (!performDash())
        {
          processMove();
          processShoot();
          Vector2? aimDirection = getAimDirection();
          Vector2 runDirection = getRunDirection();

          if (!aimDirection.HasValue)
          {
            aimDirection = runDirection;
          }


          barneyRenderer.update(aimDirection.Value, runDirection);
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

    explode();
    system.HandlePlayerDeath();
  }

  public void onPickup()
  {
    Weapon weapon = weaponSupplier.GetComponent<Weapon>();
    weapon.powerUp();
    barneyRenderer.setColorFilter(Color.red);
    isPoweredUp = true;
    timeSincePowerUp = 0;
  }

  private void explode()
  {

    if (explosion != null)
    {
      GameObject tempGo = GameObject.Instantiate(explosion, Vector3.zero, Quaternion.identity) as GameObject;
      tempGo.transform.position = transform.position;
    }
    Destroy(this.gameObject);
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

    capVelocity();
  }

  private void capVelocity()
  {
    rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, topSpeedX);
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
    // Debug.DrawRay(transform.position, aimDirectionResolved, Color.green);
    if (weaponSupplier != null)
    {
      Weapon weapon = weaponSupplier.GetComponent<Weapon>();
      weapon.use(aimDirectionResolved);
      if (isPoweredUp)
      {
        camera.GetComponent<CameraControl>().Shake(0.05f, 20, 500f);
      }
    }
  }

  private Vector2? getAimDirection()
  {
    float horizInput = inputManager.getAimAxisHorizontal();
    float verticalInput = inputManager.getAimAxisVertical();
    Vector2 direction = new Vector2(horizInput, verticalInput);
    // Debug.Log("Base direction: " + direction);
    if (direction.x != 0 || direction.y != 0)
    {
      return direction.normalized;
    }
    else
    {
      return null;
    }
  }

  private Vector2 getRunDirection()
  {
    return new Vector2(inputManager.getMoveAxisHorizontal(), inputManager.getMoveAxisVertical());
  }

  private enum DashState
  {
    READY,
    DASHING
  }

  private void dropBomb()
  {
    if (bombCount > 0)
    {
      GameObject tempGo = GameObject.Instantiate(bomb, Vector3.zero, Quaternion.identity) as GameObject;
      tempGo.transform.position = transform.position;

      camera.GetComponent<CameraControl>().Shake(0.8f, 50, 270f);
      bombCount--;
    }
  }

  public void OnUse(Vector3 direction)
  {
    // float force = 3;
    // rigidBody.AddForce(-direction * force, ForceMode2D.Impulse);
  }
}
