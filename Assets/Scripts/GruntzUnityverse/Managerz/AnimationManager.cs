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
      LoadDeathAnimations();

      // CursorAnimations = new CursorAnimationPack();
      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
        grunt.SetAnimPack(grunt.equipment.tool.toolName);
      }
    }

    private void LoadDeathAnimations() {
      DeathPack = new Dictionary<string, AnimationClip>();
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Burn.anim").Completed += handle => {
        DeathPack.Add("Burn", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Electrocute.anim").Completed += handle => {
        DeathPack.Add("Electrocute", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Explode.anim").Completed += handle => {
        DeathPack.Add("Explode", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Fall.anim").Completed += handle => {
        DeathPack.Add("Fall", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Flyup.anim").Completed += handle => {
        DeathPack.Add("Flyup", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Freeze.anim").Completed += handle => {
        DeathPack.Add("Freeze", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Hole.anim").Completed += handle => {
        DeathPack.Add("Hole", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Karaoke.anim").Completed += handle => {
        DeathPack.Add("Karaoke", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Melt.anim").Completed += handle => {
        DeathPack.Add("Melt", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Sink.anim").Completed += handle => {
        DeathPack.Add("Sink", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Death_Squash.anim").Completed += handle => {
        DeathPack.Add("Squash", handle.Result);
      };
    }
  }
}
