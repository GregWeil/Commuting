using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneTile : MonoBehaviour {
  public float delay;

  private TimingController timing;
  private bool active;

  void Start() {
    timing = FindObjectOfType<TimingController>();
  }

  void Update() {
    if (active) {
      delay -= timing.GetTurnDt();
      if (delay < 0) {
        Utilities.RecordScore(timing.GetTurn());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }
    }
  }

  void OnTileEnter() {
    foreach (PlayerCar player in FindObjectsOfType<PlayerCar>()) {
      player.LockInput();
    }
    active = true;
  }
}
