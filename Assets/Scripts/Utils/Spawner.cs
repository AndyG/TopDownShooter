using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

  public GameObject spawnObject;
  public GameObject spawnObject2;

  public GameObject target;
  public float interval;
  private float timeSince = 0f;

  private float secs = 0f;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    secs += Time.deltaTime;
    timeSince += Time.deltaTime;
    if (timeSince >= interval)
    {
      spawn();
      timeSince = 0f;
    }
  }
  private float getWeight()
  {
    if (secs < 5)
    {
      return 0.05f;
    }
    else if (secs < 10)
    {
      return 0.1f;
    }
    else if (secs < 15)
    {
      return 0.3f;
    }
    else if (secs < 20)
    {
      return 0.6f;
    }

    return 0.8f;
  }

  private void spawn()
  {
    float rand = Random.Range(0f, 1f);
    GameObject resolvedSpawn;
    float weight = getWeight();
    Debug.Log("weight: " + weight + " -- rand: " + rand);
    if (rand > Mathf.Min(weight, 1))
    {
      resolvedSpawn = spawnObject;
    }
    else
    {
      resolvedSpawn = spawnObject2;
    }
    GameObject tempGo = GameObject.Instantiate(resolvedSpawn, Vector3.zero, Quaternion.identity) as GameObject;
    Enemy enemyScript = tempGo.GetComponent<Enemy>();
    tempGo.transform.position = transform.position;
    enemyScript.target = target;
  }
}
