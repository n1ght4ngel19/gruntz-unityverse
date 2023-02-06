using System.Collections.Generic;
using System.Linq;
using Enumz;
using UnityEngine;

namespace GruntzUnityverse.AnimationPackz {
  public class GruntAnimationPack {
    public List<Sprite> Death;

    public List<Sprite> AttackEast;
    public List<Sprite> AttackNorth;
    public List<Sprite> AttackNorthEast;
    public List<Sprite> AttackNorthWest;
    public List<Sprite> AttackSouth;
    public List<Sprite> AttackSouthEast;
    public List<Sprite> AttackSouthWest;
    public List<Sprite> AttackWest;

    public List<Sprite> IdleEast;
    public List<Sprite> IdleNorth;
    public List<Sprite> IdleNorthEast;
    public List<Sprite> IdleNorthWest;
    public List<Sprite> IdleSouth;
    public List<Sprite> IdleSouthEast;
    public List<Sprite> IdleSouthWest;
    public List<Sprite> IdleWest;

    public List<Sprite> ItemEast;
    public List<Sprite> ItemNorth;
    public List<Sprite> ItemNorthEast;
    public List<Sprite> ItemNorthWest;
    public List<Sprite> ItemSouth;
    public List<Sprite> ItemSouthEast;
    public List<Sprite> ItemSouthWest;
    public List<Sprite> ItemWest;

    public List<Sprite> StruckEast;
    public List<Sprite> StruckNorth;
    public List<Sprite> StruckNorthEast;
    public List<Sprite> StruckNorthWest;
    public List<Sprite> StruckSouth;
    public List<Sprite> StruckSouthEast;
    public List<Sprite> StruckSouthWest;
    public List<Sprite> StruckWest;

    public List<Sprite> WalkEast;
    public List<Sprite> WalkNorth;
    public List<Sprite> WalkNorthEast;
    public List<Sprite> WalkNorthWest;
    public List<Sprite> WalkSouth;
    public List<Sprite> WalkSouthEast;
    public List<Sprite> WalkSouthWest;
    public List<Sprite> WalkWest;

    public GruntAnimationPack(ToolType toolType) {
      Death = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/{toolType}GruntDeath").ToList();

      AttackEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntEastAttack").ToList();
      AttackNorth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntNorthAttack").ToList();
      AttackNorthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntNorthEastAttack").ToList();
      AttackNorthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntNorthWestAttack").ToList();
      AttackSouth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntSouthAttack").ToList();
      AttackSouthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntSouthEastAttack").ToList();
      AttackSouthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntSouthWestAttack").ToList();
      AttackWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Attack/{toolType}GruntWestAttack").ToList();

      IdleEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntEastIdle").ToList();
      IdleNorth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntNorthIdle").ToList();
      IdleNorthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntNorthEastIdle").ToList();
      IdleNorthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntNorthWestIdle").ToList();
      IdleSouth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntSouthIdle").ToList();
      IdleSouthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntSouthEastIdle").ToList();
      IdleSouthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntSouthWestIdle").ToList();
      IdleWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Idle/{toolType}GruntWestIdle").ToList();

      ItemEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntEastItem").ToList();
      ItemNorth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntNorthItem").ToList();
      ItemNorthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntNorthEastItem").ToList();
      ItemNorthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntNorthWestItem").ToList();
      ItemSouth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntSouthItem").ToList();
      ItemSouthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntSouthEastItem").ToList();
      ItemSouthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntSouthWestItem").ToList();
      ItemWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Item/{toolType}GruntWestItem").ToList();

      StruckEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntEastStruck").ToList();
      StruckNorth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntNorthStruck").ToList();
      StruckNorthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntNorthEastStruck").ToList();
      StruckNorthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntNorthWestStruck").ToList();
      StruckSouth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntSouthStruck").ToList();
      StruckSouthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntSouthEastStruck").ToList();
      StruckSouthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntSouthWestStruck").ToList();
      StruckWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Struck/{toolType}GruntWestStruck").ToList();

      WalkEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntEastWalk").ToList();
      WalkNorth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntNorthWalk").ToList();
      WalkNorthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntNorthEastWalk").ToList();
      WalkNorthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntNorthWestWalk").ToList();
      WalkSouth = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntSouthWalk").ToList();
      WalkSouthEast = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntSouthEastWalk").ToList();
      WalkSouthWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntSouthWestWalk").ToList();
      WalkWest = Resources.LoadAll<Sprite>($"Animated Sprites/Gruntz/{toolType}Grunt/Walk/{toolType}GruntWestWalk").ToList();
    }
  }
}
