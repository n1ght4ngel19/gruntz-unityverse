using GruntzUnityverse.MapObjectz.Arrowz;
using UnityEditor;

namespace GruntzUnityverse.Editor {
  [CustomEditor(typeof(OneWayArrow))]
  public class OneWayArrowEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      OneWayArrow inspected = (OneWayArrow)target;

      EditorGUILayout.LabelField("Object Id", inspected.objectId.ToString());
    }
  }
}
