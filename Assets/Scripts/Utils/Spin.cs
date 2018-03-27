
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

  public float speed = 50f;

  void Start()
  {
    gameObject.GetComponent<Rigidbody2D>().angularVelocity = speed;
  }
}
