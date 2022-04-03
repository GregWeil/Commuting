using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehavior : MonoBehaviour {
  public float gridSize;
  private Dictionary<Vector2Int, MapTile> grid = new Dictionary<Vector2Int, MapTile>();

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

  public IEnumerable<Vector2Int> getGridCoords() {
    return grid.Keys;
  }

  public MapTile GetTile(Vector2Int position) {
    return this.grid.ContainsKey(position) ? this.grid[position] : null;
  }

  public Vector2Int WorldToGrid(Vector3 position) {
    return WorldToGrid(position.x, position.z);
  }

  public Vector2Int WorldToGrid(float x, float z) {
    Vector3 origin = transform.position;
    int gridX = Mathf.RoundToInt((x - origin.x) / gridSize);
    int gridY = Mathf.RoundToInt((z - origin.z) / gridSize);
    return new Vector2Int(gridX, gridY);
  }

  public Vector3 GridToWorld(Vector2 position) {
    return GridToWorld(position.x, position.y);
  }

  public Vector3 GridToWorld(float x, float y) {
    return transform.position + new Vector3(x, 0, y) * gridSize;
  }
}
