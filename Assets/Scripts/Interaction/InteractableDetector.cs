using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{

  [SerializeField]
  private float distance = 1f;

  // Layer 10 (Interactables)
  int layerMask = 1 << 10;

  public GameObject Detect(Vector2 direction)
  {
    RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, distance, layerMask);
    if (hit)
    {
      IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
      if (interactable != null)
      {
        return hit.collider.gameObject;
      }
    }
    return null;
  }
}
