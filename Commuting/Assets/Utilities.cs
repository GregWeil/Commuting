using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {
  public static float GetLerpProp(float prop, float dt, float reference) {
    return 1f - Mathf.Pow(1f - prop, dt * reference);
  }
}
