using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInTrigger : MonoBehaviour
{
  bool activated = false;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (activated)
    {
      return;
    }

    Debug.Log("Trigger Entered");
    if (other.gameObject.tag == "Player")
    {
      transform.parent.GetComponent<LockInManager>().LockIn();
      GameObject.Destroy(this.gameObject);
      activated = true;
    }
  }
}
