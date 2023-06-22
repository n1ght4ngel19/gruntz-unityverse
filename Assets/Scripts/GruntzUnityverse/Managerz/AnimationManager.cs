using System.Collections.Generic;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Managerz {
  public class AnimationManager : MonoBehaviour {
    private static AnimationManager _instance;

    public GruntAnimationPack BarehandzGruntPack { get; set; }
    public GruntAnimationPack GauntletzGruntPack { get; set; }
    public GruntAnimationPack ShovelGruntPack { get; set; }
    public PickupAnimationPack PickupPack { get; set; }

    public Dictionary<string, AnimationClip> DeathPack { get; set; }
    // [SerializeField] public CursorAnimationPack CursorAnimations { get; set; }

    public static AnimationManager Instance {
      get => _instance;
    }

    private void Start() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      BarehandzGruntPack = new GruntAnimationPack(ToolName.Barehandz);
      GauntletzGruntPack = new GruntAnimationPack(ToolName.Gauntletz);
      ShovelGruntPack = new GruntAnimationPack(ToolName.Shovel);
      PickupPack = new PickupAnimationPack();
      LoadDeathAnimations();

      // CursorAnimations = new CursorAnimationPack();
    }

    private void LoadDeathAnimations() {
      DeathPack = new Dictionary<string, AnimationClip> {
        {
          "Burn", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Burn")
        }, {
          "Electrocute", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Electrocute")
        }, {
          "Explode", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Explode")
        }, {
          "Fall", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Fall")
        }, {
          "Flyup", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Flyup")
        }, {
          "Freeze", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Freeze")
        }, {
          "Hole", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Hole")
        }, {
          "Karaoke", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Karaoke")
        }, {
          "Melt", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Melt")
        }, {
          "Sink", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Sink")
        }, {
          "Squash", Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Squash")
        },
      };
    }
  }
}
