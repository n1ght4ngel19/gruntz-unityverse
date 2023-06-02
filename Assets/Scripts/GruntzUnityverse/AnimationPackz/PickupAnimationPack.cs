using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.AnimationPackz {
  public class PickupAnimationPack {
    public readonly Dictionary<string, AnimationClip> Misc;
    public readonly Dictionary<string, AnimationClip> Powerup;
    public readonly Dictionary<string, AnimationClip> Tool;
    public readonly Dictionary<string, AnimationClip> Toy;

    public PickupAnimationPack() {
      Misc = new Dictionary<string, AnimationClip>();
      Powerup = new Dictionary<string, AnimationClip>();
      Tool = new Dictionary<string, AnimationClip>();
      Toy = new Dictionary<string, AnimationClip>();

      Misc.Add("Coin", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Misc_Coin"));
      Misc.Add("Helpbox", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Misc_Helpbox"));
      Misc.Add("Megaphone", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Misc_Megaphone"));
      Misc.Add("WarpletterW", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Misc_Warpletter_W"));
      Misc.Add("WarpletterA", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Misc_Warpletter_A"));
      Misc.Add("WarpletterR", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Misc_Warpletter_R"));
      Misc.Add("WarpletterP", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Misc_Warpletter_P"));
      Tool.Add("Gauntletz", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Tool_Gauntletz"));
      Tool.Add("Shovel", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Tool_Shovel"));
      Toy.Add("Beachball", Resources.Load<AnimationClip>("Animationz/Gruntz/Pickupz/Clipz/Pickup_Toy_Beachball"));
    }
  }
}
