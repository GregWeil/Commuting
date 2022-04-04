using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBehavior : MonoBehaviour {
  public Vector3 moveSpeed;

  private bool moving;

  void Awake() {
    Utilities.ClearCurrentScore();
  }

  void Start() {
    TimingController timing = FindObjectOfType<TimingController>();
    timing.paused = true;
  }

  void Update() {
    if (!this.moving) {
      if (Input.GetButtonDown("Action")) {
        this.moving = true;
        TimingController timing = FindObjectOfType<TimingController>();
        timing.paused = false;
      }
    } else {
      this.transform.position += this.moveSpeed * Time.deltaTime;
    }
  }
}
