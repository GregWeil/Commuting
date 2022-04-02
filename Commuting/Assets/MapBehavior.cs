using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehavior : MonoBehaviour {
  public float gridSize;
  private Dictionary<(int, int), MapTile> grid = new Dictionary<(int, int), MapTile>();

  void Start() {
    this.buildGrid();
  }

  void Update() {
  }

  public void buildGrid() {
    grid.Clear();
    foreach (MapTile tile in this.GetComponentsInChildren<MapTile>()) {
      grid[WorldToGrid(tile.transform.position)] = tile;
    }
  }

  public Dictionary<(int, int), MapTile> GetGrid() {
    return this.grid;
  }

  public (int x, int y) WorldToGrid(Vector3 pos) {
    return WorldToGrid(pos.x, pos.z);
  }

  public (int x, int y) WorldToGrid(float x, float z) {
    Vector3 origin = transform.position;
    int gridX = Mathf.RoundToInt((x - origin.x) / gridSize);
    int gridY = Mathf.RoundToInt((z - origin.z) / gridSize);
    return (gridX, gridY);
  }

  public Vector3 GridToWorld(float x, float y) {
    return transform.position + new Vector3(x, 0, y) * gridSize;
  }
}
