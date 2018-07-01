using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MainMenuInput : MonoBehaviour
{

  [SerializeField]
  private int playerId;

  private Player player;

  private bool didPressConfirm;
  private int navDirection;

  void Awake()
  {
    player = ReInput.players.GetPlayer(playerId);
  }

  void LateUpdate()
  {
    didPressConfirm = false;
    navDirection = 0;
  }

  public void GatherInput()
  {
    didPressConfirm = _GetPressedConfirm();
    navDirection = _GetNavDirection();
  }

  #region Getters
  public bool DidPressConfirm()
  {
    return didPressConfirm;
  }

  public int GetNavDirection()
  {
    return navDirection;
  }
  #endregion

  #region Get raw input
  private bool _GetPressedConfirm()
  {
    return player.GetButtonDown("Confirm");
  }

  private int _GetNavDirection()
  {
    return Math.Sign(player.GetAxis("Move Vertical"));
  }
  #endregion
}
