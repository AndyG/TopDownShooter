using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Shootable
{

  public GameObject target;
  public float speed;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (target != null)
    {
      float step = speed * Time.deltaTime;
      transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }
  }

  public void handleShot(Bullet bullet)
  {
    Destroy(this.gameObject);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("on trigger enter");

    Bullet bullet = other.GetComponent<Bullet>();
    if (bullet != null)
    {
      handleShot(bullet);
      Destroy(bullet.gameObject);
      return;
    }

    BasicPlayer player = other.GetComponent<BasicPlayer>();
    if (player != null)
    {
      Destroy(player.gameObject);
      return;
    }
  }
}
