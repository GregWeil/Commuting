using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {
  public float startHours;
  public float secondsPerTurn;
  public Light sun;
  public float sunAngle;
  public AnimationCurve sunCurve;
  public float minHours;
  public float maxHours;

  private float initialTurn;
  private TimingController timing;
  private Text text;

  void Start() {
    initialTurn = Utilities.GetCurrentScore();
    timing = FindObjectOfType<TimingController>();
    text = GetComponent<Text>();
  }

  // Update is called once per frame
  void Update() {
    float totalTurns = timing.GetTurn() + initialTurn;
    text.text = Utilities.FormatScoreAsTime(startHours, secondsPerTurn, totalTurns);

    float hour = startHours + totalTurns * this.secondsPerTurn / 60f / 60f;
    float prop = sunCurve.Evaluate((hour - minHours) / (maxHours - minHours));
    sun.transform.localRotation = Quaternion.Euler(0, Mathf.Lerp(-sunAngle, sunAngle, prop), 0);
  }
}
