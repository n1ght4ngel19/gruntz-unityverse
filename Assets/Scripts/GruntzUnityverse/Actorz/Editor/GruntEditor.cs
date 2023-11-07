using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.Actorz.Editor {
  [CustomEditor(typeof(Grunt))]
  public class GruntEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      Grunt inspected = (Grunt)target;
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Global Id", inspected.gruntId.ToString());
      EditorGUILayout.LabelField("Id", inspected.gruntId.ToString());
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.Space();
      
      EditorGUILayout.LabelField("Statz");

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Direction", "East");
      EditorGUILayout.LabelField("Direction", "East");
      EditorGUILayout.LabelField("Direction", "East");
      EditorGUILayout.LabelField("Direction", "East");
      EditorGUILayout.EndHorizontal();
    }
  }
}
