using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SortOrderYTilemap : MonoBehaviour
{

  private TilemapRenderer tilemapRenderer;

  // Use this for initialization
  void Start()
  {
    tilemapRenderer = gameObject.GetComponent<TilemapRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    tilemapRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
  }
}
