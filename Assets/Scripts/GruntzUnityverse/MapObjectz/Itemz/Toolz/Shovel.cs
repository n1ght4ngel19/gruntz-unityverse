using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Shovel : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Shovel;
      rangeType = RangeType.Melee;
      damage = GlobalValuez.ShovelDamage;
    }

    public override IEnumerator Use(Grunt grunt) {
      Vector2Int diffVector = grunt.targetObject.location - grunt.navigator.ownLocation;
      grunt.isInterrupted = true;

      grunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        // Todo: Replace with right animation
        grunt.animationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.facingDirection}"];

      grunt.animancer.Play(clipToPlay);

      StartCoroutine(grunt.targetObject.BeUsed(grunt));

      yield return new WaitForSeconds(1.6f);

      grunt.isInterrupted = false;
    }
  }
}
