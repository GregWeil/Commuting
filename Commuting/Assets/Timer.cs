using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
  public float startHours;
  public float secondsPerTurn;
  public Light sun;
  public float sunAngle;
  public AnimationCurve sunCurve;
  public float minHours;
  public float maxHours;
  public GameObject current;
  public GameObject best;

  private float initialTurn;
  private TimingController timing;
  private Text text;

  void Start() {
    initialTurn = Utilities.GetCurrentScore();
    timing = FindObjectOfType<TimingController>();
    text = GetComponent<Text>();
    if (Utilities.HasBestScore()) {
      text.text = "Best: " + Utilities.FormatScoreAsTime(startHours, secondsPerTurn, Utilities.GetBestScore());
    }
  }

  // Update is called once per frame
  void Update() {
    if (timing.paused) return;
    float totalTurns = timing.GetTurn() + initialTurn;
    text.text = Utilities.FormatScoreAsTime(startHours, secondsPerTurn, totalTurns);

    float hour = startHours + totalTurns * this.secondsPerTurn / 60f / 60f;
    float prop = sunCurve.Evaluate((hour - minHours) / (maxHours - minHours));
    sun.transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(-sunAngle, sunAngle, prop), 0);
  }

  public void SwitchToTally() {
    text.enabled = false;
    Text currentText = current.GetComponent<Text>();
    currentText.text = currentText.text + "\n" + string.Join("\n", Utilities.GetCurrentScores().Select(score => Utilities.FormatScoreAsTime(startHours, secondsPerTurn, score)));
    currentText.enabled = true;
    Text bestText = best.GetComponent<Text>();
    bestText.text = bestText.text + "\n" + string.Join("\n", Utilities.GetBestScores().Select(score => Utilities.FormatScoreAsTime(startHours, secondsPerTurn, score)));
    bestText.enabled = true;
  }
}
