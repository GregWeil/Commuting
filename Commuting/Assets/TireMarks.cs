using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireMarks : MonoBehaviour {
  public float minDistance;

  private Vector3 lastPosition;
  private LineRenderer line;

  void Start() {
    line = GetComponent<LineRenderer>();
    lastPosition = transform.position;
    line.SetPositions(new[] { lastPosition });
  }

  void Update() {
    if (Vector3.Distance(lastPosition, transform.position) > minDistance) {
      lastPosition = transform.position;
      line.positionCount += 1;
      line.SetPosition(line.positionCount - 1, lastPosition);
    }
  }
}
