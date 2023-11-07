using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEditor;

namespace GruntzUnityverse.Editor {
  [CustomEditor(typeof(ObjectSwitch), true), CanEditMultipleObjects]
  public class SwitchEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      ObjectSwitch inspected = (ObjectSwitch)target;

      EditorGUILayout.LabelField("Object Id", inspected.objectId.ToString());
      string state = inspected.isPressed ? "Pressed" : "Released";
      EditorGUILayout.LabelField("State", state);
    }
  }
}
