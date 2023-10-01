using System.Collections.Generic;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Managerz {
  public class AnimationManager : MonoBehaviour {
    public GruntAnimationPack barehandzGruntPack;
    public GruntAnimationPack clubGruntPack;
    public GruntAnimationPack gauntletzGruntPack;
    public GruntAnimationPack gooberStrawGruntPack;
    public GruntAnimationPack shovelGruntPack;
    public GruntAnimationPack warpstoneGruntPack;
    public PickupAnimationPack pickupPack;

    public Dictionary<string, AnimationClip> deathPack;
    public Dictionary<string, AnimationClip> exitPack;

    // [SerializeField] public CursorAnimationPack CursorAnimations { get; set; }

    private void OnEnable() {
      barehandzGruntPack = new GruntAnimationPack(ToolName.Barehandz);
      clubGruntPack = new GruntAnimationPack(ToolName.Club);
      gauntletzGruntPack = new GruntAnimationPack(ToolName.Gauntletz);
      gooberStrawGruntPack = new GruntAnimationPack(ToolName.GooberStraw);
      shovelGruntPack = new GruntAnimationPack(ToolName.Shovel);
      warpstoneGruntPack = new GruntAnimationPack(ToolName.Warpstone);
      pickupPack = new PickupAnimationPack();

      deathPack = new Dictionary<string, AnimationClip>();
      exitPack = new Dictionary<string, AnimationClip>();

      LoadDeathAnimations();
      LoadExitAnimations();

      // CursorAnimations = new CursorAnimationPack();
      // foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz) {
      //   grunt.SetAnimPack(grunt.equipment.tool.toolName);
      // }
    }

    private void LoadDeathAnimations() {
      foreach (DeathName deathName in System.Enum.GetValues(typeof(DeathName))) {
        if (deathName == DeathName.Default) {
          continue;
        }

        Addressables.LoadAssetAsync<AnimationClip>($"Grunt_Death_{deathName}.anim").Completed += handle => {
          deathPack.Add(deathName.ToString(), handle.Result);
        };
      }
    }

    private void LoadExitAnimations() {
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Exit_01.anim").Completed += handle => {
        exitPack.Add("Grunt_Exit_01", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Exit_02.anim").Completed += handle => {
        exitPack.Add("Grunt_Exit_02", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Exit_03.anim").Completed += handle => {
        exitPack.Add("Grunt_Exit_03", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Grunt_Exit_End.anim").Completed += handle => {
        exitPack.Add("Grunt_Exit_End", handle.Result);
      };
    }
  }
}
