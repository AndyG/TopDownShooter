using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderY : MonoBehaviour
{

  [SerializeField]
  private float positionOffset = 0f;

  private SpriteRenderer spriteRenderer;

  // Use this for initialization
  void Start()
  {
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    spriteRenderer.sortingOrder = Mathf.RoundToInt((transform.position.y + positionOffset) * 100f) * -1;
  }
}
