using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
  public Transform target;
  public Vector3 positionOffset;
  public Vector3 lookOffset;
  public float moveSpeed;
  public float turnSpeed;

  private TimingController timing;

  void Start() {
    timing = GameObject.FindObjectOfType<TimingController>();
    transform.position = target.TransformPoint(lookOffset) + positionOffset;
  }

  void Update() {
    float dt = timing.GetTurnDt();
    transform.position = Vector3.Lerp(transform.position,
        target.TransformPoint(lookOffset) + positionOffset,
        Utilities.GetLerpProp(moveSpeed, dt, 60f));
    transform.rotation = Quaternion.Lerp(transform.rotation,
        Quaternion.LookRotation(Vector3.up * lookOffset.y - positionOffset),
        Utilities.GetLerpProp(turnSpeed, dt, 60f));
  }
}
