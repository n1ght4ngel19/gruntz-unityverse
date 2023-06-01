using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.AnimationPackz {
  public class GruntAnimationPack {
    public readonly Dictionary<string, AnimationClip> Attack;
    public readonly Dictionary<string, AnimationClip> Death;
    public readonly Dictionary<string, AnimationClip> Idle;
    public readonly Dictionary<string, AnimationClip> Item;
    public readonly Dictionary<string, AnimationClip> Struck;
    public readonly Dictionary<string, AnimationClip> Walk;

    public GruntAnimationPack(ToolType tool) {
      Attack = new Dictionary<string, AnimationClip>();
      Death = new Dictionary<string, AnimationClip>();
      Idle = new Dictionary<string, AnimationClip>();
      Item = new Dictionary<string, AnimationClip>();
      Struck = new Dictionary<string, AnimationClip>();
      Walk = new Dictionary<string, AnimationClip>();

      List<AnimationClip> attackClips = Resources.LoadAll<AnimationClip>($"NewAnimationz/{tool}Grunt/Attack/Clipz/")
        .ToList();

      List<AnimationClip> deathClips = Resources.LoadAll<AnimationClip>($"NewAnimationz/{tool}Grunt/Death/Clipz/")
        .ToList();

      List<AnimationClip> idleClips = Resources.LoadAll<AnimationClip>($"NewAnimationz/{tool}Grunt/Idle/Clipz/")
        .ToList();

      List<AnimationClip> itemClips = Resources.LoadAll<AnimationClip>($"NewAnimationz/{tool}Grunt/Item/Clipz/")
        .ToList();

      List<AnimationClip> struckClips = Resources.LoadAll<AnimationClip>($"NewAnimationz/{tool}Grunt/Struck/Clipz/")
        .ToList();

      List<AnimationClip> walkClips = Resources.LoadAll<AnimationClip>($"NewAnimationz/{tool}Grunt/Walk/Clipz/")
        .ToList();

      foreach (AnimationClip clip in attackClips) {
        Attack.Add(clip.name, clip);
      }

      foreach (AnimationClip clip in deathClips) {
        Death.Add(clip.name, clip);
      }

      foreach (AnimationClip clip in idleClips) {
        Idle.Add(clip.name, clip);
      }

      foreach (AnimationClip clip in itemClips) {
        Item.Add(clip.name, clip);
      }

      foreach (AnimationClip clip in struckClips) {
        Struck.Add(clip.name, clip);
      }

      foreach (AnimationClip clip in walkClips) {
        Walk.Add(clip.name, clip);
      }
    }
  }
}
