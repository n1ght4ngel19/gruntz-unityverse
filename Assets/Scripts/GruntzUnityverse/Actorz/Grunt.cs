using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Itemz;
using GruntzUnityverse.Objectz.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    public int gruntId;

    #region Stats

    [Header("Stats")] public Owner owner;
    public float moveSpeed;
    public int health;
    public int stamina;
    public int powerupTime;
    public int toyTime;
    public int wingzTime;

    #endregion

    #region Flags

    [Header("Flags")] public bool isSelected;
    public bool isInCircle;
    public bool isInterrupted;
    public bool haveActionCommand;
    public bool haveMoveCommand;
    public bool haveSavedTarget;
    private bool _isDying;

    #endregion

    public bool actAsPlayer;
    public Grunt targetGrunt;
    public MapObject targetMapObject;
    public Node targetNode;

    #region Components

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Equipment equipment;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public AnimancerComponent animancer;
    private Animator _animator;

    #endregion


    #region Action

    [Header("Action")] [CanBeNull] public MapObject targetObject;
    // [CanBeNull] public Grunt targetGrunt;

    #endregion

    public GruntAnimationPack AnimationPack;
    public Node clickedNode;


    private void Start() {
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      navigator = gameObject.AddComponent<Navigator>();
      equipment = gameObject.AddComponent<Equipment>();
      equipment.tool = gameObject.GetComponents<Tool>().FirstOrDefault();
      equipment.toy = gameObject.GetComponents<Toy>().FirstOrDefault();
      healthBar = gameObject.GetComponentInChildren<HealthBar>();
      health = 20; // Todo: Replace with constant
      _animator = gameObject.AddComponent<Animator>();
      animancer = gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = _animator;
      BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
      boxCollider.size = Vector2.one;
    }

    protected virtual void Update() {
      // Setting flags necessary on all frames
      healthBar.spriteRenderer.enabled = isSelected;
      isInCircle = SelectorCircle.Instance.ownNode == navigator.ownNode;

      // Movement
      if (navigator.haveMoveCommand) {
        navigator.isMoving = true;
        navigator.MoveTowardsTargetNode();
      }

      // Action
      if (haveActionCommand) {}


      // Handling action
      // Todo: Fix this
      // if (haveActionCommand && !isInterrupted) {
      //   if (owner == Owner.Player) {}
      //
      //   if (targetGrunt is not null) {
      //     navigator.SetTargetBesideNode(targetGrunt.navigator.ownNode);
      //   }
      //
      //   if (targetGrunt is null) {
      //     targetObject = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<MapObject>()
      //       .FirstOrDefault(o => o.ownNode == clickedNode);
      //   }
      //
      //   if (targetObject is null) {
      //     ResetActionCommand();
      //
      //     return;
      //   }
      //
      //   HandleItemUse();
      // }


      // Todo: Fix this
      // #region Death handling
      //
      // // Handling the case when Grunt is on a blocked Node
      // Node node = navigator.ownNode;
      //
      // // Handling death
      // if (node.isBurn) {
      //   StartCoroutine(Death("Burn"));
      // }
      //
      // if (node.isWater) {
      //   StartCoroutine(Death("Sink"));
      // }
      //
      // if (node.isBlocked && !node.isBurn && !node.isWater) {
      //   StartCoroutine(Death("Squash"));
      // }
      //
      // // Todo: Move to Hole script!!!
      // if (LevelManager.Instance.Holez.Any(hole => hole.location.Equals(navigator.ownLocation) && hole.IsOpen)) {
      //   StartCoroutine(Death("Hole"));
      // }
      //
      // #endregion

      PlayWalkOrIdleAnimation();
    }

    private void HandleItemUse() {
      if (equipment.tool is Gauntletz) {
        if (targetObject is IBreakable) {
          MoveOrDo();

          return;
        }
      }

      if (equipment.tool is Shovel) {
        if (targetObject is Hole) {
          MoveOrDo();

          return;
        }
      }

      Debug.Log("No can do");
      ResetActionCommand();
    }

    public void ResetActionCommand() {
      haveActionCommand = false;
      targetObject = null;
      clickedNode = null;
    }

    public bool AtLocation(Vector2Int location) {
      return navigator.ownLocation.Equals(location);
    }

    public bool IsNeighbourOf(MapObject target) {
      return navigator.ownNode.Neighbours.Contains(target.ownNode);
    }

    public bool IsNeighbourOf(Grunt target) {
      return navigator.ownNode.Neighbours.Contains(target.navigator.ownNode);
    }

    /// <summary>
    /// Decides whether the Grunt has a Tool equipped.
    /// </summary>
    /// <param name="tool">The Tool to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasTool(ToolName tool) {
      return equipment.tool is not null && equipment.tool.toolName.Equals(tool);
    }

    /// <summary>
    /// Decides whether the Grunt has a Toy equipped.
    /// </summary>
    /// <param name="toy">The Toy to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasToy(ToyName toy) {
      return equipment.toy is not null && equipment.toy.Name.Equals(toy);
    }

    private void PlayWalkOrIdleAnimation() {
      string walkOrIdle = navigator.isMoving ? "Walk" : "Idle";

      animancer.Play(AnimationPack.Walk[$"{equipment.tool.toolName}Grunt_{walkOrIdle}_{navigator.facingDirection}"]);
    }

    public IEnumerator PickupItem(Item item, string category, string itemName) {
      switch (category) {
        case nameof(Tool):
          Destroy(GetComponents<Tool>().FirstOrDefault());

          switch (itemName) {
            case nameof(Gauntletz):
              equipment.tool = gameObject.AddComponent<Gauntletz>();

              break;
            case nameof(Shovel):
              equipment.tool = gameObject.AddComponent<Shovel>();

              break;
            case nameof(Warpstone):
              equipment.tool = gameObject.AddComponent<Warpstone>();

              break;
          }

          animancer.Play(AnimationManager.Instance.PickupPack.Tool[itemName]);
          // Todo: Play pickup sound

          yield return new WaitForSeconds(0.8f);

          SetAnimPack(itemName);

          break;
        case nameof(Toy):
          Destroy(GetComponents<Toy>().FirstOrDefault());
          //equipment.toy = gameObject.AddComponent<Beachball>();

          animancer.Play(AnimationManager.Instance.PickupPack.Toy[itemName]);

          break;
      }

      isInterrupted = true;

      // Wait the time it takes to pick up an item (subject to change)
      yield return new WaitForSeconds(0.8f);

      isInterrupted = false;
    }

    public IEnumerator PickupMiscItem(string itemName) {
      animancer.Play(AnimationManager.Instance.PickupPack.Misc[itemName]);

      isInterrupted = true;

      // Wait the time it takes to pick up an item (subject to change)
      yield return new WaitForSeconds(0.8f);

      isInterrupted = false;
    }

    public void MoveOrDo() {
      if (IsNeighbourOf(targetObject)) {
        StartCoroutine(equipment.tool.Use(this));
        haveActionCommand = false;
        targetObject = null;
        clickedNode = null;
      } else {
        Debug.Log("Setting target closest to target");
        Debug.Log("Target's node is: " + targetObject.ownNode);
        navigator.SetTargetBesideNode(targetObject.ownNode);
      }
    }

    public IEnumerator GetStruck() {
      AnimationClip struckClip =
        AnimationPack.Struck[$"{equipment.tool.toolName}Grunt_Struck_{navigator.facingDirection}"];

      animancer.Play(struckClip);

      yield return new WaitForSeconds(struckClip.length);
    }

    public IEnumerator Death(string deathName) {
      if (!_isDying) {
        _isDying = true;
        transform.position += Vector3.forward * 15;
      }

      healthBar.spriteRenderer.enabled = false;
      // Todo: Stair attribute bars, and move into separate method
      enabled = false;
      navigator.enabled = false;
      isInterrupted = true;

      AnimationClip deathClip = AnimationManager.Instance.DeathPack[deathName];

      animancer.Play(deathClip);

      // Wait the time it takes to play the animation (based on the animation)
      yield return new WaitForSeconds(deathClip.length);

      navigator.ownLocation = Vector2IntExtra.Max();
      LevelManager.Instance.playerGruntz.Remove(this);
      LevelManager.Instance.allGruntz.Remove(this);
      Destroy(gameObject, deathClip.length);
    }

    public IEnumerator Death() {
      healthBar.spriteRenderer.enabled = false;
      // Todo: Stair attribute bars, and move into separate method
      enabled = false;
      navigator.enabled = false;
      isInterrupted = true;
      AnimationClip deathClip = AnimationPack.Death[$"{equipment.tool.GetType().Name}Grunt_Death_01"];

      animancer.Play(deathClip);

      yield return new WaitForSeconds(deathClip.length);

      navigator.ownLocation = Vector2IntExtra.Max();
      LevelManager.Instance.playerGruntz.Remove(this);
      LevelManager.Instance.allGruntz.Remove(this);
      Destroy(gameObject, deathClip.length);
    }

    public void SetAnimPack(ToolName tool) {
      AnimationPack = tool switch {
        ToolName.Barehandz => AnimationManager.Instance.BarehandzGruntPack,
        ToolName.Gauntletz => AnimationManager.Instance.GauntletzGruntPack,
        ToolName.Shovel => AnimationManager.Instance.ShovelGruntPack,
        _ => AnimationManager.Instance.GauntletzGruntPack,
      };
    }

    public void SetAnimPack(string tool) {
      AnimationPack = tool switch {
        nameof(Barehandz) => AnimationManager.Instance.BarehandzGruntPack,
        nameof(Gauntletz) => AnimationManager.Instance.GauntletzGruntPack,
        nameof(Shovel) => AnimationManager.Instance.ShovelGruntPack,
        //nameof(Warpstone) => AnimationManager.Instance.WarpstonePack,
        _ => AnimationManager.Instance.BarehandzGruntPack,
      };
    }
  }
}
