using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Shootable, Bombable, Targeter
{

  [SerializeField]
  private GameObject explosion;

  [SerializeField]
  private GameObject target;

  [SerializeField]
  private float speed;

  [SerializeField]
  private GameObject powerupDrop;

  [SerializeField]
  [Range(0, 1.0f)]
  private float powerupChance = 0.1f;

  [SerializeField]
  private int hp = 5;

  private UnityEngine.Color baseColor;

  private SpriteRenderer spriteRenderer;

  [SerializeField]
  private bool zigZag;

  [SerializeField]
  private float zigZagTargetingTime = 1f;

  [SerializeField]
  private float zigZagTravelTime = 0.5f;

  private float zigZagCurrentTravelTime = 0f;

  [SerializeField]
  private Vector3 zigZagTargetPos;

  private bool zigZagTargeting = false;
  private bool hasBegunZigZagging = false;

  // Use this for initialization
  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    baseColor = spriteRenderer.material.GetColor("_Color");
    zigZagTargetPos = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    if (target != null)
    {
      if (!zigZag)
      {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
      }
      else
      {
        if (!hasBegunZigZagging)
        {
          zigZagTargetPos = computeZigZagTargetPos();
          zigZagTargeting = false;
          zigZagCurrentTravelTime = 0f;
          hasBegunZigZagging = true;
        }
        else if (!zigZagTargeting)
        {
          float step = speed * Time.deltaTime;
          transform.position = Vector3.MoveTowards(transform.position, zigZagTargetPos, step);

          float distanceFromTarget = Vector3.Distance(transform.position, zigZagTargetPos);
          zigZagCurrentTravelTime += Time.deltaTime;
          if (zigZagCurrentTravelTime >= zigZagTravelTime)
          {
            StartCoroutine(delayedComputeZigZagTargetPos());
          }
        }
      }
    }
  }

  public void onBombed()
  {
    die(true);
  }

  public void handleShot(Bullet bullet)
  {
    hp--;
    if (hp <= 0)
    {
      die(false);
    }
    else
    {
      spriteRenderer.material.SetColor("_Color", Color.blue);
      StartCoroutine(hitFlash());
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
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

  private void die(bool wasBomb)
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

    onDeath(wasBomb);

    Destroy(this.gameObject);
  }

  private void onDeath(bool wasBomb)
  {
    if (!wasBomb)
    {
      SplittingEnemy splittingEnemy = gameObject.GetComponent<SplittingEnemy>();
      if (splittingEnemy != null)
      {
        splittingEnemy.spawnObjects(target);
      }
    }
  }

  private IEnumerator hitFlash()
  {
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.material.SetColor("_Color", baseColor);
    yield return null;
  }

  private IEnumerator delayedComputeZigZagTargetPos()
  {
    zigZagTargeting = true;
    yield return new WaitForSeconds(zigZagTargetingTime);
    zigZagTargetPos = computeZigZagTargetPos();
    zigZagTargeting = false;
    zigZagCurrentTravelTime = 0f;
  }

  private Vector3 computeZigZagTargetPos()
  {
    Vector3 position = transform.position;
    Vector3 targetPosition = target.transform.position;

    Vector3 direction = (targetPosition - position).normalized;
    float angle = Random.Range(-45f, 45f);
    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
    Vector3 angledDirection = rotation * direction;

    return transform.position + (angledDirection * speed);
  }

  public void SetTarget(GameObject gameObject)
  {
    this.target = gameObject;
  }
}
