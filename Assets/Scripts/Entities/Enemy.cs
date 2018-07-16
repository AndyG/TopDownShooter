using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Shootable, Bombable, Targeter, IHittable
{

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

  [SerializeField]
  private Material hitFlashMaterial;

  private Material baseMaterial;

  private SpriteRenderer spriteRenderer;

  private Explosion explosion;

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

  private Spawner spawner;

  public delegate void OnDeath(Enemy enemy);
  public event OnDeath OnDeathEvent;
  private bool reportDeath = true;

  // Use this for initialization
  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    baseMaterial = spriteRenderer.material;
    zigZagTargetPos = transform.position;
    explosion = GetComponent<Explosion>();
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

  public void SetReportDeath(bool reportDeath)
  {
    this.reportDeath = reportDeath;
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
      spriteRenderer.material = hitFlashMaterial;
      StartCoroutine(hitFlash());
    }
  }

  void OnTriggerStay2D(Collider2D other)
  {
    HandleCollision(other);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    HandleCollision(other);
  }

  public void AttachSpawner(Spawner spawner)
  {
    this.spawner = spawner;
  }

  private void HandleCollision(Collider2D other)
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
      player.onHit(gameObject);
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
      explosion.Explode();
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
    if (reportDeath && spawner != null)
    {
      spawner.OnEnemyDeath(this);
    }

    if (!wasBomb)
    {
      SplittingEnemy splittingEnemy = gameObject.GetComponent<SplittingEnemy>();
      if (splittingEnemy != null)
      {
        splittingEnemy.spawnObjects(target, spawner);
      }
    }
  }

  private IEnumerator hitFlash()
  {
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.material = baseMaterial;
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

  public void OnHit(GameObject other)
  {
    Rocket rocket = other.GetComponent<Rocket>();
    if (rocket != null)
    {
      die(true);
    }
  }
}
