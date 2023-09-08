using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Barehandz : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Barehandz;
      toolRange = RangeType.Melee;
      damage = GlobalValuez.BarehandzDamage;
      mapItemName = nameof(Barehandz);
      // itemUseContactDelay = Not applicable;
      attackContactDelay = 0.5f;
    }
    // -------------------------------------------------------------------------------- //

    // Todo: Why does this exist?
    public override IEnumerator UseTool() {
      Vector2Int diffVector = ownGrunt.targetGrunt is null
        ? ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation
        : ownGrunt.targetGrunt.navigator.ownLocation - ownGrunt.navigator.ownLocation;

      ownGrunt.isInterrupted = true;
      ownGrunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      // Todo: Proper length
      yield return new WaitForSeconds(1f);

      // Todo: Maybe not
      ownGrunt.CleanState();

      if (ownGrunt.targetMapObject is not null) {
        ownGrunt.targetObject = null;
      }
    }
  }
}
