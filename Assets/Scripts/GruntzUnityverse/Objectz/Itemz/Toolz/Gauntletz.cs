using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Gauntletz : ItemTool {
    private void Start() { Type = ToolType.Gauntletz; }


    public IEnumerator BreakRock(Grunt grunt) {
      grunt.Animator.Play($"UseItem_{grunt.NavComponent.FacingDirection}");
      grunt.IsMovementInterrupted = true;

      // Todo: Wait for the exact time needed for breaking Rockz
      yield return new WaitForSeconds(0.5f);

      grunt.IsMovementInterrupted = false;

      StartCoroutine(((Rock)grunt.TargetObject).Break());
      grunt.TargetObject = null;
    }
  }
}
