using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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

  [SerializeField]
  private Text killCountText;

  void Awake()
  {
    text = GetComponentInChildren<TextMeshProUGUI>();
  }

  void OnEnable()
  {
    player.OnBombCountChangedEvent += setBombCount;
    player.OnHitPointsChangedEvent += setHitPointCount;
    setBombCount(player.getBombCount());
    player.OnKillCountChangedEvent += OnEnemyKilled;
  }

  void OnDisable()
  {
    player.OnBombCountChangedEvent -= setBombCount;
    player.OnHitPointsChangedEvent -= setHitPointCount;
    player.OnKillCountChangedEvent -= OnEnemyKilled;
  }

  private void setBombCount(int bombCount)
  {
    text.SetText(bombCount.ToString());
  }

  private void setHitPointCount(int hitPoints)
  {
    if (hitPoints == 3)
    {
      hitPoint3.Recover();
      hitPoint2.Recover();
      hitPoint1.Recover();
    }
    if (hitPoints <= 2)
    {
      hitPoint3.Destroy();
      hitPoint2.Recover();
      hitPoint1.Recover();
    }

    if (hitPoints <= 1)
    {
      hitPoint2.Destroy();
      hitPoint1.Recover();
    }

    if (hitPoints <= 0)
    {
      hitPoint1.Destroy();
    }
  }

  public void OnEnemyKilled(int killCount)
  {
    killCountText.text= "Kills: " + killCount;
  }
}
