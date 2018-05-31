using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayer : MonoBehaviour
{

  [SerializeField]
  float topSpeed = 15;

  Rigidbody2D rb;

  Animator animator;

  // Use this for initialization
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    float horiz = Input.GetAxis("Horizontal");
    float vert = Input.GetAxis("Vertical");

    updateAnimator(horiz, vert);

    Vector2 velocity = new Vector2(horiz, vert);
    rb.AddForce(velocity, ForceMode2D.Impulse);
    rb.velocity = Vector2.ClampMagnitude(rb.velocity, topSpeed);
  }

  void updateAnimator(float horiz, float vert)
  {
    animator.SetFloat("HorizInput", horiz);
  }
}
