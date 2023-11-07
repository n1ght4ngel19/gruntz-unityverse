using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.Actorz.Editor {
  [CustomEditor(typeof(Hole)), CanEditMultipleObjects]
  public class HoleEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      Hole inspected = (Hole)target;

      EditorGUILayout.LabelField("Object Id", inspected.objectId.ToString());

      GUILayout.BeginHorizontal();

      if (GUILayout.Button("Reset", GUILayout.Width(100))) {
        ResetHiddenItem(inspected);
      }

      EditorGUILayout.LabelField("Hidden Item", GUILayout.Width(100));

      inspected.hiddenItem = (MapObject)EditorGUILayout.ObjectField(inspected.hiddenItem, typeof(MapObject), true);
      GUILayout.EndHorizontal();
    }

    public void ResetHiddenItem(Hole hole) {
      hole.hiddenItem = null;
    }
  }
}
