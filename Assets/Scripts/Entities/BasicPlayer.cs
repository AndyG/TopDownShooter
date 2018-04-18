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
  private SpriteRenderer spriteRenderer;
  private InputManager inputManager;

  // Use this for initialization
  void Start()
  {
    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = defaultSprite;

    inputManager = inputManagerSupplier.GetComponent<InputManager>();
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
        }
        break;
      case DashState.DASHING:
        timeSinceDashStart += Time.deltaTime * 60;
        if (timeSinceDashStart > dashDuration)
        {
          dashState = DashState.READY;
          spriteRenderer.sprite = defaultSprite;
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
      Debug.Log(dashingSprite);
      spriteRenderer.sprite = dashingSprite;
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


  private enum DashState
  {
    READY,
    DASHING
  }
}
