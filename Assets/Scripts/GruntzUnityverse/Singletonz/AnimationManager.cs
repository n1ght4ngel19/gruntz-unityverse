using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz;

using UnityEngine;

namespace GruntzUnityverse.Singletonz {
  public class AnimationManager : MonoBehaviour {
    private static AnimationManager _instance;

    public static AnimationManager Instance {
      get => _instance;
    }
  
    private void Awake() {
      if (_instance != null && _instance != this)
        Destroy(gameObject);
      else
        _instance = this;
    }

    public static readonly GruntAnimationPack BareHandzGruntAnimations = new(ToolType.BareHandz);
    public static readonly GruntAnimationPack ClubGruntAnimations = new(ToolType.Club);
    public static readonly GruntAnimationPack GauntletzGruntAnimations = new(ToolType.Gauntletz);

    public static readonly CursorAnimationPack CursorAnimations = new();
    
    public static readonly ToolAnimationPack ToolAnimations = new ();
    public static readonly ToyAnimationPack ToyAnimations = new ();
  }
}
