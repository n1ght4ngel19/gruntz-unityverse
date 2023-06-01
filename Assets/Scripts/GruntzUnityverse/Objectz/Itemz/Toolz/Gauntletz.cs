using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Gauntletz : Tool {
    public override IEnumerator Use(Grunt grunt) {
      Vector2Int diffVector = grunt.TargetObject.OwnLocation - grunt.Navigator.OwnLocation;
      grunt.IsInterrupted = true;

      grunt.Navigator.ChangeFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        grunt.AnimationPack.Item[$"{grunt.Equipment.Tool.Type}Grunt_Item_{grunt.Navigator.FacingDirection}"];

      grunt._Animancer.Play(clipToPlay);

      StartCoroutine(grunt.TargetObject.BeUsed());

      yield return new WaitForSeconds(2f);

      grunt.IsInterrupted = false;

      if (grunt.TargetObject is null) {
        yield break;
      }

      grunt.TargetObject = null;
    }
  }
}
