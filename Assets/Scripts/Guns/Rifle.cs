using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour, Weapon
{

  private static float MAX_RATE_OF_FIRE = 20;
  private static float POWER_UP_DURATION_SECONDS = 5;

  [SerializeField]
  private float puScale = 4;

  public GameObject projectile;
  public float speed;

  [SerializeField]
  [Range(1, 30)]
  private float baseRateOfFire = 5;

  [SerializeField]
  private float rateOfFire = 1;

  private float timeSinceLastShot;
  private float timeSincePowerUp = POWER_UP_DURATION_SECONDS;

  // Use this for initialization
  void Start()
  {
    rateOfFire = baseRateOfFire;
    timeSinceLastShot = rateOfFire;
    timeSincePowerUp = POWER_UP_DURATION_SECONDS;
  }

  // Update is called once per frame
  void Update()
  {
    timeSinceLastShot += Time.deltaTime;
    timeSincePowerUp += Time.deltaTime;
    if (timeSincePowerUp >= POWER_UP_DURATION_SECONDS)
    {
      rateOfFire = baseRateOfFire;
    }
    // Vector2? aimDirection = getAimDirection();
    // if (!aimDirection.HasValue)
    // {
    //   return;
    // }

    // Vector2 aimDirectionResolved = aimDirection.Value;
    // Debug.DrawRay(transform.position, aimDirectionResolved, Color.green);
    // if (Input.GetKeyDown(KeyCode.Space) || isControllerConnected())
    // {
    //   shoot(aimDirectionResolved, speed);
    // }

  }

  public void use(Vector2 direction)
  {
    if (timeSinceLastShot < (1 / rateOfFire))
    {
      return;
    }
    Debug.Log("USE");
    GameObject tempGo = GameObject.Instantiate(projectile, Vector3.zero, Quaternion.identity) as GameObject;
    Rigidbody2D rigidBody = tempGo.GetComponent<Rigidbody2D>();
    tempGo.transform.position = transform.position;
    if (isPoweredUp())
    {
      tempGo.transform.localScale = new Vector3(puScale, puScale, tempGo.transform.localScale.z);
    }
    rigidBody.velocity = direction * speed;

    timeSinceLastShot = 0;
  }

  private bool isPoweredUp()
  {
    return timeSincePowerUp < POWER_UP_DURATION_SECONDS;
  }

  public void powerUp()
  {
    timeSincePowerUp = 0;
    rateOfFire = MAX_RATE_OF_FIRE;
  }

  // private Vector2? getAimDirection()
  // {
  //   if (isControllerConnected())
  //   {
  //     return getAimDirectionController();
  //   }
  //   else
  //   {
  //     return getAimDirectionMouse();
  //   }
  // }

  // private bool isControllerConnected()
  // {
  //   return true;
  // }

  private Vector2 getAimDirectionMouse()
  {
    Vector3 p = new Vector3();
    Camera c = Camera.main;
    Vector2 mousePos = new Vector2();

    // Get the mouse position from Event.
    // Note that the y position from Event is inverted.
    mousePos.x = Input.mousePosition.x;
    mousePos.y = Input.mousePosition.y;

    p = c.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
    Vector2 direction = p - transform.position;
    return direction.normalized;
  }

  private void shoot(Vector2 direction, float speed)
  {
    GameObject tempGo = GameObject.Instantiate(projectile, Vector3.zero, Quaternion.identity) as GameObject;
    Rigidbody2D rigidBody = tempGo.GetComponent<Rigidbody2D>();
    tempGo.transform.position = transform.position;
    rigidBody.velocity = direction * speed;
  }
}
