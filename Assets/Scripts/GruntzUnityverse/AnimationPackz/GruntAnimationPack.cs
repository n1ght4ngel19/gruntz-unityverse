using System.Collections.Generic;
using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.AnimationPackz {
  public class GruntAnimationPack {
    public readonly Dictionary<string, AnimationClip> Attack;
    public readonly Dictionary<string, AnimationClip> Death;
    public readonly Dictionary<string, AnimationClip> Idle;
    public readonly Dictionary<string, AnimationClip> Item;
    public readonly Dictionary<string, AnimationClip> Struck;
    public readonly Dictionary<string, AnimationClip> Walk;

    public GruntAnimationPack(ToolName tool) {
      Attack = new Dictionary<string, AnimationClip>();
      Death = new Dictionary<string, AnimationClip>();
      Idle = new Dictionary<string, AnimationClip>();
      Item = new Dictionary<string, AnimationClip>();
      Struck = new Dictionary<string, AnimationClip>();
      Walk = new Dictionary<string, AnimationClip>();

      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_East_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_East_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_East_Idle.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_North_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_North_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_North_Idle.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northeast_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northeast_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northeast_Idle.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northwest_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northwest_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northwest_Idle.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_South_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_South_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_South_Idle.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southeast_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southeast_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southeast_Idle.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southwest_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southwest_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southwest_Idle.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_West_01.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_West_02.anim");
      LoadAddressableClipInto(Attack, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_West_Idle.anim");

      LoadAddressableClipInto(Death, $"Assets/Animationz/Gruntz/{tool}Grunt/Death/Clipz/{tool}Grunt_Death.anim");

      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_East_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_East_02.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_North_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_North_02.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_Northeast_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_Northwest_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_South_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_South_02.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_Southeast_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_Southwest_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_West_01.anim");
      LoadAddressableClipInto(Idle, $"Assets/Animationz/Gruntz/{tool}Grunt/Idle/Clipz/{tool}Grunt_Idle_West_02.anim");

      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_East.anim");
      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_North.anim");
      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_Northeast.anim");
      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_Northwest.anim");
      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_South.anim");
      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_Southeast.anim");
      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_Southwest.anim");
      LoadAddressableClipInto(Item, $"Assets/Animationz/Gruntz/{tool}Grunt/Item/Clipz/{tool}Grunt_Item_West.anim");

      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_East_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_East_02.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_North_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_North_02.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Northeast_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Northeast_02.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Northwest_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Northwest_02.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_South_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_South_02.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Southeast_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Southeast_02.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Southwest_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_Southwest_02.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_West_01.anim");
      LoadAddressableClipInto(Struck, $"Assets/Animationz/Gruntz/{tool}Grunt/Struck/Clipz/{tool}Grunt_Struck_West_02.anim");

      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_East.anim");
      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_North.anim");
      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_Northeast.anim");
      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_Northwest.anim");
      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_South.anim");
      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_Southeast.anim");
      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_Southwest.anim");
      LoadAddressableClipInto(Walk, $"Assets/Animationz/Gruntz/{tool}Grunt/Walk/Clipz/{tool}Grunt_Walk_West.anim");
    }

    public static void LoadAddressableClipInto(Dictionary<string, AnimationClip> dictionary, string name) {
      Addressables.LoadAssetAsync<AnimationClip>(name).Completed += handle => {
        dictionary.Add(handle.Result.name, handle.Result);
      };
    }
  }
}
