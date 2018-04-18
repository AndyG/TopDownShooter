using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputManager
{
  bool isFirePressed();
  bool isDashPressed();

  float getMoveAxisHorizontal();
  float getMoveAxisVertical();
  float getAimAxisHorizontal();
  float getAimAxisVertical();
}
