using GruntzUnityverse.V2.Core;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.V2.Editor {
  [CustomEditor(typeof(LevelV2))]
  public class LevelV2Editor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();
      
      LevelV2 level = (LevelV2) target;
      
      if (GUILayout.Button("Initialize")) {
        level.Initialize();
      }
    }
  }
}
