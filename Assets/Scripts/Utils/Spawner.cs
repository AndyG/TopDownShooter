using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

  public GameObject spawnObject;

  public GameObject target;
  public float interval;
  private float timeSince = 0f;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    timeSince += Time.deltaTime;
    if (timeSince >= interval)
    {
      spawn();
      timeSince = 0f;
    }
  }

  private void spawn()
  {
    GameObject tempGo = GameObject.Instantiate(spawnObject, Vector3.zero, Quaternion.identity) as GameObject;
    Enemy enemyScript = tempGo.GetComponent<Enemy>();
    tempGo.transform.position = transform.position;
    enemyScript.target = target;
  }
}
