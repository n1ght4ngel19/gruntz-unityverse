using GruntzUnityverse.MapObjectz;
using UnityEditor;

namespace GruntzUnityverse.Editor {
  [CustomEditor(typeof(Flag), true), CanEditMultipleObjects]
  public class FlagEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      Flag inspected = (Flag)target;

      EditorGUILayout.LabelField("Object Id", inspected.objectId.ToString());
    }
  }
}
