using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Gauntletz : Tool {
    protected override void Start() {
      toolName = ToolName.Gauntletz;
    }

    public override IEnumerator Use(Grunt grunt) {
      Vector2Int diffVector = grunt.TargetObject.location - grunt.navigator.ownLocation;
      grunt.IsInterrupted = true;

      grunt.navigator.ChangeFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        grunt.AnimationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.facingDirection}"];

      grunt.animancer.Play(clipToPlay);

      StartCoroutine(((IBreakable)grunt.TargetObject).Break());

      yield return new WaitForSeconds(2f);

      grunt.IsInterrupted = false;

      if (grunt.TargetObject is null) {
        yield break;
      }

      grunt.TargetObject = null;
    }
  }
}
