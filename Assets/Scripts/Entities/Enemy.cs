using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Shootable, Bombable
{

  [SerializeField]
  private GameObject explosion;
  public GameObject target;
  public float speed;

  [SerializeField]
  private GameObject powerupDrop;

  [SerializeField]
  [Range(0, 1.0f)]
  private float powerupChance = 0.1f;

  [SerializeField]
  private int hp = 5;

  private UnityEngine.Color baseColor;

  private SpriteRenderer spriteRenderer;

  // Use this for initialization
  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    baseColor = spriteRenderer.material.GetColor("_Color");
  }

  // Update is called once per frame
  void Update()
  {
    if (target != null)
    {
      float step = speed * Time.deltaTime;
      transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }
  }

  public void onBombed()
  {
    die();
  }

  public void handleShot(Bullet bullet)
  {
    hp--;
    if (hp <= 0)
    {
      die();
    }
    else
    {
      spriteRenderer.material.SetColor("_Color", Color.blue);
      StartCoroutine(hitFlash());
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("on trigger enter");

    Bullet bullet = other.GetComponent<Bullet>();
    if (bullet != null)
    {
      handleShot(bullet);
      Destroy(bullet.gameObject);
      return;
    }

    BasicPlayer player = other.GetComponent<BasicPlayer>();
    if (player != null)
    {
      player.onHit();
    }

    Bomb bomb = other.GetComponent<Bomb>();
    if (bomb != null)
    {
      onBombed();
    }
  }

  private void die()
  {
    if (explosion != null)
    {
      GameObject tempGo = GameObject.Instantiate(explosion, Vector3.zero, Quaternion.identity) as GameObject;
      tempGo.transform.position = transform.position;
    }

    float rand = Random.Range(0, 1.0f);
    if (rand < powerupChance)
    {
      GameObject tempGo = GameObject.Instantiate(powerupDrop, Vector3.zero, Quaternion.identity) as GameObject;
      tempGo.transform.position = transform.position;
    }
    Destroy(this.gameObject);
  }

  private IEnumerator hitFlash()
  {
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.material.SetColor("_Color", baseColor);
    yield return null;
  }
}
