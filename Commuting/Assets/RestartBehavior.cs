using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartBehavior : MonoBehaviour {
  public float delay;

  void Start() {
    FindObjectOfType<TimingController>().paused = true;
  }

  void Update() {
    delay -= Time.deltaTime;
    if (delay < 0) {
      if (Input.GetButtonDown("Action")) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
    }
  }
}
