using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class AnimationManager : MonoBehaviour {
    private static AnimationManager _instance;

    public GruntAnimationPack GauntletzGruntPack { get; set; }
    public PickupAnimationPack PickupPack { get; set; }
    // [SerializeField] public CursorAnimationPack CursorAnimations { get; set; }

    public static AnimationManager Instance {
      get => _instance;
    }

    private void Awake() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      GauntletzGruntPack = new GruntAnimationPack(ToolType.Gauntletz);
      PickupPack = new PickupAnimationPack();
      // CursorAnimations = new CursorAnimationPack();
    }


    // public static readonly DeathAnimationPack DeathAnimations = new();
    // public static readonly GruntAnimationPack BareHandzGruntAnimations = new(ToolType.None);
    // public static readonly GruntAnimationPack ClubGruntAnimations = new(ToolType.Club);

    // public static readonly ToolAnimationPack ToolAnimations = new();
    // public static readonly ToyAnimationPack ToyAnimations = new();
  }
}
