using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.Editor {
  [CustomEditor(typeof(Rock))]
  public class RockEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      Rock inspected = (Rock)target;

      EditorGUILayout.LabelField("Object Id", inspected.objectId.ToString());

      GUILayout.BeginHorizontal();

      if (GUILayout.Button("Reset", GUILayout.Width(100))) {
        ResetHiddenItem(inspected);
      }

      EditorGUILayout.LabelField("Hidden Item", GUILayout.Width(100));

      inspected.hiddenItem = (MapObject)EditorGUILayout.ObjectField(inspected.hiddenItem, typeof(MapObject), true);
      GUILayout.EndHorizontal();
    }

    public void ResetHiddenItem(Rock rock) {
      rock.hiddenItem = null;
    }
  }
}
