using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Shovel : Tool {
    public IEnumerator DigHole(Grunt grunt) {
      grunt.Animator.Play($"UseItem_{grunt.Navigator.FacingDirection}");
      grunt.IsInterrupted = true;

      yield return new WaitForSeconds(grunt.Animator.GetCurrentAnimatorStateInfo(0).length);

      grunt.IsInterrupted = false;

      StartCoroutine(((Hole)grunt.TargetObject).Dig());

      grunt.TargetObject = null;
    }
  }
}
