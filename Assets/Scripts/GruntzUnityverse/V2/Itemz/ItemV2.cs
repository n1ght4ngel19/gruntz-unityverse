using System.Collections;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Objectz;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz {
  public abstract class ItemV2 : GridObject, IAnimatable {
    /// <summary>
    /// The name of the item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// The animation clip this item uses when rotating.
    /// </summary>
    public AnimationClip rotatingAnim;

    /// <summary>
    /// The animation clip the Grunt uses that picks up this item.
    /// </summary>
    public AnimationClip pickupAnim;

    /// <summary>
    /// The animation pack this item uses on the Grunt that picks it up.
    /// </summary>
    public GruntAnimationPack animationPack;

    public override void Setup() {
      base.Setup();

      Animator = GetComponent<Animator>();
      Animancer = GetComponent<AnimancerComponent>();
    }

    // --------------------------------------------------
    // IAnimatable
    // --------------------------------------------------

    #region IAnimatable
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public AnimancerComponent Animancer { get; set; }
    #endregion

    protected override void Start() {
      if (GetComponent<GruntV2>() != null) {
        return;
      }

      base.Start();

      Animancer.Play(rotatingAnim);
    }

    /// <summary>
    /// Called when a <see cref="GruntV2"/> picks up this item.
    /// (Provides no implementation since child classes need to modify
    /// different properties of the Grunt picking up the item.)
    /// </summary>
    protected virtual IEnumerator Pickup(GruntV2 target) {
      target.flagz.interrupted = true;

      target.Animancer.Play(pickupAnim);

      // All pickup animationz are 1 second long
      yield return new WaitForSeconds(1f);

      target.flagz.interrupted = false;
    }

    /// <summary>
    /// Called when an <see cref="GruntV2"/> moves onto this Item.
    /// Other than RollingBallz, only Gruntz have the ability to collide with Items.
    /// This is checked inside the method, so there is no need to expose this method to child classes.
    /// </summary>
    /// <param name="other">The collider of the colliding object.</param>
    protected virtual IEnumerator OnTriggerEnter2D(Collider2D other) {
      GruntV2 grunt = other.gameObject.GetComponent<GruntV2>();

      if (grunt == null) {
        yield break;
      }

      Animancer.Stop();
      spriteRenderer.enabled = false;

      yield return Pickup(grunt);

      Destroy(gameObject);
    }

    // protected override void OnDestroy() {
    //   if (!SceneLoaded()) {
    //     return;
    //   }
    //
    //   base.OnDestroy();
    // }
  }
}
