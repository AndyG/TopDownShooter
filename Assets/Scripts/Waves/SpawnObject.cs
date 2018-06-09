using UnityEngine;

[System.Serializable]
public class SpawnObject : System.Object
{

  [SerializeField]
  private GameObject gameObject;

  [SerializeField]
  private float weight;

  public GameObject getGameObject()
  {
    return gameObject;
  }

  public float getWeight()
  {
    return weight;
  }
}