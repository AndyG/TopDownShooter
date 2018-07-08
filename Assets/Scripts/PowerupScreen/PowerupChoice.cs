using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Powerup Choice", menuName = "Powerup Choice")]
public class PowerupChoice : ScriptableObject
{
  [SerializeField]
  private string title;
  [SerializeField]
  private string description;
  [SerializeField]
  private Sprite artwork;
  [SerializeField]
  private bool flipHorizontal;
  [SerializeField]
  private bool flipVertical;

  public string GetTitle()
  {
    return title;
  }

  public string GetDescription()
  {
    return description;
  }

  public Sprite GetArtwork()
  {
    return artwork;
  }

  public bool GetFlipHorizontal()
  {
    return flipHorizontal;
  }

  public bool GetFlipVertical()
  {
    return flipVertical;
  }
}