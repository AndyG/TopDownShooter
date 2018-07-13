using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
  [SerializeField]
  private bool hasGoodRockets;

  [SerializeField]
  private bool hasInvincibleRoll;

  private static bool created = false;

  void Awake()
  {
    if (!created)
    {
      DontDestroyOnLoad(this.gameObject);
      created = true;
    }
  }

  public bool getHasGoodRockets()
  {
    return hasGoodRockets;
  }

  public void SetHasGoodRockets()
  {
    hasGoodRockets = true;
  }

  public bool getHasInvincibleRoll()
  {
    return hasInvincibleRoll;
  }

  public void SetHasInvincibleRoll()
  {
    hasInvincibleRoll = true;
  }
}
