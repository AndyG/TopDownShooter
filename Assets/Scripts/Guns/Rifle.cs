using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{

  public GameObject projectile;
  public float speed;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Vector2? aimDirection = getAimDirection();
    if (!aimDirection.HasValue)
    {
      return;
    }

    Vector2 aimDirectionResolved = aimDirection.Value;
    Debug.DrawRay(transform.position, aimDirectionResolved, Color.green);
    if (Input.GetKeyDown(KeyCode.Space) || isControllerConnected())
    {
      shoot(aimDirectionResolved, speed);
    }
  }

  private Vector2? getAimDirection()
  {
    if (isControllerConnected())
    {
      return getAimDirectionController();
    }
    else
    {
      return getAimDirectionMouse();
    }
  }

  private bool isControllerConnected()
  {
    return true;
  }

  private Vector2? getAimDirectionController()
  {
    float horizInput = Input.GetAxis("RightHorizontal");
    float verticalInput = Input.GetAxis("RightVertical");
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
