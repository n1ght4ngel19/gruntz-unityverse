using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Gauntletz : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Gauntletz;
      toolRange = RangeType.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.GauntletzDamage;
      mapItemName = nameof(Gauntletz);
      itemUseContactDelay = 1.5f;
      attackContactDelay = 0.5f;
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator UseTool() {
      Vector2Int diffVector = ownGrunt.targetGrunt is null
        ? ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation
        : ownGrunt.targetGrunt.navigator.ownLocation - ownGrunt.navigator.ownLocation;

      ownGrunt.isInterrupted = true;

      ownGrunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      string gruntType = $"{toolName}Grunt";
      AnimationClip clipToPlay =
        ownGrunt.animationPack.Item[$"{gruntType}_Item_{ownGrunt.navigator.facingDirection}"];

      ownGrunt.animancer.Play(clipToPlay);

      StartCoroutine(ownGrunt.targetMapObject is GiantRockEdge
        ? ((GiantRockEdge)ownGrunt.targetMapObject).mainRock.Break(itemUseContactDelay)
        : ((IBreakable)ownGrunt.targetMapObject).Break(itemUseContactDelay));

      yield return new WaitForSeconds(2f);

      ownGrunt.CleanState();
    }
  }
}
