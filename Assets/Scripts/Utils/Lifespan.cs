using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour
{

  [SerializeField]
  private float lifespanSecs;

  void Start()
  {
    StartCoroutine(Destroy());
  }

  private IEnumerator Destroy()
  {
    yield return new WaitForSeconds(lifespanSecs);
    GameObject.Destroy(gameObject);
  }
}
