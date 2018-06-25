using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUD : MonoBehaviour
{

  [SerializeField]
  private TextMeshProUGUI text;

  [SerializeField]
  private BasicPlayer player;

  [SerializeField]
  private HitPoint hitPoint1;

  [SerializeField]
  private HitPoint hitPoint2;

  [SerializeField]
  private HitPoint hitPoint3;

  void Awake()
  {
    text = GetComponentInChildren<TextMeshProUGUI>();
  }

  void OnEnable()
  {
    player.OnBombCountChangedEvent += setBombCount;
    player.OnHitPointsChangedEvent += setHitPointCount;
    setBombCount(player.getBombCount());
  }

  void OnDisable()
  {
    player.OnBombCountChangedEvent -= setBombCount;
    player.OnHitPointsChangedEvent -= setHitPointCount;
  }

  private void setBombCount(int bombCount)
  {
    text.SetText(bombCount.ToString());
  }

  private void setHitPointCount(int hitPoints)
  {
    if (hitPoints <= 2)
    {
      hitPoint3.Destroy();
    }

    if (hitPoints <= 1)
    {
      hitPoint2.Destroy();
    }

    if (hitPoints <= 0)
    {
      hitPoint1.Destroy();
    }
  }
}
