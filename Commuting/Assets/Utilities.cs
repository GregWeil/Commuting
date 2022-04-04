using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utilities {
  public static float GetLerpProp(float prop, float dt, float reference) {
    return 1f - Mathf.Pow(1f - prop, dt * reference);
  }

  public static string FormatScoreAsTime(float startHours, float secondsPerScore, float score) {
    return System.DateTime.Today
      .AddHours(startHours)
      .AddSeconds(secondsPerScore * score)
      .ToShortTimeString();
  }

  public static void RecordScore(float score) {
    string sceneName = SceneManager.GetActiveScene().path;
    if (PlayerPrefs.GetFloat(sceneName + "-best", 0f) < score) {
      PlayerPrefs.SetFloat(sceneName + "-best", score);
    }
    PlayerPrefs.SetFloat(sceneName + "-current", score);
  }

  public static bool HasBestScore() {
    int sceneCount = SceneManager.sceneCountInBuildSettings;
    for (int i = 0; i < sceneCount; ++i) {
      string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
      if (!PlayerPrefs.HasKey(sceneName + "-best")) return false;
    }
    return true;
  }

  public static IEnumerable<float> GetBestScores() {
    float score = 0f;
    int sceneCount = SceneManager.sceneCountInBuildSettings;
    for (int i = 0; i < sceneCount; ++i) {
      string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
      score += PlayerPrefs.GetFloat(sceneName + "-best", 0f);
      yield return score;
    }
  }

  public static float GetBestScore() {
    return GetBestScores().Last();
  }

  public static IEnumerable<float> GetCurrentScores() {
    float score = 0f;
    int sceneCount = SceneManager.sceneCountInBuildSettings;
    for (int i = 0; i < sceneCount; ++i) {
      string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
      score += PlayerPrefs.GetFloat(sceneName + "-current", 0f);
      yield return score;
    }
  }

  public static float GetCurrentScore() {
    return GetCurrentScores().Last();
  }

  public static void ClearCurrentScore() {
    int sceneCount = SceneManager.sceneCountInBuildSettings;
    for (int i = 0; i < sceneCount; ++i) {
      string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
      PlayerPrefs.DeleteKey(sceneName + "-current");
    }
  }
}
