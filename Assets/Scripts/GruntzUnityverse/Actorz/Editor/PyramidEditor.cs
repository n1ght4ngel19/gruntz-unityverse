using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEditor;

namespace GruntzUnityverse.Actorz.Editor {
  [CustomEditor(typeof(Pyramid), true), CanEditMultipleObjects]
  public class PyramidEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      Pyramid inspected = (Pyramid)target;

      EditorGUILayout.LabelField("Object Id", inspected.objectId.ToString());
    }
  }
}
