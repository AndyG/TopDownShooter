using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

  [Header("Wave Configuration")]
  [SerializeField]
  private float durationSecs;

  [Header("Spawners")]
  [SerializeField]
  private List<Spawner> spawners;

  private float timeRemainingSecs;
  private bool isEnded;

  // Use this for initialization
  void Start()
  {
    timeRemainingSecs = durationSecs;
  }

  // Update is called once per frame
  void Update()
  {
    if (!isEnded)
    {
      timeRemainingSecs -= Time.deltaTime;
      if (timeRemainingSecs <= 0)
      {
        EndWave();
      }
    }
  }

  private void EndWave()
  {
    isEnded = true;
    foreach (Spawner spawner in spawners)
    {
      spawner.OnWaveEnded();
    }
  }
}
