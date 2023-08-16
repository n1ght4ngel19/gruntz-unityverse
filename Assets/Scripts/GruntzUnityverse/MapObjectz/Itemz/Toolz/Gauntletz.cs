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
      rangeType = RangeType.Melee;
    }

    public override IEnumerator UseItem() {
      Vector2Int diffVector = ownGrunt.targetGrunt is null
        ? ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation
        : ownGrunt.targetGrunt.navigator.ownLocation - ownGrunt.navigator.ownLocation;

      ownGrunt.isInterrupted = true;

      ownGrunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        ownGrunt.animationPack.Item[$"{ownGrunt.equipment.tool.toolName}Grunt_Item_{ownGrunt.navigator.facingDirection}"];

      ownGrunt.animancer.Play(clipToPlay);
      StartCoroutine(((IBreakable)ownGrunt.targetMapObject).Break());

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
