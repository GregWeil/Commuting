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
    if (this.turnDuration > 0) {
      this.turn += Time.deltaTime / this.turnDuration;
    }
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

  public float GetTurnDt() {
    if (this.turnDuration <= 0) return 0;
    return Time.deltaTime / this.turnDuration;
  }
}
