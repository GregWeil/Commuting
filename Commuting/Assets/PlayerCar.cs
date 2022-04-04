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
  public float turnSpeed;
  public GameObject emitter;

  private int turn;
  private bool inputLocked;
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
      MapTile prevTile = map.GetTile(this.position);
      if (prevTile != null) {
        prevTile.SendMessage("OnTileExit", this.to, SendMessageOptions.DontRequireReceiver);
      }
      this.position += this.to;
      this.from = -this.to;
      MapTile tile = map.GetTile(this.position);
      if (tile == null) {
        this.Stop(true);
      } else {
        tile.SendMessage("OnTileEnter", this.from, SendMessageOptions.DontRequireReceiver);
        if (!tile.AllowedDirections().Any()) this.Stop(true);
      }
      this.to = GetNextDirection(tile, this.from, this.inputLocked);
      this.turn += 1;
    }

    float turnFrac = timing.GetTurnFrac();
    if (turnFrac < 0.9f && !inputLocked) {
      Vector2Int input = GetInputDirection(map.GetTile(this.position), this.from);
      if (!input.Equals(Vector2Int.zero)) this.to = input;
    }

    float turnProp = this.curve.Evaluate(turnFrac);
    Vector3 target = GetTargetPosition(turnProp);

    Vector3 nextPosition = Vector3.Lerp(this.transform.position, target,
      Utilities.GetLerpProp(speed, timing.GetTurnDt(), 60f));
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
      Quaternion.LookRotation(target - transform.position),
      Utilities.GetLerpProp(turnSpeed, timing.GetTurnDt(), 60f));
    this.transform.position = nextPosition;
  }

  static Vector2Int GetInputDirection(MapTile tile, Vector2Int from) {
    if (tile == null) return Vector2Int.zero;
    Vector2Int input = Vector2Int.zero;
    if (Input.GetAxis("Horizontal") < 0) input += Vector2Int.left;
    if (Input.GetAxis("Horizontal") > 0) input += Vector2Int.right;
    if (Input.GetAxis("Vertical") > 0) input += Vector2Int.up;
    if (Input.GetAxis("Vertical") < 0) input += Vector2Int.down;
    if (!input.Equals(from) && tile.Allows(input)) return input;
    return Vector2Int.zero;
  }

  static Vector2Int GetNextDirection(MapTile tile, Vector2Int from, bool inputLocked) {
    if (tile == null) return -from;
    Vector2Int input = GetInputDirection(tile, from);
    if (!input.Equals(Vector2Int.zero) && !inputLocked) return input;

    if (tile.Allows(-from)) return -from;
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    foreach (Vector2Int direction in tile.AllowedDirections()) {
      if (!direction.Equals(from)) {
        return direction;
      }
    }
    return from;
  }

  Vector3 GetTargetPosition(float turnProp) {
    Vector2 pos = position;
    Vector2 fromLaneOffset = new Vector2(-from.y, from.x) * laneOffset;
    Vector2 fromPos = (from + fromLaneOffset) / 2f;
    Vector2 toLaneOffset = new Vector2(to.y, -to.x) * laneOffset;
    Vector2 toPos = (to + toLaneOffset) / 2f;

    if (to.Equals(-from)) {
      return Vector3.Lerp(map.GridToWorld(pos + fromPos), map.GridToWorld(pos + toPos), turnProp);
    }

    if (!to.Equals(from)) {
      Vector2 center = new Vector2(from.x + to.x, from.y + to.y) / 2f;
      Vector2 fromOffset = fromPos - center;
      float toRadius = (toPos - center).magnitude;
      float startAngle = Mathf.Atan2(fromOffset.y, fromOffset.x);
      float difference = Vector2.SignedAngle(fromOffset, toPos - center);
      float radius = Mathf.Lerp(fromOffset.magnitude, toRadius, turnProp);
      float angle = startAngle + Mathf.Deg2Rad * difference * turnProp;
      Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
      return map.GridToWorld(pos + center + offset);
    }

    Vector2 midPos = toLaneOffset + fromLaneOffset;
    if (turnProp < 0.5f) {
      toPos = midPos - fromPos / 2f;
    } else {
      fromPos = midPos - toPos / 2f;
    }
    return Vector3.Lerp(map.GridToWorld(pos + fromPos), map.GridToWorld(pos + toPos), turnProp);
  }

  public void Stop(bool crash) {
    if (crash && !inputLocked) {
      this.emitter.SetActive(true);
    }
    this.enabled = false;
  }

  public void LockInput() {
    this.inputLocked = true;
  }
}
