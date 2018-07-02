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

  public delegate void OnHitPointsChanged(int hitPoints);
  public event OnHitPointsChanged OnHitPointsChangedEvent;

  private delegate void OnHitKnockbackEnded();

  private PlayerInput playerInput;

  [SerializeField]
  private bool inputLocked = false;

  [SerializeField]
  private int bombCount = 3;

  [SerializeField]
  private Camera camera;
  public GameObject SpawnObject;

  public Sprite defaultSprite;

  [SerializeField]
  private GameObject bomb;

  public float topSpeedX = 50f;
  public float topSpeedY = 50f;
  public float acceleration = 10f;

  public GameObject weaponSupplier;
  private Rigidbody2D rigidBody;

  private float timeSincePowerUp = 0f;

  private Animator animator;

  private BarneyRenderer barneyRenderer;

  private Explosion explosion;

  [SerializeField]
  private bool isPoweredUp;

  [Header("On Hit")]
  [SerializeField]
  private float hitKnockbackDurationSecs = 0.05f;

  [SerializeField]
  private float hitInvulDuration = 0.05f;

  [SerializeField]
  private float hitKnockbackForce = 20;

  [SerializeField]
  private bool isInvulnerable;

  [SerializeField]
  [Tooltip("How fast the character flashes after taking damage.")]
  private float flashInterval = 0.05f;

  private bool hitThisFrame = false;

  private HitPointManager hitPointManager;

  [Header("Dash")]
  [SerializeField]
  private float dashForce = 20f;
  [SerializeField]
  private float dashDurationSecs = 0.25f;
  private bool isDashing;

  private Vector2 currentDashDirection;

  private IEnumerator currentDashCoroutine;

  // Use this for initialization
  void Start()
  {
    rigidBody = gameObject.GetComponent<Rigidbody2D>();
    animator = gameObject.GetComponent<Animator>();
    playerInput = gameObject.GetComponent<PlayerInput>();
    barneyRenderer = gameObject.GetComponentInChildren<BarneyRenderer>();

    explosion = gameObject.GetComponent<Explosion>();
    hitPointManager = gameObject.GetComponent<HitPointManager>();
    hitPointManager.OnHitPointsChangedEvent += _OnHitPointsChanged;

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

    if (!inputLocked)
    {
      playerInput.GatherInput();
    }

    timeSincePowerUp += Time.deltaTime;
    if (isPoweredUp && timeSincePowerUp > 5)
    {
      barneyRenderer.setColorFilter(Color.white);
      isPoweredUp = false;
    }

    Vector2? aimDirection = getAimDirection();
    Vector2 moveDirection;
    if (isDashing)
    {
      moveDirection = currentDashDirection;
    }
    else
    {
      moveDirection = playerInput.GetRunDirection();
    }

    if (!aimDirection.HasValue)
    {
      aimDirection = moveDirection;
    }

    if (playerInput.DidPressDash())
    {
      PerformDash(moveDirection);
    }
    else
    {
      if (playerInput.DidPressSkill1())
      {
        dropBomb();
      }

      processMove();
      processShoot();
    }

    barneyRenderer.update(aimDirection.Value, moveDirection, isDashing);
  }

  void LateUpdate()
  {
    hitThisFrame = false;
  }

  public void onHit(GameObject other)
  {
    if (isInvulnerable || hitThisFrame)
    {
      return;
    }

    StopDash();

    hitThisFrame = true;
    hitPointManager.decrement();

    Vector3 knockbackDir = (this.transform.position - other.transform.position).normalized;
    StartCoroutine(HitKnockback(knockbackDir, _OnHitKnockbackEnded));
  }

  private void _OnHitPointsChanged(int hitPoints)
  {
    if (hitPoints <= 0)
    {
      die();
    }

    if (OnHitPointsChangedEvent != null)
    {
      OnHitPointsChangedEvent(hitPoints);
    }
  }

  private IEnumerator HitKnockback(Vector2 direction, OnHitKnockbackEnded onEnded)
  {
    isInvulnerable = true;
    setVelocity(0f, 0f);
    rigidBody.AddForce(direction * hitKnockbackForce, ForceMode2D.Impulse);
    inputLocked = true;
    barneyRenderer.FlashWhite();

    yield return new WaitForSeconds(hitKnockbackDurationSecs);

    inputLocked = false;
    barneyRenderer.SetNormal();
    onEnded();
  }

  private void _OnHitKnockbackEnded()
  {
    StartCoroutine(_OnHitKnockbackEndedCoroutine());
  }

  private IEnumerator _OnHitKnockbackEndedCoroutine()
  {
    float timeInvul = 0f;
    float toggleTimer = 0f;
    while (timeInvul < hitInvulDuration)
    {
      timeInvul += Time.deltaTime;
      toggleTimer += Time.deltaTime;
      if (toggleTimer > flashInterval)
      {
        barneyRenderer.ToggleEnabled();
        toggleTimer = 0f;
      }
      yield return null;
    }

    barneyRenderer.SetEnabled(true);
    isInvulnerable = false;
  }

  public void onPickup()
  {
    Weapon weapon = weaponSupplier.GetComponent<Weapon>();
    weapon.powerUp();
    barneyRenderer.setColorFilter(Color.red);
    isPoweredUp = true;
    timeSincePowerUp = 0;
  }

  private void die()
  {
    if (explosion != null)
    {
      explosion.Explode();
    }

    Destroy(this.gameObject);
    OnPlayerDeathEvent();
  }

  private void processMove()
  {
    if (inputLocked) return;

    Vector2 moveDirection = playerInput.GetRunDirection();
    float horizInput = moveDirection.x;
    float verticalInput = moveDirection.y;

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

  private void processShoot()
  {
    Vector2? aimDirection = getAimDirection();
    if (!aimDirection.HasValue)
    {
      return;
    }

    Vector2 aimDirectionResolved = aimDirection.Value;
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
    Vector2 aimDirection = playerInput.GetAimDirection();
    if (aimDirection.x != 0 || aimDirection.y != 0)
    {
      return aimDirection;
    }
    else
    {
      return null;
    }
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

  private void PerformDash(Vector2 direction)
  {
    currentDashCoroutine = PerformDashCoroutine(direction);
    StartCoroutine(currentDashCoroutine);
  }

  private void StopDash()
  {
    isDashing = false;
    if (currentDashCoroutine != null)
    {
      StopCoroutine(currentDashCoroutine);
    }
  }

  private IEnumerator PerformDashCoroutine(Vector2 direction)
  {
    isDashing = true;
    inputLocked = true;

    currentDashDirection = direction;

    setVelocity(0f, 0f);
    rigidBody.AddForce(direction * dashForce, ForceMode2D.Impulse);

    yield return new WaitForSeconds(dashDurationSecs);

    isDashing = false;
    inputLocked = false;
    currentDashCoroutine = null;
    currentDashDirection = Vector2.zero;
  }

  public int getBombCount()
  {
    return bombCount;
  }

  // Weapon was used.
  public void OnUse(Vector3 direction)
  {
  }
}
