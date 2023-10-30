using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
  public class Shovel : Tool {
    protected override void Start() {
      toolName = ToolName.Shovel;
      range = Range.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.ShovelDamage;
      mapItemName = nameof(Shovel);
      itemUseContactDelay = 1.5f;
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

      StartCoroutine(((Hole)ownGrunt.targetMapObject).BeUsed());

      yield return new WaitForSeconds(2.5f);

      ownGrunt.CleanState();
    }
  }
}
