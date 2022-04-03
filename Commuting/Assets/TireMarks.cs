using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireMarks : MonoBehaviour {
  public float minDistance;

  private int turn;
  private LineRenderer line;
  private TimingController timing;

  void Start() {
    line = GetComponent<LineRenderer>();
    timing = FindObjectOfType<TimingController>();
    line.SetPositions(new[] { transform.position, transform.position });
  }

  void Update() {
    Vector3 lastPosition = line.GetPosition(line.positionCount - 2);
    if (Vector3.Distance(lastPosition, transform.position) > minDistance) {
      line.positionCount += 1;
    }
    line.SetPosition(line.positionCount - 1, transform.position);
  }
}
