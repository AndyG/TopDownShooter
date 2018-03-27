// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ExamplePlayer : MonoBehaviour
// {

//   public bool flipped = false;
//   public float acceleration = 10f;
//   public bool infiniteAcceleration = true;
//   public float topSpeedX = 50f;
//   public float dashSpeed = 150f;
//   public float maxDashTime = 30f;
//   public float jumpPower = 10f;
//   public float minJumpMomentum = 4f;
//   public float postDashVelocityY = -5f;
//   public int maxAirdashesPerAirborne = 1;
//   public float jumpLeniency = 60f;
//   public float groundDetectionDistance = 5f;

//   public GameObject projectile;
//   public GameObject jetpack;

//   private Useable currentPowerup;

//   private Rigidbody2D rigidBody;
//   private Animator animator;

//   private SpriteFlipper spriteFlipper;
//   private GroundChecker groundChecker;
//   private WallPushChecker wallPushChecker;
//   private ProjectileShooter projectileShooter;

//   private DashState dashState;
//   private WallPushState wallPushState;
//   private float timeSinceDashStart;

//   private int airdashesSinceAirborne = 0;
//   private float timeSinceCouldJump = 0f;

//   private bool grounded = true;

//   private float intrinsicGravity;
//   private float intrinsicDrag;

//   public GameObject rotatingCenter;

//   void Start()
//   {
//     rigidBody = gameObject.GetComponent<Rigidbody2D>();
//     intrinsicGravity = rigidBody.gravityScale;
//     intrinsicDrag = rigidBody.drag;

//     animator = gameObject.GetComponent<Animator>();
//     int environmentLayerMask = 1 << LayerMask.NameToLayer("Environment");

//     spriteFlipper = new SpriteFlipper(delegate ()
//     {
//       return rigidBody.velocity.x < 0 || wallPushState == WallPushState.PUSHING_LEFT;
//     });

//     groundChecker = new GroundCheckerRays(gameObject.GetComponent<Collider2D>(), transform, environmentLayerMask);
//     wallPushChecker = new WallPushCheckerRays(gameObject.GetComponent<Collider2D>(), transform, environmentLayerMask);
//     projectileShooter = gameObject.GetComponentInChildren<ProjectileShooter>();
//   }

//   void Update()
//   {
//     grounded = groundChecker.isGrounded();
//     wallPushState = wallPushChecker.getWallPushState();

//     processClinging();

//     updateGravity();

//     switch (dashState)
//     {
//       case DashState.READY:
//         if (!performDash())
//         {
//           performHorizMove();
//         }
//         break;
//       case DashState.DASHING:
//         timeSinceDashStart += Time.deltaTime * 60;
//         if (timeSinceDashStart > maxDashTime)
//         {
//           dashState = DashState.READY;
//           setVelocity(0, postDashVelocityY);
//           performHorizMove();
//         }
//         break;
//     }

//     processJump();
//     processProjectileInput();
//     updateAnimator();
//     spriteFlipper.tryFlipSprite(transform);
//     projectileShooter.setIsFlipped(rigidBody.velocity.x < 0);
//     usePowerup();
//   }

//   // Returns true if a dash was performed.
//   private bool performDash()
//   {
//     if (!grounded && airdashesSinceAirborne >= maxAirdashesPerAirborne)
//     {
//       return false;
//     }

//     bool isDashKeyDown = Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift);
//     if (isDashKeyDown)
//     {
//       dashState = DashState.DASHING;

//       int directionMultiplier = Input.GetAxis("Horizontal") >= 0 ? 1 : -1;
//       setVelocity(dashSpeed * directionMultiplier, 0);
//       timeSinceDashStart = 0;

//       if (!grounded)
//       {
//         airdashesSinceAirborne++;
//       }

//       return true;
//     }

//     return false;
//   }

//   private void performHorizMove()
//   {
//     if (wallPushState != WallPushState.NONE)
//     {
//       return;
//     }

//     float horizInput = Input.GetAxis("Horizontal");

//     if (horizInput == 0)
//     {
//       setVelocityX(0);
//       return;
//     }

//     float force = (horizInput) * (infiniteAcceleration ? float.MaxValue : acceleration);
//     rigidBody.AddForce(Vector2.right * force, ForceMode2D.Impulse);
//     capVelocityX();
//   }

//   private void capVelocityX()
//   {
//     float currentHorizVelocity = rigidBody.velocity.x;
//     if (Mathf.Abs(currentHorizVelocity) > topSpeedX)
//     {
//       int multiplier = currentHorizVelocity > 0 ? 1 : -1;
//       setVelocityX(topSpeedX * multiplier);
//     }
//   }

//   private bool getCanJumpAndUpdateTimer()
//   {
//     bool clinging = wallPushState != WallPushState.NONE;
//     if (grounded || clinging)
//     {
//       timeSinceCouldJump = 0f;
//       airdashesSinceAirborne = 0;
//     }
//     else
//     {
//       timeSinceCouldJump += Time.deltaTime;
//     }

//     return grounded || clinging || timeSinceCouldJump < jumpLeniency;
//   }

//   private void processJump()
//   {
//     bool canJump = getCanJumpAndUpdateTimer();

//     if (Input.GetKeyDown("space") && canJump)
//     {
//       Vector3 up = transform.TransformDirection(Vector3.up);
//       setVelocityY(0);
//       rigidBody.AddForce(up * jumpPower, ForceMode2D.Impulse);
//       dashState = DashState.READY;
//     }

//     tryShortenJump();
//   }

//   private void tryShortenJump()
//   {
//     if (Input.GetKeyUp("space") && rigidBody.velocity.y > minJumpMomentum)
//     {
//       setVelocityY(minJumpMomentum);
//     }
//   }

//   private void processProjectileInput()
//   {
//     if (Input.GetKey("f"))
//     {
//       projectileShooter.shootProjectile(rigidBody.velocity);
//     }
//   }

//   private void processClinging()
//   {
//     if (isClinging())
//     {
//       setVelocityY(0);
//     }
//   }

//   private void updateGravity()
//   {
//     if (dashState == DashState.DASHING || isClinging())
//     {
//       rigidBody.drag = 0f;
//       float curVelocityY = rigidBody.velocity.y;
//       rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(curVelocityY, 0f));
//     }
//     else
//     {
//       rigidBody.drag = intrinsicDrag;
//     }
//   }

//   private bool isClinging()
//   {
//     return !grounded && wallPushState != WallPushState.NONE;
//   }

//   private void updateAnimator()
//   {
//     animator.SetFloat("Player_Velocity_Horiz", Mathf.Abs(rigidBody.velocity.x));
//     animator.SetBool("Player_Grounded", grounded);
//     animator.SetBool("Player_Dashing", (dashState == DashState.DASHING));
//     animator.SetBool("Player_Pushing_Wall", (grounded && (wallPushState != WallPushState.NONE)));
//     animator.SetBool("Player_Clinging_Wall", (!grounded && (wallPushState != WallPushState.NONE)));
//   }

//   /**
//    * Various helper methods for updating position or velocity.
//    */
//   private void setVelocityX(float x)
//   {
//     setVelocity(x, rigidBody.velocity.y);
//   }

//   private void setVelocityY(float y)
//   {
//     setVelocity(rigidBody.velocity.x, y);
//   }

//   private void setVelocity(float x, float y)
//   {
//     rigidBody.velocity = new Vector2(x, y);
//   }

//   private void usePowerup()
//   {
//     if (currentPowerup != null)
//     {
//       if (Input.GetKey(KeyCode.P))
//       {
//         currentPowerup.use(this);
//       }
//     }
//   }

//   public void pushUpwards(float force)
//   {
//     Vector3 up = transform.TransformDirection(Vector3.up);
//     rigidBody.AddForce(up * force, ForceMode2D.Impulse);
//   }

//   private void OnTriggerEnter2D(Collider2D collision)
//   {
//     if (collision.gameObject.layer == LayerMask.NameToLayer("Powerup"))
//     {
//       Powerup powerup = collision.gameObject.GetComponent<Powerup>();
//       processPowerup(powerup.powerupType);
//       powerup.onCollected();
//     }
//   }

//   private void processPowerup(Powerup.PowerupType powerupType)
//   {
//     switch (powerupType)
//     {
//       case Powerup.PowerupType.JETPACK:
//         break;
//     }

//     GameObject instantiated = Instantiate(jetpack);
//     instantiated.transform.parent = transform;
//     instantiated.transform.position = transform.position;
//     instantiated.transform.position += (Vector3.back * 0.1f);
//     currentPowerup = instantiated.GetComponent<Useable>();
//   }

//   private enum DashState
//   {
//     READY,
//     DASHING
//   }
// }
