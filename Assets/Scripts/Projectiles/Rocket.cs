using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{

  [Header("Movement")]
  [SerializeField]
  [Range(0.1f, 30f)]
  private float speed = 5f;

  [Header("Smoke")]
  [SerializeField]
  private GameObject smokePrototype;
  [SerializeField]
  private float smokeInterval = 0.1f;

  [Header("Explosion")]
  [SerializeField]
  private GameObject explosionPrototype;

  private Vector2 direction;

  private Rigidbody2D rigidBody;

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

  void OnTriggerEnter2D(Collider2D other)
  {
    IHittable hittable = other.gameObject.GetComponent<IHittable>();
    if (hittable != null)
    {
      hittable.OnHit(this.gameObject);
      Explode();
    }
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
      GameObject smoke = GameObject.Instantiate(smokePrototype, Vector3.zero, Quaternion.identity) as GameObject;

      // Big rockets means big smoke ya dig.
      smoke.transform.localScale = this.transform.localScale;
      smoke.transform.position = transform.position;
    }
  }

  private void Explode()
  {
    if (explosionPrototype != null)
    {
      GameObject explosion = GameObject.Instantiate(explosionPrototype, this.transform.position, this.transform.rotation);
    }

    GameObject.Destroy(this.gameObject);
  }
}
