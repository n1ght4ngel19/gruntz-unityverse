using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
  public class GooberStraw : Tool {
    protected override void Start() {
      toolName = ToolName.GooberStraw;
      range = Range.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.GooberStrawDamage;
      mapItemName = nameof(GooberStraw);
      itemUseContactDelay = 0.5f;
      attackContactDelay = 0.4f;

      base.Start();
    }

    public override IEnumerator UseTool() {
      Vector2Int diffVector = ownGrunt.targetMapObject.location - ownGrunt.navigator.ownLocation;
      ownGrunt.navigator.FaceTowards(new Vector3(diffVector.x, diffVector.y, 0));

      AnimationClip clipToPlay =
        ownGrunt.animationPack.Item[$"{gruntType}_Item_{ownGrunt.navigator.facingDirection}"];
      
      ownGrunt.audioSource.PlayOneShot(useSound);
      ownGrunt.animancer.Play(clipToPlay);

      yield return new WaitForSeconds(0.5f);

      StartCoroutine(((GruntPuddle)ownGrunt.targetMapObject).BeUsed());

      yield return new WaitForSeconds(2.5f);

      ownGrunt.CleanState();
    }
  }
}
