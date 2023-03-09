using GruntzUnityverse.AnimationPackz;
using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class AnimationManager : MonoBehaviour {
    private static AnimationManager _instance;


    #region Singleton Stuff

    public static AnimationManager Instance {
      get => _instance;
    }

    private void Awake() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }
    }

    #endregion


    [field: SerializeField] public AnimatorOverrideController GauntletzGruntAnimationz { get; set; }

    // public static readonly DeathAnimationPack DeathAnimations = new();
    // public static readonly GruntAnimationPack BareHandzGruntAnimations = new(ToolType.None);
    // public static readonly GruntAnimationPack ClubGruntAnimations = new(ToolType.Club);
    // public static readonly GruntAnimationPack GauntletzGruntAnimations = new(ToolType.Gauntletz);
    public static readonly CursorAnimationPack CursorAnimations = new CursorAnimationPack();
    // public static readonly ToolAnimationPack ToolAnimations = new();
    // public static readonly ToyAnimationPack ToyAnimations = new();
  }
}
