using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

  [SerializeField]
  private GameObject prototype;

  public void Explode()
  {
    GameObject tempGo = GameObject.Instantiate(prototype, Vector3.zero, Quaternion.identity) as GameObject;
    tempGo.transform.position = transform.position;
  }
}
