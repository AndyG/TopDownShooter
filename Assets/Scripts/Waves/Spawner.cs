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

  private HashSet<Enemy> enemies;

  [SerializeField]
  private bool isEnemies;

  // Use this for initialization
  void Start()
  {
    enemies = new HashSet<Enemy>();
    spawnerConfig = GetComponent<SpawnerConfig>();
    target = GameObject.FindGameObjectWithTag("Player");
  }

  // Update is called once per frame
  void Update()
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

  public void OnWaveEnded()
  {
    foreach (Enemy enemy in enemies)
    {
      enemy.SetReportDeath(false);
      enemy.onBombed();
    }
    this.enabled = false;
  }

  public void OnEnemyDeath(Enemy enemy)
  {
    enemies.Remove(enemy);
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
      Enemy enemy = tempGo.GetComponent<Enemy>();
      enemy.AttachSpawner(this);
      enemies.Add(enemy);
    }
  }
}
