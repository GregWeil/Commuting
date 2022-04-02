using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapBehavior))]
public class MapEditor : Editor {
  GameObject selectedPrefab;

  public override void OnInspectorGUI() {
    base.OnInspectorGUI();
    selectedPrefab = (GameObject)EditorGUILayout.ObjectField("Tile", selectedPrefab, typeof(GameObject), false);
  }

  public void OnSceneGUI() {
    MapBehavior map = (MapBehavior)target;
    Selection.activeGameObject = map.gameObject;

    Plane plane = new Plane(Vector3.up, Vector3.zero);
    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
    float distance;
    if (!plane.Raycast(ray, out distance)) return;

    map.buildGrid();
    Vector2Int pos = map.WorldToGrid(ray.GetPoint(distance));
    MapTile tile = map.GetTile(pos);

    Handles.color = tile != null ? Color.red : Color.green;
    Handles.DrawPolyLine(
      map.GridToWorld(pos.x - 0.5f, pos.y - 0.5f),
      map.GridToWorld(pos.x + 0.5f, pos.y - 0.5f),
      map.GridToWorld(pos.x + 0.5f, pos.y + 0.5f),
      map.GridToWorld(pos.x - 0.5f, pos.y + 0.5f),
      map.GridToWorld(pos.x - 0.5f, pos.y - 0.5f)
    );
    SceneView.RepaintAll();

    if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
      if (Event.current.control) {
        if (tile != null) {
          tile.transform.Rotate(0f, 90f, 0f);
          bool tmp = tile.north;
          tile.north = tile.west;
          tile.west = tile.south;
          tile.south = tile.east;
          tile.east = tmp;
        }
      } else {
        if (tile != null) GameObject.DestroyImmediate(tile.gameObject);
        if (selectedPrefab != null) {
          GameObject.Instantiate(selectedPrefab, map.GridToWorld(pos), Quaternion.identity, map.transform);
        }
      }
    }
  }
}
