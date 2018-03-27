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
    if (Input.GetKeyDown(KeyCode.Space))
    {
      shoot();
    }
  }

  private void shoot()
  {
    Vector3 p = new Vector3();
    Camera c = Camera.main;
    Vector2 mousePos = new Vector2();

    // Get the mouse position from Event.
    // Note that the y position from Event is inverted.
    mousePos.x = Input.mousePosition.x;
    mousePos.y = c.pixelHeight - Input.mousePosition.y;

    p = c.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));


    GameObject tempGo = GameObject.Instantiate(projectile, Vector3.zero, Quaternion.identity) as GameObject;
    Rigidbody2D rigidBody = tempGo.GetComponent<Rigidbody2D>();
    tempGo.transform.position = transform.position;
    Vector2 direction = transform.position - p;
    rigidBody.velocity = direction;
  }
}
