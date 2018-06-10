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

  public void spawnObjects(GameObject target)
  {
    for (int i = 0; i <= numToSpawn; i++)
    {
      _spawnObject(target);
    }
  }

  private void _spawnObject(GameObject target)
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
  }
}
