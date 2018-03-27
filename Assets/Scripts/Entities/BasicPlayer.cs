using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour
{

  public GameObject SpawnObject;

  public Sprite defaultSprite;
  public Sprite dashingSprite;
  public AudioSource audioSource;
  public AudioClip dashChargeSound;
  public AudioClip dashExecuteSound;

  public float topSpeedX = 50f;
  public float topSpeedY = 50f;
  public float acceleration = 10f;

  public float dashDuration = 10f;
  public float dashChargeDuration = 30f;

  public float dashSpeed = 30f;

  public bool afterImages = true;
  private Rigidbody2D rigidBody;

  private float timeSinceDashStart = 0f;
  private float dashChargeTime = 0f;

  private DashState dashState = DashState.READY;

  private Animator animator;
  private SpriteRenderer spriteRenderer;

  private float dashXForce = 0;
  private float dashYForce = 0;

  // Use this for initialization
  void Start()
  {
    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = defaultSprite;
  }

  // Update is called once per frame
  void Update()
  {
    switch (dashState)
    {
      case DashState.CHARGING:
        dashChargeTime += Time.deltaTime * 60;
        if (dashChargeTime >= dashChargeDuration)
        {
          spriteRenderer.sprite = defaultSprite;
          executeDash();
        }
        break;
      case DashState.READY:
        if (!performDash())
        {
          processMove();
        }
        break;
      case DashState.DASHING:
        timeSinceDashStart += Time.deltaTime * 60;
        if (timeSinceDashStart > dashDuration)
        {
          dashState = DashState.READY;
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

  private void leaveAfterImage()
  {
    GameObject tempGo = GameObject.Instantiate(SpawnObject, Vector3.zero, Quaternion.identity) as GameObject;
    tempGo.transform.position = transform.position;
    // tempGo.transform.parent = transform;
    // tempGo.transform.localPosition = transform.forward;
  }

  private bool performDash()
  {
    if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
    {
      spriteRenderer.sprite = dashingSprite;
      dashState = DashState.CHARGING;
      audioSource.PlayOneShot(dashChargeSound);
      float horizInput = Input.GetAxis("Horizontal");
      float verticalInput = Input.GetAxis("Vertical");

      dashXForce = (horizInput);
      dashYForce = (verticalInput);
      setVelocity(0, 0);
      return true;
    }
    else
    {
      return false;
    }
  }

  private void executeDash()
  {
    dashChargeTime = 0;
    dashState = DashState.DASHING;
    timeSinceDashStart = 0f;

    float forceX = (dashXForce) * dashSpeed;
    float forceY = (dashYForce) * dashSpeed;
    Debug.Log("x: " + forceX + ", y: " + forceY);
    rigidBody.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
  }

  private void processMove()
  {
    float horizInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

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

  private enum DashState
  {
    READY,
    DASHING,
    CHARGING
  }
}
