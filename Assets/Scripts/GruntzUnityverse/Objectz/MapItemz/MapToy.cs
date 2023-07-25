using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapToy : MapItem {
    [field: SerializeField] public AnimatorOverrideController OverrideController { get; set; }
    [field: SerializeField] public Toy PickupToy { get; set; }


    protected override void Start() {
      base.Start();

      PickupToy = gameObject.GetComponent<Toy>();
      RotationAnimation = Resources.Load<AnimationClip>($"Animationz/MapItemz/Toy/Clipz/{PickupToy.GetType().Name}_Rotating");
      Animancer.Play(RotationAnimation);
    }
  }
}
