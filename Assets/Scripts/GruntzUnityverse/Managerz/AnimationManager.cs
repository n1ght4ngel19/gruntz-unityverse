using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class AnimationManager : MonoBehaviour {
    private static AnimationManager _instance;

    public GruntAnimationPack GauntletzGruntPack { get; set; }
    public GruntAnimationPack ShovelGruntPack { get; set; }
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

      GauntletzGruntPack = new GruntAnimationPack(ToolName.Gauntletz);
      ShovelGruntPack = new GruntAnimationPack(ToolName.Shovel);
      PickupPack = new PickupAnimationPack();
      // CursorAnimations = new CursorAnimationPack();
    }
  }
}
