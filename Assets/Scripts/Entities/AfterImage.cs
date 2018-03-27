
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{

  public float duration = 2f;

  private float existenceTime = 0;

  private SpriteRenderer spriteRenderer;

  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    float percent = 1 - (existenceTime / duration);
    existenceTime += Time.deltaTime;
    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, percent);

    if (existenceTime >= duration)
    {
      Destroy(gameObject);
    }
  }
}
