using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointManager : MonoBehaviour
{

  public delegate void OnHitPointsChanged(int hitPoints);
  public event OnHitPointsChanged OnHitPointsChangedEvent;

  [SerializeField]
  private float regenTimeSecs = -1;

  [SerializeField]
  private int hitPoints;

  [SerializeField]
  private int maxHitPoints;

  private IEnumerator regenCoroutine;

  public void increment()
  {
    hitPoints++;

    if (hitPoints >= maxHitPoints)
    {
      StopRegen();
    }

    Clamp();
    Notify();
  }

  public void decrement()
  {
    hitPoints--;
    StartRegen();
    Clamp();
    Notify();
  }

  public void subtract(int numHitPoints)
  {
    hitPoints -= numHitPoints;
    StartRegen();
    Clamp();
    Notify();
  }

  private void StartRegen()
  {
    if (regenCoroutine == null)
    {
      regenCoroutine = RegenCoroutine();
      StartCoroutine(regenCoroutine);
    }
  }

  private void StopRegen()
  {
    if (regenCoroutine != null)
    {
      StopCoroutine(regenCoroutine);
      regenCoroutine = null;
    }
  }

  private IEnumerator RegenCoroutine()
  {
    while (hitPoints < maxHitPoints)
    {
      yield return new WaitForSeconds(regenTimeSecs);
      increment();
    }

    regenCoroutine = null;
  }

  private void Clamp()
  {
    hitPoints = Mathf.Clamp(hitPoints, 0, maxHitPoints);
  }

  private void Notify()
  {
    if (OnHitPointsChangedEvent != null)
    {
      OnHitPointsChangedEvent(hitPoints);
    }
  }
}
