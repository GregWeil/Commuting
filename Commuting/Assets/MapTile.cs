using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour {
  public bool north;
  public bool south;
  public bool east;
  public bool west;

  public IEnumerable<Vector2Int> AllowedDirections() {
    if (this.north) yield return Vector2Int.up;
    if (this.south) yield return Vector2Int.down;
    if (this.east) yield return Vector2Int.right;
    if (this.west) yield return Vector2Int.left;
  }

  public bool Allows(Vector2Int direction) {
    if (Vector2Int.up.Equals(direction)) return this.north;
    if (Vector2Int.down.Equals(direction)) return this.south;
    if (Vector2Int.right.Equals(direction)) return this.east;
    if (Vector2Int.left.Equals(direction)) return this.west;
    return false;
  }

  public void Invalidate(Vector2Int direction) {
    if (Vector2Int.up.Equals(direction)) this.north = false;
    if (Vector2Int.down.Equals(direction)) this.south = false;
    if (Vector2Int.right.Equals(direction)) this.east = false;
    if (Vector2Int.left.Equals(direction)) this.west = false;
  }
}
