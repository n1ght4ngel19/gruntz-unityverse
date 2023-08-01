using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Club : Tool {
    public override IEnumerator Use(Grunt grunt) {
      yield return null;
    }

    public override IEnumerator Attack(Grunt grunt) {
      Vector2Int diffVector = grunt.targetObject.location - grunt.navigator.ownLocation;
      grunt.isInterrupted = true;

      grunt.navigator.ChangeFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        grunt.AnimationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.facingDirection}"];

      grunt.animancer.Play(clipToPlay);

      // Wait the amount of time it takes for the tool to get in contact with the target Grunt
      yield return new WaitForSeconds(1f);

      StartCoroutine(grunt.GetStruck());
    }
  }
}
