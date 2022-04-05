using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTile : MonoBehaviour {
  void OnTileEnter() {
    foreach (PlayerCar player in FindObjectsOfType<PlayerCar>()) {
      player.Stop(true);
    }
  }

  void OnTileExit() {
    foreach (PlayerCar player in FindObjectsOfType<PlayerCar>()) {
      //player.Stop(true);
    }
  }
}
