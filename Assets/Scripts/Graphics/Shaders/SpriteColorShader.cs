using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Example : MonoBehaviour
{
  private const string SHADER_COLOR_NAME = "_Color";
  private Material material;

  void Awake()
  {
    // makes a new instance of the material for runtime changes
    material = GetComponent<SpriteRenderer>().material;
  }

  private void Update()
  {
    if (Input.anyKey)
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        SetColor(Color.white);
      }

      if (Input.GetKeyDown(KeyCode.Backspace))
      {
        SetColor(new Color(1, 1, 1, 0));
      }
    }
  }

  private void SetColor(Color color)
  {
    material.SetColor(SHADER_COLOR_NAME, color);
  }
}