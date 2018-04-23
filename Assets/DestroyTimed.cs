using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimed : MonoBehaviour
{

  [SerializeField]
  private float lifespan;

  private float timeAlive = 0f;
  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    timeAlive += Time.deltaTime;

    if (timeAlive > lifespan)
    {
      Destroy(this.gameObject);
    }
  }
}
