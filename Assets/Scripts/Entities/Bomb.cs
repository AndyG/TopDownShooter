using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

  [SerializeField]
  private float scaleRate = 0.1f;

  private float fadeRate = 0.05f;

  private SpriteRenderer spriteRenderer;

  public float duration = 2f;

  private float existenceTime = 0;

  void Start()
  {
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
  }

  void Update()
  {
    transform.localScale += new Vector3(scaleRate, scaleRate, 0);
    float percent = 1 - (existenceTime / duration);
    existenceTime += Time.deltaTime;
    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, percent);

    if (existenceTime >= duration)
    {
      Destroy(gameObject);
    }
  }

  // void OnTriggerEnter2D(Collider2D other)
  // {
  //   Bombable bombable = other.GetComponent<Bombable>();
  //   if (bombable != null)
  //   {
  //     bombable.onBombed();
  //   }
  // }
}