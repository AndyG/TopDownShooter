using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DeathScreen : MonoBehaviour
{

  [SerializeField]
  private TextMeshProUGUI text;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void setDead(float timeAlive)
  {
    text.SetText("nothing personnel kid... survived for " + timeAlive);
  }
}
