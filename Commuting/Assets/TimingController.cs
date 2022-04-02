using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingController : MonoBehaviour {
  public float turnDuration;
  private float turn;

  void Start() {
    this.turn = 0;
  }

  void Update() {
    this.turn += Time.deltaTime / this.turnDuration;
  }

  public float GetTurn() {
    return this.turn;
  }

  public int GetTurnIndex() {
    return Mathf.RoundToInt(this.turn - 0.5f);
  }

  public float GetTurnFrac() {
    return this.GetTurn() - this.GetTurnIndex();
  }
}
