using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittingEnemy : MonoBehaviour
{

  [SerializeField]
  private GameObject spawnObject;

  [SerializeField]
  private GameObject target;

  [SerializeField]
  private int numToSpawn;

  // Parent's spawner
  public void spawnObjects(GameObject target, Spawner spawner)
  {
    for (int i = 0; i <= numToSpawn; i++)
    {
      _spawnObject(target, spawner);
    }
  }

  private void _spawnObject(GameObject target, Spawner spawner)
  {
    GameObject tempGo = GameObject.Instantiate(spawnObject, Vector3.zero, Quaternion.identity) as GameObject;
    tempGo.transform.position = transform.position;

    if (target != null)
    {
      Targeter targeter = tempGo.GetComponent<Targeter>();
      if (targeter != null)
      {
        targeter.SetTarget(target);
      }
    }

    Enemy enemy = tempGo.GetComponent<Enemy>();
    if (enemy != null && spawner != null)
    {
      enemy.AttachSpawner(spawner);
    }
  }
}
