using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour, IDirectable
{

  [SerializeField]
  [Range(0.1f, 30f)]
  private float speed = 5f;

  private Vector2 direction;

  private Rigidbody2D rigidBody;

  [Header("Smoke")]
  [SerializeField]
  private GameObject smokePrototype;
  [SerializeField]
  private float smokeInterval = 0.1f;

  // Use this for initialization
  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    StartCoroutine(DropSmoke());
  }

  void FixedUpdate()
  {
    Move();
  }

  public void SetDirection(Vector2 direction)
  {
    float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;
    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
  }

  private void Move()
  {
    rigidBody.velocity = -transform.up * speed;
  }

  private IEnumerator DropSmoke()
  {
    while (true)
    {
      yield return new WaitForSeconds(smokeInterval);
      GameObject tempGo = GameObject.Instantiate(smokePrototype, Vector3.zero, Quaternion.identity) as GameObject;
      tempGo.transform.position = transform.position;
    }
  }
}
