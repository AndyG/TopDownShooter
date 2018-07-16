using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeGroup : MonoBehaviour
{
  private Barricade[] barricades;

  // Use this for initialization
  void Start()
  {
    barricades = GetComponentsInChildren<Barricade>();
    // StartCoroutine(TestLoop());
  }

  public void Appear()
  {
    foreach (Barricade barricade in barricades)
    {
      barricade.Appear();
    }
  }

  public void Disappear()
  {
    foreach (Barricade barricade in barricades)
    {
      barricade.Disappear();
    }
  }

  private IEnumerator TestLoop()
  {
    while (true)
    {
      yield return new WaitForSeconds(2);
      Appear();
      yield return new WaitForSeconds(2);
      Disappear();
    }
  }
}
