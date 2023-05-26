using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Shovel : Tool {
    public IEnumerator DigHole(Grunt grunt) {
      Vector2Int diffVector = grunt.TargetObject.OwnLocation - grunt.Navigator.OwnLocation;
      grunt.IsInterrupted = true;

      grunt.Navigator.DetermineFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));
      grunt.Animator.Play($"UseItem_{grunt.Navigator.FacingDirection}");
      // ((Hole)grunt.TargetObject).OwnAnimator.Play("DirtFlying");

      yield return new WaitForSeconds(2.1f);

      grunt.IsInterrupted = false;

      ((Hole)grunt.TargetObject).IsOpen = !((Hole)grunt.TargetObject).IsOpen;

      ((Hole)grunt.TargetObject).Renderer.sprite = ((Hole)grunt.TargetObject).IsOpen
        ? ((Hole)grunt.TargetObject).OpenSprite
        : ((Hole)grunt.TargetObject).FilledSprite;

      if (grunt.TargetObject is null) {
        yield break;
      }

      // StartCoroutine(((Hole)grunt.TargetObject).Dig());

      grunt.TargetObject = null;
    }
  }
}
