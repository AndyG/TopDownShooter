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

  public bool isGrowPressed()
  {
    return Input.GetKeyDown(KeyCode.A);
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