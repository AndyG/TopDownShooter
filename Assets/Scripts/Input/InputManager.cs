using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputManager
{
  bool isFirePressed();
  bool isDashPressed();

  bool isButton1Pressed();
  bool isPausePressed();

  float getMoveAxisHorizontal();
  float getMoveAxisVertical();
  float getAimAxisHorizontal();
  float getAimAxisVertical();
}
