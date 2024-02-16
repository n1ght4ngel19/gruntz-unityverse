using UnityEditor;

namespace GruntzUnityverse.V2.Utils {
  public static class CustomEditorLayout {
    public static float IncrementSlider(string label, float value, float min, float max, float increment) {
      return EditorGUILayout.Slider(label, value, min, max).SnappedToIncrement(increment);
    }
    
    public static int IncrementIntSlider(string label, int value, int min, int max, int increment) {
      return EditorGUILayout.IntSlider(label, value, min, max).SnappedToIncrement(increment);
    }
    
    // public static float IncrementSlider(string label, double value, float min, float max, double increment) {
    //   return EditorGUILayout.Slider(label, value, min, max).SnappedToIncrement(increment);
    // }

  }
}
