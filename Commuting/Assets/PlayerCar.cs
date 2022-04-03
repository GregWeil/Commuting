using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCar : MonoBehaviour {
  public MapBehavior map;
  public TimingController timing;
  public float laneOffset;
  public AnimationCurve curve;
  public float speed;
  public GameObject emitter;

  private int turn;
  private Vector2Int position;
  private Vector2Int from;
  private Vector2Int to;

  void Start() {
    this.turn = timing.GetTurnIndex();
    this.position = map.WorldToGrid(transform.position);
    this.to = new Vector2Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z));
    this.from = -this.to;
  }

  void Update() {
    while (timing.GetTurnIndex() > this.turn) {
      map.GetTile(this.position)?.Invalidate(this.to);
      this.position += this.to;
      this.from = -this.to;
      MapTile tile = map.GetTile(this.position);
      if (tile != null && !tile.AllowedDirections().Any()) {
        this.emitter.SetActive(true);
        this.enabled = false;
      }
      this.to = GetNextDirection(tile, this.from);
      this.turn += 1;
    }

    float turnFrac = timing.GetTurnFrac();
    if (turnFrac < 0.9f) {
      Vector2Int input = GetInputDirection(map.GetTile(this.position), this.from);
      if (!input.Equals(Vector2Int.zero)) this.to = input;
    }

    float turnProp = this.curve.Evaluate(turnFrac);
    Vector2 pos = this.position;
    Vector2 fromLaneOffset = new Vector2(-this.from.y, this.from.x) * laneOffset;
    Vector2 fromPos = this.from + fromLaneOffset;
    Vector2 toLaneOffset = new Vector2(this.to.y, -this.to.x) * laneOffset;
    Vector2 toPos = this.to + toLaneOffset;
    Vector2 midPos = this.to.Equals(-this.from) ? toLaneOffset : toLaneOffset + fromLaneOffset;
    if (turnProp < 0.5f) {
      toPos = 2 * midPos - fromPos;
    } else {
      fromPos = 2 * midPos - toPos;
    }
    Vector3 target = Vector3.Lerp(map.GridToWorld(pos + fromPos / 2f), map.GridToWorld(pos + toPos / 2f), turnProp);

    float prop = Utilities.GetLerpProp(speed, timing.GetTurnDt(), 60f);
    Vector3 nextPosition = Vector3.Lerp(this.transform.position, target, prop);
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target - transform.position), prop);
    this.transform.position = nextPosition;
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
    if (tile == null) return -from;
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
