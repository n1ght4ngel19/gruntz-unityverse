using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public abstract class Tool : Item {
    public ToolName toolName;
    public Range range;
    public DeathName deathInflicted;
    [Range(0, 40)] public int damage;
    [Range(0, 20)] public int damageReduction;
    public float itemUseContactDelay;
    public float attackContactDelay;
    public AudioClip useSound;
    public string gruntType;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      category = nameof(Tool);

      gruntType = $"{toolName}Grunt";
      string path = $"Assets/Audio/Soundz/Gruntz/{gruntType}/Sound_{gruntType}_UseItem.wav";
      Addressables.LoadAssetAsync<AudioClip>(path).Completed += handle => {
        useSound = handle.Result;
      };
    }
    // -------------------------------------------------------------------------------- //

    public IEnumerator Attack(Grunt attackTarget) {
      Vector2Int diffVector = attackTarget.navigator.ownLocation - ownGrunt.navigator.ownLocation;
      ownGrunt.navigator.FaceTowards(new Vector3(diffVector.x, diffVector.y, 0));

      string attackIndex = $"0{Random.Range(1, 3)}";
      AnimationClip clipToPlay = ownGrunt.animationPack.Attack[$"{gruntType}_Attack_{ownGrunt.navigator.facingDirection}_{attackIndex}"];

      ownGrunt.audioSource.PlayOneShot(useSound);
      ownGrunt.animancer.Play(clipToPlay);

      yield return new WaitForSeconds(attackContactDelay);

      ownGrunt.state = GruntState.AttackingIdle;
      ownGrunt.isInterrupted = false;
      attackTarget.TakeDamage(ownGrunt.equipment.tool.damage, attackTarget.equipment.tool.damageReduction);
      attackTarget.deathToDie = deathInflicted;

      StartCoroutine(attackTarget.GetHitBy(ownGrunt));
    }

    public virtual IEnumerator UseTool() {
      yield return null;
    }
  }
}
