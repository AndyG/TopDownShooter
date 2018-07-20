using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

  public delegate void OnWaveEnded();
  public event OnWaveEnded OnWaveEndedEvent;

  [Header("Wave Configuration")]
  [SerializeField]
  private float durationSecs;

  [Header("Spawners")]
  [SerializeField]
  private List<Spawner> spawners;

  private float timeRemainingSecs;
  private bool isStarted;
  private bool isEnded;

  // Use this for initialization
  void Start()
  {
    timeRemainingSecs = durationSecs;
  }

  // Update is called once per frame
  void Update()
  {
    if (isStarted && !isEnded)
    {
      timeRemainingSecs -= Time.deltaTime;
      if (timeRemainingSecs <= 0)
      {
        EndWave();
      }
    }
  }

  public void BeginWave()
  {
    isStarted = true;
    foreach (Spawner spawner in spawners)
    {
      spawner.StartSpawning();
    }
  }

  private void EndWave()
  {
    isEnded = true;
    foreach (Spawner spawner in spawners)
    {
      spawner.OnWaveEnded();
    }

    if (OnWaveEndedEvent != null)
    {
      OnWaveEndedEvent();
    }
  }
}
