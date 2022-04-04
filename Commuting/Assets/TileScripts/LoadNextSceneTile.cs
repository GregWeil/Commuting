using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneTile : MonoBehaviour {
  void OnTileEnter() {
    FindObjectOfType<LevelCompleteBehavior>().LevelCompleted();
  }
}
