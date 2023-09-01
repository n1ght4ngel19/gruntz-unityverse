using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public abstract class Tool : Item {
    public ToolName toolName;
    public RangeType rangeType;
    public DeathName deathInflicted;
    [Range(0, 40)] public int damage;
    [Range(0, 20)] public int damageReduction;
    public float contactDelay;
    public float attackContactDelay;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      category = nameof(Tool);
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator Use(Grunt grunt) {
      // Not applicable
      yield return null;
    }

    public virtual IEnumerator Attack(Grunt attackTarget) {
      Vector2Int diffVector = attackTarget.navigator.ownLocation - ownGrunt.navigator.ownLocation;
      ownGrunt.isInterrupted = true;

      ownGrunt.navigator.SetFacingDirection(new Vector3(diffVector.x, diffVector.y, 0));

      string gruntType = $"{GetType().Name}Grunt";
      string attackIndex = $"0{Random.Range(1, 3)}";
      AnimationClip clipToPlay =
        ownGrunt.animationPack.Attack[
          $"{gruntType}_Attack_{ownGrunt.navigator.facingDirection}_{attackIndex}"];

      ownGrunt.animancer.Play(clipToPlay);

      yield return new WaitForSeconds(attackContactDelay);

      attackTarget.TakeDamage(ownGrunt.equipment.tool.damage, attackTarget.equipment.tool.damageReduction);
      attackTarget.deathToDie = deathInflicted;

      if (attackTarget.health > 0) {
        ownGrunt.gruntState = GruntState.Hostile;
        ownGrunt.isInterrupted = false;
      }
    }

    public virtual IEnumerator UseItem() {
      yield return null;
    }
  }
}
