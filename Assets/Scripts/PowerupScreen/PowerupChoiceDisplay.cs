using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupChoiceDisplay : MonoBehaviour
{

  [Header("Powerup Choice")]
  [SerializeField]
  private PowerupChoice powerupChoice;

  [Header("UI Fields")]
  [SerializeField]
  private TextMeshProUGUI titleText;
  [SerializeField]
  private TextMeshProUGUI descriptionText;
  [SerializeField]
  private Image image;

  private Animator animator;

  // Use this for initialization
  void Start()
  {
    animator = GetComponent<Animator>();
    titleText.SetText(powerupChoice.GetTitle());
    descriptionText.SetText(powerupChoice.GetDescription());
    image.sprite = powerupChoice.GetArtwork();
    float xScale = powerupChoice.GetFlipHorizontal() ? -1 : 1;
    float yScale = powerupChoice.GetFlipVertical() ? -1 : 1;

    image.transform.localScale = new Vector3(xScale * image.transform.localScale.x, yScale * image.transform.localScale.y, image.transform.localScale.z);
  }

  public void SetFocused(bool isFocused)
  {
    Debug.Log("setting focused: " + isFocused + " -- " + name);
    animator.SetBool("IsFocused", isFocused);
  }

  public PowerupChoice GetPowerupChoice()
  {
    return powerupChoice;
  }
}
