using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBodyAnimationListener : MonoBehaviour
{

  public delegate void OnDashEndedDelegate();
  public event OnDashEndedDelegate OnDashEndedEvent;

  public delegate void OnFinishedWarpingIn();
  public event OnFinishedWarpingIn OnFinishedWarpingInEvent;

  public void OnDashEnded()
  {
    if (OnDashEndedEvent != null)
    {
      OnDashEndedEvent();
    }
  }

  public void OnWarpingInEnded()
  {
    if (OnFinishedWarpingInEvent != null)
    {
      OnFinishedWarpingInEvent();
    }
  }
}
