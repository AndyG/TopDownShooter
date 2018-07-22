using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

  private SpawnerConfig spawnerConfig;

  private GameObject target;
  private float timeSince = 0f;

  private float secs = 0f;

  [SerializeField]
  private float distanceThreshold = 16;

  private HashSet<Spawnable> spawnables;

  [SerializeField]
  private bool isEnemies;

  [SerializeField]
  private bool startActive = true;

  private bool isSpawningObjects;

  // Use this for initialization
  void Start()
  {
    spawnables = new HashSet<Spawnable>();
    spawnerConfig = GetComponent<SpawnerConfig>();
    target = GameObject.FindGameObjectWithTag("Player");
    isSpawningObjects = startActive;
  }

  // Update is called once per frame
  void Update()
  {
    if (isSpawningObjects)
    {
      secs += Time.deltaTime;
      timeSince += Time.deltaTime;
      if (timeSince >= spawnerConfig.getIntervalSeconds())
      {
        float distanceFromTarget = Vector3.Distance(target.transform.position, this.transform.position);
        if (distanceFromTarget > distanceThreshold)
        {
          spawn();
          timeSince = 0f;
        }
      }
    }
  }

  public void OnWaveEnded()
  {
    foreach (Spawnable spawnable in spawnables)
    {
      spawnable.OnSpawnerDeactivated();
    }
    StopSpawning();
  }

  public void OnEnemyDeath(Spawnable spawnable)
  {
    spawnables.Remove(spawnable);
  }

  public void StartSpawning()
  {
    this.isSpawningObjects = true;
  }

  public void StopSpawning()
  {
    this.isSpawningObjects = false;
    timeSince = 0f;
  }

  private void spawn()
  {
    GameObject spawnObject = spawnerConfig.GetRandomSpawnObject();
    GameObject tempGo = GameObject.Instantiate(spawnObject, Vector3.zero, Quaternion.identity) as GameObject;
    tempGo.transform.position = transform.position;

    Targeter targeter = tempGo.GetComponent<Targeter>();
    if (targeter != null)
    {
      targeter.SetTarget(target);
    }

    if (isEnemies)
    {
      Spawnable spawnable = tempGo.GetComponent<Spawnable>();
      spawnable.AttachSpawner(this);
      spawnables.Add(spawnable);
    }
  }
}
