using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

  private SpawnerConfig spawnerConfig;

  public GameObject target;
  private float timeSince = 0f;

  private float secs = 0f;

  // Use this for initialization
  void Start()
  {
    spawnerConfig = GetComponent<SpawnerConfig>();
  }

  // Update is called once per frame
  void Update()
  {
    secs += Time.deltaTime;
    timeSince += Time.deltaTime;
    if (timeSince >= spawnerConfig.getIntervalSeconds())
    {
      spawn();
      timeSince = 0f;
    }
  }

  private void spawn()
  {
    GameObject spawnObject = spawnerConfig.GetRandomSpawnObject();
    GameObject tempGo = GameObject.Instantiate(spawnObject, Vector3.zero, Quaternion.identity) as GameObject;
    tempGo.transform.position = transform.position;

    Targeter targeter = tempGo.GetComponent<Targeter>();
    targeter.SetTarget(target);
  }
}
