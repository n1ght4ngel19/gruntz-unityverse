using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
  public class Gauntletz : Tool {
    protected override void Start() {
      toolName = ToolName.Gauntletz;
      range = Range.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.GauntletzDamage;
      mapItemName = nameof(Gauntletz);
      itemUseContactDelay = 0.75f;
      attackContactDelay = 0.4f;

      base.Start();
    }

    // Todo: ? Refactor this to be used like the Attack(Grunt attackTarget) method.
    public override IEnumerator UseTool() {
      Vector2Int diffVector = ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation;
      ownGrunt.navigator.FaceTowards(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        ownGrunt.animationPack.Item[$"{gruntType}_Item_{ownGrunt.navigator.facingDirection}"];

      ownGrunt.audioSource.PlayOneShot(useSound);
      ownGrunt.animancer.Play(clipToPlay);

      StartCoroutine(ownGrunt.targetMapObject is GiantRockEdge
        ? ((GiantRockEdge)ownGrunt.targetMapObject).mainRock.Break(itemUseContactDelay)
        : ((IBreakable)ownGrunt.targetMapObject).Break(itemUseContactDelay));

      yield return new WaitForSeconds(1f);

      ownGrunt.CleanState();
    }
  }
}
