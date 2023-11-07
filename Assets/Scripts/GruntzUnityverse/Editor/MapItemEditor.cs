using UnityEditor;

namespace GruntzUnityverse.Editor {
  [CustomEditor(typeof(MapItemEditor), true), CanEditMultipleObjects]
  public class MapItemEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() { }
  }
}
