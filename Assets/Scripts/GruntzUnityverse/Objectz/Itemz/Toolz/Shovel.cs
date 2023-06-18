using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Shovel : Tool {
    protected override void Start() {
      Name = ToolName.Shovel;
    }

    public override IEnumerator Use(Grunt grunt) {
      Vector2Int diffVector = grunt.TargetObject.Location - grunt.navigator.OwnLocation;
      grunt.IsInterrupted = true;

      grunt.navigator.ChangeFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        // Todo: Replace with right animation
        grunt.AnimationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.FacingDirection}"];

      grunt.animancer.Play(clipToPlay);

      StartCoroutine(grunt.TargetObject.BeUsed(grunt));

      yield return new WaitForSeconds(1.6f);

      grunt.IsInterrupted = false;

      if (grunt.TargetObject is null) {
        yield break;
      }
      // grunt.TargetObject = null;
    }
  }
}
