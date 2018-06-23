using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class BasicPlayer : MonoBehaviour, PickupReceiver, WeaponUser
{

  public delegate void OnPlayerDeath();
  public event OnPlayerDeath OnPlayerDeathEvent;

  public delegate void OnBombCountChanged(int bombBount);
  public event OnBombCountChanged OnBombCountChangedEvent;

  public LevelChanger levelChanger;

  [SerializeField]
  private int playerId = 0;

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

  [SerializeField]
  private GameObject barneyRendererSupplier;

  private BarneyRenderer barneyRenderer;

  private OscillateSize oscillateSize;

  [SerializeField]
  private GameObject explosion;

  [SerializeField]
  private bool isPoweredUp;

  private Player player;

  // Use this for initialization
  void Start()
  {
    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();

    barneyRenderer = barneyRendererSupplier.GetComponent<BarneyRenderer>();

    player = ReInput.players.GetPlayer(playerId);
    setBombCount(bombCount);
  }

  void OnEnable()
  {
    setBombCount(bombCount);
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.timeScale <= 0)
    {
      return;
    }

    if (player.GetButtonDown("Skill 1"))
    {
      Debug.Log("pressed skill 1");
      levelChanger.FadeToScene(1);
      // dropBomb();
    }

    timeSincePowerUp += Time.deltaTime;
    if (isPoweredUp && timeSincePowerUp > 5)
    {
      barneyRenderer.setColorFilter(Color.white);
      isPoweredUp = false;
    }

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

  public void onHit()
  {
    if (dashState == DashState.DASHING)
    {
      // invincible
      return;
    }

    explode();
    OnPlayerDeathEvent();
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

  private void processMove()
  {
    float horizInput = player.GetAxis("Move Horizontal");
    float verticalInput = player.GetAxis("Move Vertical");

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
    float horizInput = player.GetAxis("Aim Horizontal"); ;
    float verticalInput = player.GetAxis("Aim Vertical");
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
    return new Vector2(player.GetAxis("Move Horizontal"), player.GetAxis("Move Vertical"));
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
      setBombCount(bombCount - 1);
    }
  }

  private void setBombCount(int bombCount)
  {
    this.bombCount = bombCount;
    if (OnBombCountChangedEvent != null)
    {
      OnBombCountChangedEvent(bombCount);
    }
  }

  public int getBombCount()
  {
    return bombCount;
  }

  public float forceAction = 0.05f;
  public void OnUse(Vector3 direction)
  {
    Vector3 position = transform.position;
    transform.position += -direction * forceAction;
    // rigidBody.AddForce(-direction * force, ForceMode2D.Impulse);
  }
}
