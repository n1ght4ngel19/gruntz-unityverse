using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Gauntletz : Tool {
    public IEnumerator BreakRock(Grunt grunt) {
      Vector2Int diffVector = grunt.TargetObject.OwnLocation - grunt.Navigator.OwnLocation;

      grunt.Navigator.DetermineFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));
      grunt.Animator.Play($"UseItem_{grunt.Navigator.FacingDirection}");

      grunt.IsInterrupted = true;

      yield return new WaitForSeconds(1f);

      grunt.IsInterrupted = false;

      if (grunt.TargetObject is null) {
        yield break;
      }

      StartCoroutine(((Rock)grunt.TargetObject).Break());

      grunt.TargetObject = null;
    }
  }
}
