using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapBehavior))]
public class MapEditor : Editor {
  string selectedPrefabPath = "";

  public override void OnInspectorGUI() {
    MapBehavior map = (MapBehavior)target;
    float newGridSize = EditorGUILayout.FloatField("Grid Size", map.gridSize);
    if (newGridSize != map.gridSize && newGridSize != 0) {
      map.buildGrid();
      map.gridSize = newGridSize;
      foreach (Vector2Int cell in map.getGridCoords()) {
        map.GetTile(cell).transform.position = map.GridToWorld(cell);
      }
    }

    string[] assets = getPrefabAssetPaths();
    GUIContent[] content = assets.Select(path => {
      GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
      Texture image = AssetDatabase.GetCachedIcon(path);
      return new GUIContent(obj.name, image);
    }).Concat(new[] { new GUIContent("None") }).ToArray();
    int index = System.Array.IndexOf(assets, selectedPrefabPath);
    index = EditorGUILayout.Popup(index < 0 ? content.Length - 1 : index, content);
    if (index >= 0 && index < assets.Length) selectedPrefabPath = assets[index];
    else selectedPrefabPath = "";
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

    if (Event.current.type == EventType.MouseDown) {
      if (Event.current.control && Event.current.button == 0) {
        if (tile != null) {
          Undo.RecordObject(tile.transform, tile.name);
          Undo.RecordObject(tile, tile.name);
          tile.transform.Rotate(0f, 90f, 0f);
          bool tmp = tile.north;
          tile.north = tile.west;
          tile.west = tile.south;
          tile.south = tile.east;
          tile.east = tmp;
          EditorUtility.SetDirty(tile.transform);
          EditorUtility.SetDirty(tile);
          PrefabUtility.RecordPrefabInstancePropertyModifications(tile.transform);
          PrefabUtility.RecordPrefabInstancePropertyModifications(tile);
        }
      } else if (Event.current.control && Event.current.button == 1) {
        selectedPrefabPath = tile == null ? "" : PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(tile.gameObject);
        this.Repaint();
      } else if (Event.current.button == 0) {
        if (tile != null) {
          Undo.DestroyObjectImmediate(tile.gameObject);
        }
        if (selectedPrefabPath != "") {
          GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(selectedPrefabPath);
          GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, map.transform);
          instance.transform.position = map.GridToWorld(pos);
          Undo.RegisterCreatedObjectUndo(instance, instance.name);
        }
      }
    }
  }

  string[] getPrefabAssetPaths() {
    return AssetDatabase.FindAssets("", new[] { "Assets/RoadPieces" })
        .Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
  }
}
