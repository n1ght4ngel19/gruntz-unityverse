using System.Collections.Generic;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Managerz {
  public class AnimationManager : MonoBehaviour {
    public GruntAnimationPack BarehandzGruntPack { get; set; }
    public GruntAnimationPack GauntletzGruntPack { get; set; }
    public GruntAnimationPack ShovelGruntPack { get; set; }
    public PickupAnimationPack PickupPack { get; set; }

    public Dictionary<string, AnimationClip> DeathPack { get; set; }
    // [SerializeField] public CursorAnimationPack CursorAnimations { get; set; }

    public static AnimationManager Instance { get; private set; }

    private void Start() {
      if (Instance is not null && Instance != this) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }

      BarehandzGruntPack = new GruntAnimationPack(ToolName.Barehandz);
      GauntletzGruntPack = new GruntAnimationPack(ToolName.Gauntletz);
      ShovelGruntPack = new GruntAnimationPack(ToolName.Shovel);
      PickupPack = new PickupAnimationPack();
      DeathPack = new Dictionary<string, AnimationClip>();

      LoadDeathAnimations();

      // CursorAnimations = new CursorAnimationPack();
      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
        grunt.SetAnimPack(grunt.equipment.tool.toolName);
      }
    }
    // -------------------------------------------------------------------------------- //

    private void LoadDeathAnimations() {
      foreach (DeathName deathName in System.Enum.GetValues(typeof(DeathName))) {
        if (deathName == DeathName.Default) {
          continue;
        }

        Addressables.LoadAssetAsync<AnimationClip>($"Grunt_Death_{deathName}.anim").Completed += handle => {
          DeathPack.Add(deathName.ToString(), handle.Result);
        };
      }
    }
  }
}
