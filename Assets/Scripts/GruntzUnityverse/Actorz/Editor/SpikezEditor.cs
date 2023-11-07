using GruntzUnityverse.MapObjectz.Hazardz;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.Actorz.Editor {
  [CustomEditor(typeof(Spikez), true), CanEditMultipleObjects]
  public class SpikezEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      Spikez inspected = (Spikez)target;

      EditorGUILayout.LabelField("Object Id", inspected.objectId.ToString());

      GUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Hidden Item", GUILayout.Width(100));
      inspected.damage = EditorGUILayout.IntSlider(inspected.damage, 0, 40);
      GUILayout.EndHorizontal();
    }
  }
}
