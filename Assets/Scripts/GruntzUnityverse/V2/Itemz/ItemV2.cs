using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Objectz;
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
    /// The animation pack this item uses on the Grunt that picks it up.
    /// </summary>
    public GruntAnimationPack animationPack;
    
    #region IAnimatable
    // --------------------------------------------------
    // IAnimatable
    // --------------------------------------------------
    [field: SerializeField]
    public Animator Animator { get; set; }

    [field: SerializeField]
    public AnimancerComponent Animancer { get; set; }
    #endregion

    protected override void Awake() {
      base.Awake();

      Animancer.Play(rotatingAnim);
    }

    /// <summary>
    /// Called when a <see cref="GruntV2"/> picks up this item.
    /// (Provides no implementation since child classes need to modify
    /// different properties of the Grunt picking up the item.)
    /// </summary>
    protected abstract void Pickup(GruntV2 grunt);

    /// <summary>
    /// Called when an <see cref="GruntV2"/> moves onto this Item.
    /// Other than RollingBallz, only Gruntz have the ability to collide with Items.
    /// This is checked inside the method, so there is no need to expose this method to child classes.
    /// </summary>
    /// <param name="other">The collider of the colliding object.</param>
    private void OnCollisionEnter2D(Collision2D other) {
      GruntV2 grunt = other.gameObject.GetComponent<GruntV2>();

      if (grunt != null) {
        Pickup(grunt);
      }
    }
  }
}
