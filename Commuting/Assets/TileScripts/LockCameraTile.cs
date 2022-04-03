using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCameraTile : MonoBehaviour {
  public Vector3 positionOffset;
  public Vector3 lookOffset;
  public float moveSpeed;
  public float lookSpeed;
  
  void OnTileEnter() {
    CameraFollow follow = FindObjectOfType<CameraFollow>();
    if (follow != null) {
      follow.target = transform;
      follow.positionOffset = positionOffset;
      follow.lookOffset = lookOffset;
      follow.moveSpeed = moveSpeed;
      follow.turnSpeed = lookSpeed;
    }
  }
}
