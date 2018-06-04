using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupNut : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D other)
  {
    PickupReceiver pickupReceiver = other.GetComponent<PickupReceiver>();
    if (pickupReceiver != null)
    {
      pickupReceiver.onPickup();
      Destroy(this.gameObject);
    }
  }
}