using System;
using System.Collections.Generic;
using Enumz;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Attributez;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Grunt : MonoBehaviour {
    public IAttribute Health { get; set; }
    public IAttribute Stamina { get; set; }
    public HealthBar OwnHealthBar { get; set; }
    public StaminaBar OwnStaminaBar { get; set; }

    public HealthBar healthBarPrefab;
    public StaminaBar staminaBarPrefab;

    // Todo: Extract into Equipment class
    public ToolType tool;
    public ToyType toy;

    private GruntAnimationPack animations;

    private void Start() {
      Health = gameObject.AddComponent<Health>();
      Stamina = gameObject.AddComponent<Stamina>();

      OwnHealthBar = Instantiate(healthBarPrefab, transform, false);
      OwnStaminaBar = Instantiate(staminaBarPrefab, transform, false);

      SelectGruntAnimationPack(tool);
    }

    public void SelectGruntAnimationPack(ToolType toolType) {
      animations = toolType switch {
        ToolType.BareHandz => AnimationManager.BareHandzGruntAnimations,
        ToolType.Club => AnimationManager.ClubGruntAnimations,
        ToolType.Gauntletz => AnimationManager.GauntletzGruntAnimations,
        // ToolType.Warpstone1 => AnimationManager.WarpstoneGruntAnimations, // TODO: Add animations
        _ => throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null)
      };
    }
  }
}
