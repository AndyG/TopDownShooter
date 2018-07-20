using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInManager : MonoBehaviour
{

  public delegate void OnLockInCompleted();
  public event OnLockInCompleted OnLockInCompletedEvent;

  private Camera camera;
  private Track cameraTrack;

  private BarricadeGroup[] barricadeGroups;
  private int nextIndex = -1;
  private int onDeckIndex = 0;

  [SerializeField]
  private float distanceThreshold;
  [SerializeField]
  private float lingerSecs = 0.5f;

  [Header("Tracking Details")]
  [SerializeField]
  private float distance = 15;
  [SerializeField]
  public float lerpFactor = 0.1f;

  [Header("Debug")]
  [SerializeField]
  private List<BarricadeGroup> testBarricadeGroups;

  private LockInState lockInState = LockInState.NONE;

  private enum LockInState
  {
    LOCKING,
    RELEASING,
    NONE
  }

  void Start()
  {
    camera = GameObject.FindObjectOfType<Camera>();
    cameraTrack = camera.GetComponent<Track>();
  }

  void Update()
  {
    if (lockInState == LockInState.LOCKING && nextIndex >= 0)
    {
      ContinueLockIn();
    }
    else if (lockInState == LockInState.RELEASING && nextIndex >= 0)
    {
      ContinueRelease();
    }
  }

  public void LockIn()
  {
    LockIn(testBarricadeGroups.ToArray());
  }

  public void LockIn(BarricadeGroup barricadeGroup)
  {
    BarricadeGroup[] barricadeGroups = new BarricadeGroup[] { barricadeGroup };
    LockIn(barricadeGroups);
  }

  public void LockIn(BarricadeGroup[] barricadeGroups)
  {
    Time.timeScale = 0f;
    this.barricadeGroups = barricadeGroups;
    cameraTrack.DisableTracking();
    nextIndex = 0;
    onDeckIndex = 0;
    lockInState = LockInState.LOCKING;
  }

  public void Release()
  {
    Release(testBarricadeGroups.ToArray());
  }

  public void Release(BarricadeGroup barricadeGroup)
  {
    BarricadeGroup[] barricadeGroups = new BarricadeGroup[] { barricadeGroup };
    Release(barricadeGroups);
  }

  public void Release(BarricadeGroup[] barricadeGroups)
  {
    Time.timeScale = 0f;
    this.barricadeGroups = barricadeGroups;
    cameraTrack.DisableTracking();
    nextIndex = 0;
    onDeckIndex = 0;
    lockInState = LockInState.RELEASING;
  }

  private void ContinueLockIn()
  {
    Vector3 targetPosition = computeTargetPosition();
    camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, lerpFactor);
    float distance = Vector3.Distance(camera.transform.position, targetPosition);
    if (distance < distanceThreshold)
    {
      barricadeGroups[nextIndex].Appear();
      bool hasMore = nextIndex < barricadeGroups.Length - 1;
      if (hasMore)
      {
        StartCoroutine(TrackNext(nextIndex + 1));
      }
      else
      {
        StartCoroutine(EndLockIn());
      }
    }
  }

  private void ContinueRelease()
  {
    Vector3 targetPosition = computeTargetPosition();
    camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, lerpFactor);
    float distance = Vector3.Distance(camera.transform.position, targetPosition);
    if (distance < distanceThreshold)
    {
      barricadeGroups[nextIndex].Disappear();
      bool hasMore = nextIndex < barricadeGroups.Length - 1;
      if (hasMore)
      {
        StartCoroutine(TrackNext(nextIndex + 1));
      }
      else
      {
        StartCoroutine(EndLockInRelease());
      }
    }
  }

  private IEnumerator EndLockIn()
  {
    yield return new WaitForSecondsRealtime(lingerSecs);
    cameraTrack.EnableTracking();
    barricadeGroups = null;
    lockInState = LockInState.NONE;
    nextIndex = -1;
    onDeckIndex = -1;
    Time.timeScale = 1f;
    if (OnLockInCompletedEvent != null)
    {
      OnLockInCompletedEvent();
    }
  }

  private IEnumerator EndLockInRelease()
  {
    yield return new WaitForSecondsRealtime(lingerSecs);
    cameraTrack.EnableTracking();
    barricadeGroups = null;
    lockInState = LockInState.NONE;
    nextIndex = -1;
    onDeckIndex = -1;
    Time.timeScale = 1f;
  }

  private Vector3 computeTargetPosition()
  {
    Transform target = barricadeGroups[nextIndex].transform;
    return target.transform.position + (Vector3.back * distance);
  }

  private IEnumerator TrackNext(int next)
  {
    onDeckIndex = next;
    // Set this to linger.
    nextIndex = -1;
    yield return new WaitForSecondsRealtime(lingerSecs);
    nextIndex = onDeckIndex;
  }
}
