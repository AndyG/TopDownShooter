using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUD : MonoBehaviour
{

  [SerializeField]
  private TextMeshProUGUI text;

  [SerializeField]
  private BasicPlayer player;

  void Awake()
  {
    text = GetComponentInChildren<TextMeshProUGUI>();
  }

  void OnEnable()
  {
    player.OnBombCountChangedEvent += setBombCount;
    setBombCount(player.getBombCount());
  }

  void OnDisable()
  {
    player.OnBombCountChangedEvent -= setBombCount;
  }

  private void setBombCount(int bombCount)
  {
    text.SetText(bombCount.ToString());
  }
}
