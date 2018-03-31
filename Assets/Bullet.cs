using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("on trigger enter");
    Shootable shootable = other.GetComponent<Shootable>();
    if (shootable != null)
    {
      shootable.handleShot(this);
      Destroy(this.gameObject);
    }
  }
}
