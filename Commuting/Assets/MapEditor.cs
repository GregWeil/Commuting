using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapBehavior))]
public class MapEditor : Editor {
  string selectedPrefabPath = "";

  public override void OnInspectorGUI() {
    base.OnInspectorGUI();
    string[] assets = AssetDatabase.FindAssets("", new[] { "Assets/RoadPieces" })
        .Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
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
        if (selectedPrefabPath != "") {
          GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(selectedPrefabPath);
          GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, map.transform);
          obj.transform.position = map.GridToWorld(pos);
        }
      }
    }
  }
}
