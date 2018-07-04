using UnityEngine;

public interface IHittable
{
  void OnHit(GameObject other);
}