using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireMarks : MonoBehaviour {
  public float minDistance;
  public float startDistance;
  public float delay;

  private int turn;
  private LineRenderer line;
  private TimingController timing;

  void Start() {
    line = GetComponent<LineRenderer>();
    timing = FindObjectOfType<TimingController>();
    line.SetPositions(new[] { transform.TransformPoint(Vector3.up * startDistance), transform.position });
  }

  void Update() {
    delay -= timing.GetTurnDt();
    Vector3 lastPosition = line.GetPosition(line.positionCount - 2);
    if (Vector3.Distance(lastPosition, transform.position) > minDistance) {
      if (delay < 0f) line.positionCount += 1;
    }
    line.SetPosition(line.positionCount - 1, transform.position);
  }
}
