using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour {
  public MapBehavior map;
  public TimingController timing;
  public AnimationCurve curve;

  private int turn;
  private Vector2Int position;
  private Vector2Int from;
  private Vector2Int to;

  void Start() {
    this.turn = timing.GetTurnIndex();
    this.position = map.WorldToGrid(transform.position);
    this.from = Vector2Int.down;
    this.to = Vector2Int.up;
  }

  void Update() {
    while (timing.GetTurnIndex() > this.turn) {
      this.position += this.to;
      this.from = -this.to;
      this.to = GetNextDirection(map.GetTile(this.position), this.from);
      this.turn += 1;
    }

    float turnFrac = timing.GetTurnFrac();
    if (turnFrac < 0.5f) {
      Vector2Int input = GetInputDirection(map.GetTile(this.position), this.from);
      if (!input.Equals(Vector2Int.zero)) this.to = input;
    }

    Vector2 pos = this.position;
    Vector2 fromPos = this.from;
    Vector2 toPos = this.to;
    if (turnFrac < 0.5f) {
      toPos = -this.from;
    } else {
      fromPos = -this.to;
    }
    Vector3 target = Vector3.Lerp(map.GridToWorld(pos + fromPos / 2f), map.GridToWorld(pos + toPos / 2f), curve.Evaluate(turnFrac));
    this.transform.LookAt(target);
    this.transform.position = target;
  }

  static Vector2Int GetInputDirection(MapTile tile, Vector2Int from) {
    if (tile == null) return Vector2Int.zero;
    Vector2Int input = Vector2Int.zero;
    if (Input.GetKey(KeyCode.UpArrow)) input += Vector2Int.up;
    if (Input.GetKey(KeyCode.DownArrow)) input += Vector2Int.down;
    if (Input.GetKey(KeyCode.LeftArrow)) input += Vector2Int.left;
    if (Input.GetKey(KeyCode.RightArrow)) input += Vector2Int.right;
    if (!input.Equals(from) && tile.Allows(input)) return input;
    return Vector2Int.zero;
  }

  static Vector2Int GetNextDirection(MapTile tile, Vector2Int from) {
    if (tile == null) return from;
    Vector2Int input = GetInputDirection(tile, from);
    if (!input.Equals(Vector2Int.zero)) return input;

    if (tile.Allows(-from)) return -from;
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    foreach (Vector2Int direction in tile.AllowedDirections()) {
      if (!direction.Equals(from)) {
        return direction;
      }
    }
    return from;
  }
}
