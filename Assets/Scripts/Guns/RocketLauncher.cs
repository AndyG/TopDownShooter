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
  private Rocket projectile;

  [SerializeField]
  private float speed;

  [SerializeField]
  private GameObject muzzleFlashPrototype;

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
    Rocket rocket = GameObject.Instantiate(projectile, Vector3.zero, Quaternion.identity) as Rocket;
    rocket.transform.position = bulletOrigin.position;
    rocket.SetDirection(direction);

    GameObject muzzleFlash = GameObject.Instantiate(muzzleFlashPrototype, Vector3.zero, Quaternion.identity) as GameObject;
    muzzleFlash.transform.position = bulletOrigin.position;
    float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;
    muzzleFlash.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

    timeSinceLastShot = 0;
  }
}
