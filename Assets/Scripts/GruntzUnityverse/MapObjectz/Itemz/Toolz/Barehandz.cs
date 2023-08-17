using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Barehandz : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Barehandz;
      rangeType = RangeType.Melee;
      damage = GlobalValuez.BarehandzDamage;
    }

    public override IEnumerator Attack(Grunt attackTarget) {
      Debug.Log("Attacking target!");
      Vector2Int diffVector = attackTarget.navigator.ownLocation - ownGrunt.navigator.ownLocation;
      ownGrunt.isInterrupted = true;

      ownGrunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      string gruntType = $"{toolName}Grunt";
      string attackIndex = $"0{Random.Range(1, 3)}";

      Addressables.LoadAssetAsync<AnimationClip>(
          $"{GlobalNamez.GruntAnimzPath}{gruntType}/{GlobalNamez.AttackAnimzSubPath}{gruntType}_Attack_{ownGrunt.navigator.facingDirection}_{attackIndex}.anim")
        .Completed += handle => {
        ownGrunt.animancer.Play(handle.Result);
      };

      yield return new WaitForSeconds(0.5f);

      attackTarget.TakeDamage(ownGrunt.equipment.tool.damage, attackTarget.equipment.tool.damageReduction);

      if (attackTarget.health <= 0) {
        ownGrunt.CleanState();
      } else {
        ownGrunt.gruntState = GruntState.Hostile;
        ownGrunt.isInterrupted = false;
      }
    }

    public override IEnumerator UseItem() {
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

    public override IEnumerator Use(Grunt grunt) {
      // Not applicable
      yield return null;
    }
  }
}
