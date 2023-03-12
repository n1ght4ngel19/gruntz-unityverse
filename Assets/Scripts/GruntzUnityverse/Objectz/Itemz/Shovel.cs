using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Shovel : ItemTool {
    private void Start() { Type = ToolType.Shovel; }
    
    public  IEnumerator DigHole(Grunt grunt) {
      grunt.Animator.Play($"UseItem_{grunt.NavComponent.FacingDirection}");
      grunt.IsMovementInterrupted = true;

      // Todo: Wait for the exact time needed for digging Holez
      yield return new WaitForSeconds(1);

      grunt.IsMovementInterrupted = false;

      StartCoroutine(((Hole)grunt.TargetObject).Dig());
      grunt.TargetObject = null;
    }
  }
}
