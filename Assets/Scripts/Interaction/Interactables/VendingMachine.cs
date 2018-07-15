using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IInteractable
{

  private PurchaseManager purchaseManager;

  [SerializeField]
  private GameObject explosionPrototype;

  // Use this for initialization
  void Start()
  {
    purchaseManager = GameObject.FindObjectOfType<PurchaseManager>();
  }

  public void OnInteract()
  {
    purchaseManager.BeginPurchase(this);
  }

  public void OnPurchaseEnded()
  {
    Explode();
  }

  private void Explode()
  {
    GameObject explosion = GameObject.Instantiate(explosionPrototype, this.transform.position, this.transform.rotation);
    GameObject.Destroy(transform.parent.gameObject);
  }
}
