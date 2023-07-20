using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Shovel : Tool {
    public override IEnumerator Use(Grunt grunt) {
      Vector2Int diffVector = grunt.targetObject.location - grunt.navigator.ownLocation;
      grunt.isInterrupted = true;

      grunt.navigator.ChangeFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        // Todo: Replace with right animation
        grunt.AnimationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.facingDirection}"];

      grunt.animancer.Play(clipToPlay);

      StartCoroutine(grunt.targetObject.BeUsed(grunt));

      yield return new WaitForSeconds(1.6f);

      grunt.isInterrupted = false;
    }
  }
}
