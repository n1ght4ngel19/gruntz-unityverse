using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Gauntletz : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Gauntletz;
      toolRange = RangeType.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.GauntletzDamage;
      mapItemName = nameof(Gauntletz);
      contactDelay = 1.5f;
      attackContactDelay = 0.4f;
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator UseItem() {
      Vector2Int diffVector = ownGrunt.targetGrunt is null
        ? ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation
        : ownGrunt.targetGrunt.navigator.ownLocation - ownGrunt.navigator.ownLocation;

      ownGrunt.isInterrupted = true;

      ownGrunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      string gruntType = $"{toolName}Grunt";
      AnimationClip clipToPlay =
        ownGrunt.animationPack.Item[$"{gruntType}_Item_{ownGrunt.navigator.facingDirection}"];

      ownGrunt.animancer.Play(clipToPlay);

      StartCoroutine(((IBreakable)ownGrunt.targetMapObject).Break(contactDelay));

      yield return new WaitForSeconds(2f);

      ownGrunt.CleanState();
    }

    public override IEnumerator Use(Grunt grunt) {
      Vector2Int diffVector = grunt.targetObject.location - grunt.navigator.ownLocation;
      grunt.isInterrupted = true;

      grunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        grunt.animationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.facingDirection}"];

      grunt.animancer.Play(clipToPlay);

      StartCoroutine(((IBreakable)grunt.targetObject).Break(contactDelay));

      yield return new WaitForSeconds(2f);

      grunt.isInterrupted = false;

      if (grunt.targetObject is null) {
        yield break;
      }

      grunt.targetObject = null;
    }
  }
}
