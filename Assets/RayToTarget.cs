using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayToTarget : MonoBehaviour
{

  [SerializeField]
  private Transform target;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Vector3 dir = target.position - transform.position;
    Debug.DrawRay(transform.position, dir, Color.blue);

    Quaternion angle = Quaternion.AngleAxis(45, Vector3.back);
    Vector3 newDir = angle * dir;
    Debug.DrawRay(transform.position, newDir, Color.red);
  }
}
