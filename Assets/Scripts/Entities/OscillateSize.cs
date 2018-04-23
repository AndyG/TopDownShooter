using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateSize : MonoBehaviour
{

  Transform transform = null;

  [SerializeField]
  private float rate = 0.05f;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (transform != null)
    {
      Vector3 scale = transform.localScale;
      Vector3 newScale = new Vector3(scale.x + rate, scale.y + rate, scale.z);
      transform.localScale = newScale;
    }
  }

  public void setActive(Transform transform)
  {
    this.transform = transform;
  }
}
