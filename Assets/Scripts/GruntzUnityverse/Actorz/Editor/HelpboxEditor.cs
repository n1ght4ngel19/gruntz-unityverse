using GruntzUnityverse.Itemz.MiscItemz;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.Actorz.Editor {
  [CustomEditor(typeof(Helpbox), true), CanEditMultipleObjects]
  public class HelpboxEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      Helpbox inspected = (Helpbox)target;

      inspected.boxText = EditorGUILayout.TextArea(inspected.boxText, GUILayout.Height(200));
    }
  }
}
