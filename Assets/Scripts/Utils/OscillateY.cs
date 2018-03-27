using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateY : MonoBehaviour
{

  public float delta = 1.5f;
  public float speed = 2.0f;
  private Vector3 startPos;

  public float timeOffset;

  public OscillateAxis oscillateAxis = OscillateAxis.Y;

  void Start()
  {
    startPos = transform.position;
    timeOffset = Random.value;
  }

  void Update()
  {
    Vector3 dest = startPos;

    float step = delta * Mathf.Sin((Time.time - timeOffset) * speed);
    if (oscillateAxis == OscillateAxis.Y)
    {
      dest.y += step;
    }
    else if (oscillateAxis == OscillateAxis.X)
    {
      dest.x += step;
    }

    transform.position = dest;
  }

  public enum OscillateAxis
  {
    X, Y
  }
}
