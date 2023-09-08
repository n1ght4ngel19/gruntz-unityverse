using System.Collections;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Shovel : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Shovel;
      toolRange = RangeType.Melee;
      damage = GlobalValuez.ShovelDamage;
      mapItemName = nameof(Shovel);
      itemUseContactDelay = 1.5f;
      attackContactDelay = 0.5f;
    }
    // -------------------------------------------------------------------------------- //

    // public override IEnumerator UseItem() {
    //   Vector2Int diffVector = grunt.targetObject.location - grunt.navigator.ownLocation;
    //   grunt.isInterrupted = true;
    //
    //   grunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));
    //
    //   AnimationClip clipToPlay =
    //     // Todo: Replace with right animation
    //     grunt.animationPack.Item[$"{GetType().Name}Grunt_Item_{grunt.navigator.facingDirection}"];
    //
    //   grunt.animancer.Play(clipToPlay);
    //
    //   StartCoroutine(grunt.targetObject.BeUsed(grunt));
    //
    //   yield return new WaitForSeconds(1.6f);
    //
    //   grunt.isInterrupted = false;
    // }

    public override IEnumerator UseTool() {
      // Todo: Implement
      yield return null;
    }
  }
}
