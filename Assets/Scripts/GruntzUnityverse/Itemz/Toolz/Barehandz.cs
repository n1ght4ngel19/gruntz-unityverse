﻿using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
  public class Barehandz : Tool {
    protected override void Start() {
      toolName = ToolName.Barehandz;
      range = Range.Melee;
      damage = GlobalValuez.BarehandzDamage;
      mapItemName = nameof(Barehandz);
      // itemUseContactDelay = Not applicable;
      attackContactDelay = 0.5f;

      base.Start();
    }
    // -------------------------------------------------------------------------------- //

    // Todo: Why does this exist?
    public override IEnumerator UseTool() {
      Vector2Int diffVector = ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation;

      ownGrunt.isInterrupted = true;
      ownGrunt.navigator.FaceTowards(new Vector3(diffVector.x, diffVector.y, 0));

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