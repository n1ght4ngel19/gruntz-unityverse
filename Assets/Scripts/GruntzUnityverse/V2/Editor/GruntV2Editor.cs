using GruntzUnityverse.V2.Grunt;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.V2.Editor {
  [CustomEditor(typeof(GruntV2))]
  public class GruntV2Editor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      GruntV2 grunt = (GruntV2)target;
      
      GUILayout.Space(10);
      
      if (GUILayout.Button("Test Pathfinding")) {
        grunt.TestPathfinding();
      }

      GUILayout.Space(10);

      if (GUILayout.Button("Generate Guid")) {
        grunt.GenerateGuid();
      }

      GUILayout.Space(10);

      base.OnInspectorGUI();
    }
  }
}
