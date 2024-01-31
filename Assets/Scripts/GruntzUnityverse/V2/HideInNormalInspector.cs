using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.V2 {
  public class HideInNormalInspector : PropertyAttribute { }

  #if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(HideInNormalInspector))]
  public class HideInNormalInspectorDrawer : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      return 0f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) { }
  }
  #endif
}
