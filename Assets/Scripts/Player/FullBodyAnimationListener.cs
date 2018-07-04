using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBodyAnimationListener : MonoBehaviour
{

  public delegate void OnDashEndedDelegate();
  public event OnDashEndedDelegate OnDashEndedEvent;

  public void OnDashEnded()
  {
    if (OnDashEndedEvent != null)
    {
      OnDashEndedEvent();
    }
  }
}
