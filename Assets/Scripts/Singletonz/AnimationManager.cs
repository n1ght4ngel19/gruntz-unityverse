using Itemz;


using UnityEngine;

namespace Singletonz {
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

    public static GruntAnimationPack BareHandzGruntAnimations = new(ToolType.BareHandz);
    public static GruntAnimationPack ClubGruntAnimations = new(ToolType.Club);
    public static GruntAnimationPack GauntletzGruntAnimations = new(ToolType.Gauntletz);
  }
}
