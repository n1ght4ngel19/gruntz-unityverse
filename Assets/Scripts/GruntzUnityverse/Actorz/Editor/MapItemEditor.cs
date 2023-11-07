using UnityEditor;

namespace GruntzUnityverse.Actorz.Editor {
  [CustomEditor(typeof(MapItemEditor), true), CanEditMultipleObjects]
  public class MapItemEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() { }
  }
}
