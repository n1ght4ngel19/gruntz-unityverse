using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Gauntletz : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Gauntletz;
      rangeType = RangeType.Melee;
      damage = GlobalValuez.GauntletzDamage;
    }

    public override IEnumerator UseItem() {
      Vector2Int diffVector = ownGrunt.targetGrunt is null
        ? ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation
        : ownGrunt.targetGrunt.navigator.ownLocation - ownGrunt.navigator.ownLocation;

      ownGrunt.isInterrupted = true;

      ownGrunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));
      
      string gruntType =  $"{toolName}Grunt";

      Addressables.LoadAssetAsync<AnimationClip>(
          $"{GlobalNamez.GruntAnimzPath}{gruntType}/{GlobalNamez.UseAnimzSubPath}{gruntType}_Item_{ownGrunt.navigator.facingDirection}.anim")
        .Completed += handle => {
        ownGrunt.animancer.Play(handle.Result);
      };

      StartCoroutine(((IBreakable)ownGrunt.targetMapObject).Break());

      yield return new WaitForSeconds(2f);

      ownGrunt.CleanState();
      // AnimationClip clipToPlay =
      //   ownGrunt.animationPack.Item[$"{ownGrunt.equipment.tool.toolName}Grunt_Item_{ownGrunt.navigator.facingDirection}"];
      //
      // ownGrunt.animancer.Play(clipToPlay);
      // StartCoroutine(((IBreakable)ownGrunt.targetMapObject).Break());
      //
      // yield return new WaitForSeconds(2f);
    }

    public override IEnumerator Use(Grunt grunt) {
      Vector2Int diffVector = grunt.targetObject.location - grunt.navigator.ownLocation;
      grunt.isInterrupted = true;

      grunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        grunt.animationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.facingDirection}"];

      grunt.animancer.Play(clipToPlay);

      StartCoroutine(((IBreakable)grunt.targetObject).Break());

      yield return new WaitForSeconds(2f);

      grunt.isInterrupted = false;

      if (grunt.targetObject is null) {
        yield break;
      }

      grunt.targetObject = null;
    }
  }
}
