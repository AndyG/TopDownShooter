using UnityEngine;
public class InputManagerPS4 : MonoBehaviour, InputManager
{
  public bool isFirePressed()
  {
    return Input.GetButtonDown("Fire1");
  }
  public bool isDashPressed()
  {
    return false;
  }

  public bool isPausePressed()
  {
    return Input.GetKeyDown("joystick button 9");
  }

  public float getMoveAxisHorizontal()
  {
    return Input.GetAxis("Horizontal"); ;
  }
  public float getMoveAxisVertical()
  {
    return Input.GetAxis("Vertical");
  }
  public float getAimAxisHorizontal()
  {
    return Input.GetAxis("RightHorizontal");
  }
  public float getAimAxisVertical()
  {
    return Input.GetAxis("RightVertical");
  }
}