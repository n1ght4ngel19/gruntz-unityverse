using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.V2.Editor {
  [CustomEditor(typeof(GruntV2))]
  public class GruntV2Editor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      GruntV2 grunt = (GruntV2)target;

      
      if (GUILayout.Button("Test Pathfinding")) {
        grunt.TestPathfinding();
      }
    }
  }
}
