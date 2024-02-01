using UnityEngine;

namespace GruntzUnityverse.V2 {
  [CreateAssetMenu(fileName = "HelpboxText", menuName = "Gruntz Unityverse/Helpbox Text")]
  public class HelpboxText : ScriptableObject {
    [TextArea(3, 10)]
    public string text;
    
    // Todo: Translations
  }
}
