using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerConfig : MonoBehaviour
{

  [SerializeField]
  private List<SpawnObject> spawnObjects;

  [SerializeField]
  private float intervalSecs = 1f;

  private float weightSum = 0;

  void Start()
  {
    if (!Application.isEditor)
    {
      computeWeightSum();
    }
  }

  void OnValidate()
  {
    computeWeightSum();
  }

  public float getIntervalSeconds()
  {
    return intervalSecs;
  }

  public GameObject GetRandomSpawnObject()
  {
    float rand = Random.Range(0f, weightSum);
    float min = 0f;
    foreach (SpawnObject spawnObject in spawnObjects)
    {
      float max = spawnObject.getWeight() + min;
      if (rand >= min && rand < max)
      {
        return spawnObject.getGameObject();
      }

      min = max;
    }

    throw new System.Exception("invalid rand in spawn config: " + rand + " with weightSum: " + weightSum);
  }

  private void computeWeightSum()
  {
    weightSum = 0f;
    foreach (SpawnObject spawnObject in spawnObjects)
    {
      weightSum += spawnObject.getWeight();
    }
  }
}