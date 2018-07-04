using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
  private static float MAX_RATE_OF_FIRE = 20;
  private static float POWER_UP_DURATION_SECONDS = 5;

  [SerializeField]
  private Transform bulletOrigin;

  [SerializeField]
  public Rocket projectile;

  [SerializeField]
  public float speed;

  [SerializeField]
  [Range(1, 30)]
  private float baseRateOfFire = 5;

  [SerializeField]
  private float rateOfFire = 1;

  [SerializeField]
  private AudioClip audioClip;

  private AudioSource audioSource;

  private float timeSinceLastShot;

  // Use this for initialization
  void Start()
  {
    rateOfFire = baseRateOfFire;
    timeSinceLastShot = rateOfFire;

    audioSource = gameObject.GetComponent<AudioSource>();
  }

  void Update()
  {
    timeSinceLastShot += Time.deltaTime;
  }

  public bool Use(Vector2 direction)
  {
    if (timeSinceLastShot < (1 / rateOfFire))
    {
      return false;
    }

    spawnBullet(direction);
    return true;
  }

  private void spawnBullet(Vector3 direction)
  {
    Rocket tempGo = GameObject.Instantiate(projectile, Vector3.zero, Quaternion.identity) as Rocket;
    Rigidbody2D rigidBody = tempGo.GetComponent<Rigidbody2D>();
    tempGo.transform.position = bulletOrigin.position;

    tempGo.SetDirection(direction);

    timeSinceLastShot = 0;
  }
}
