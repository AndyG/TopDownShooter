using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointManager : MonoBehaviour
{

  public delegate void OnHitPointsChanged(int hitPoints);
  public event OnHitPointsChanged OnHitPointsChangedEvent;

  [SerializeField]
  private int hitPoints;

  [SerializeField]
  private int maxHitPoints;

  public void decrement()
  {
    hitPoints--;
    Clamp();
    Notify();
  }

  public void subtract(int numHitPoints)
  {
    hitPoints -= numHitPoints;
    Clamp();
    Notify();
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
