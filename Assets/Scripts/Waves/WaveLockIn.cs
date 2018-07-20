using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLockIn : MonoBehaviour
{

  [SerializeField]
  private LockInManager lockInManager;

  [SerializeField]
  private WaveManager waveManager;

  [SerializeField]
  private float releaseDelaySecs = 1;

  // Use this for initialization
  void Start()
  {
    lockInManager.OnLockInCompletedEvent += this.OnLockInCompleted;
    waveManager.OnWaveEndedEvent += this.OnWaveEnded;
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnLockInCompleted()
  {
    waveManager.BeginWave();
  }

  private void OnWaveEnded()
  {
    StartCoroutine(ReleaseLockInDelayed(releaseDelaySecs));
  }

  private IEnumerator ReleaseLockInDelayed(float delaySecs)
  {
    yield return new WaitForSeconds(delaySecs);
    lockInManager.Release();
  }
}
