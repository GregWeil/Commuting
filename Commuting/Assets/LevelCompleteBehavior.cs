using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteBehavior : MonoBehaviour {
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
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings) {
          SceneManager.LoadScene(nextIndex);
        } else {
          timing.paused = true;
          Timer timer = FindObjectOfType<Timer>();
          if (timer != null) timer.SwitchToTally();
        }
        active = false;
      }
    }
  }

  public void LevelCompleted() {
    foreach (PlayerCar player in FindObjectsOfType<PlayerCar>()) {
      player.LockInput();
    }
    active = true;
  }
}
