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
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    #region Stats
    [Header("Stats")]
    public int gruntId;
    public Owner owner;
    public float moveSpeed;
    public int health;
    public int stamina;
    public int powerupTime;
    public int toyTime;
    public int wingzTime;
    #endregion

    #region Flags
    [Header("Flags")]
    public bool isSelected;
    public bool isInCircle;
    public bool haveActionCommand;
    public bool haveGiveToyCommand;
    public bool canInteract;
    public bool isInterrupted;
    private bool _isDying;
    #endregion

    #region Action
    [Header("Action")]
    public MapObject targetObject;
    [CanBeNull] public Grunt targetGrunt;
    [CanBeNull] public MapObject targetMapObject;
    public InteractionType interactionType;
    #endregion

    #region Components
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Equipment equipment;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public AnimancerComponent animancer;
    private Animator _animator;
    #endregion

    #region Other
    public GruntAnimationPack animationPack;
    #endregion


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
      // ----------------------------------------

      // Action
      if (haveActionCommand && !isInterrupted) {
        // Giving or placing a Toy
        if (haveGiveToyCommand) {
          canInteract = targetGrunt is not null
            ? IsNeighbourOf(targetGrunt)
            : IsNeighbourOf(navigator.targetNode);

          interactionType = InteractionType.GiveToy;
          // Attacking a Grunt or using a Tool
        } else if (equipment.tool.rangeType == RangeType.Melee) {
          canInteract = targetGrunt is not null
            ? IsNeighbourOf(targetGrunt)
            : IsNeighbourOf(targetMapObject);

          interactionType = targetGrunt is not null
            ? InteractionType.Attack
            : InteractionType.Use;
        }

        navigator.haveMoveCommand = !canInteract;
      }
      // ----------------------------------------

      // Interaction (Attack / Item use / Toy use)
      if (canInteract && !isInterrupted) { // Todo: Add stamina condition
        switch (interactionType) {
          case InteractionType.None:
            break;
          case InteractionType.Attack:
            StartCoroutine(HandleAttack());

            break;
          case InteractionType.Use:
            StartCoroutine(HandleItemUse());

            break;
          case InteractionType.GiveToy:
            // Give toy
            break;
          default:
            break;
        }
      }
      // ----------------------------------------

      // Death
      // Todo
      // ----------------------------------------

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

    private IEnumerator HandleItemUse() {
      // Item
      switch (equipment.tool) {
        // Todo: All valid item use conditions
        case Gauntletz when targetMapObject is IBreakable:
          StartCoroutine(equipment.tool.UseItem());

          yield return new WaitForSeconds(1.5f);
          break;
        case Shovel when targetMapObject is Hole:
          StartCoroutine(equipment.tool.UseItem());

          yield return new WaitForSeconds(1.5f);
          break;
        default:
          Debug.Log("Say: Can't do it ");
          CleanState();
          break;
      }
    }

    private IEnumerator HandleAttack() {
      // Attack
      StartCoroutine(equipment.tool.UseItem()); // Only Barehandz for now

      yield return new WaitForSeconds(1.5f);
    }

    /// <summary>
    /// Checks if the Grunt is a valid target for another Grunt.
    /// </summary>
    /// <param name="grunt">The other Grunt.</param>
    /// <returns>True if the Grunt is a valid target, false otherwise.</returns>
    public bool IsValidTargetFor(Grunt grunt) {
      return grunt != this && grunt.owner != owner;
    }

    /// <summary>
    /// Resets all the Grunt's states and commands to default.
    /// </summary>
    public void CleanState() {
      haveActionCommand = false;
      haveGiveToyCommand = false;
      canInteract = false;
      isInterrupted = false;
      targetGrunt = null;
      targetMapObject = null;
      interactionType = InteractionType.None;
    }

    /// <summary>
    /// Checks if the Grunt is at the given Node.
    /// </summary>
    /// <param name="node">The Node in question.</param>
    /// <returns>True if the Grunt is at the Node, false otherwise.</returns>
    public bool AtNode(Node node) {
      return navigator.ownNode == node;
    }

    /// <summary>
    /// Checks if the Grunt is beside the given object (Arrow, Rock, etc.).
    /// </summary>
    /// <param name="target">The object to check.</param>
    /// <returns>True if the Grunt is beside the object, false otherwise.</returns>
    public bool IsNeighbourOf(MapObject target) {
      return navigator.ownNode.Neighbours.Contains(target.ownNode);
    }

    /// <summary>
    /// Checks if the Grunt is beside another given Grunt.
    /// </summary>
    /// <param name="target">The Grunt to check.</param>
    /// <returns>True if the Grunt is beside the other Grunt, false otherwise.</returns>
    public bool IsNeighbourOf(Grunt target) {
      return navigator.ownNode.Neighbours.Contains(target.navigator.ownNode);
    }

    /// <summary>
    /// Checks if the Grunt is beside the given Node. 
    /// </summary>
    /// <param name="target">The Node to check.</param>
    /// <returns>True if the Grunt is beside the Node, false otherwise.</returns>
    public bool IsNeighbourOf(Node target) {
      return navigator.ownNode.Neighbours.Contains(target);
    }

    /// <summary>
    /// Checks if the Grunt has the given Tool equipped.
    /// </summary>
    /// <param name="tool">The Tool to check.</param>
    /// <returns>True if the Grunt has the Tool, false otherwise.</returns>
    public bool HasTool(ToolName tool) {
      return equipment.tool is not null && equipment.tool.toolName.Equals(tool);
    }

    /// <summary>
    /// Checks if the Grunt has the given Toy equipped.
    /// </summary>
    /// <param name="toy">The Toy to check.</param>
    /// <returns>True if the Grunt has the Toy, false otherwise.</returns>
    public bool HasToy(ToyName toy) {
      return equipment.toy is not null && equipment.toy.Name.Equals(toy);
    }

    /// <summary>
    /// Plays the appropriate animation given the Grunt's current state.
    /// </summary>
    private void PlayWalkOrIdleAnimation() {
      if (navigator.isMoving) {
        animancer.Play(animationPack.Walk[$"{equipment.tool.toolName}Grunt_Walk_{navigator.facingDirection}"]);
      } else {
        animancer.Play(animationPack.Idle[$"{equipment.tool.toolName}Grunt_Idle_{navigator.facingDirection}_01"]);
      }
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

          animancer.Play(AnimationManager.Instance.PickupPack.tool[itemName]);
          // Todo: Play pickup sound

          yield return new WaitForSeconds(0.8f);

          SetAnimPack(itemName);

          break;
        case nameof(Toy):
          Destroy(GetComponents<Toy>().FirstOrDefault());
          //equipment.toy = gameObject.AddComponent<Beachball>();

          animancer.Play(AnimationManager.Instance.PickupPack.toy[itemName]);

          break;
      }

      isInterrupted = true;

      // Wait the time it takes to pick up an item (subject to change)
      yield return new WaitForSeconds(0.8f);

      isInterrupted = false;
    }

    public IEnumerator PickupMiscItem(string itemName) {
      animancer.Play(AnimationManager.Instance.PickupPack.misc[itemName]);

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
      } else {
        navigator.SetTargetBesideNode(targetObject.ownNode);
      }
    }

    public IEnumerator GetStruck() {
      AnimationClip struckClip =
        animationPack.Struck[$"{equipment.tool.toolName}Grunt_Struck_{navigator.facingDirection}"];

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

      navigator.ownLocation = Vector2Direction.max;
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

      AnimationClip deathClip =
        animationPack.Death[$"{equipment.tool.GetType().Name}Grunt_Death_01"];

      animancer.Play(deathClip);

      yield return new WaitForSeconds(deathClip.length);

      navigator.ownLocation = Vector2Direction.max;
      LevelManager.Instance.playerGruntz.Remove(this);
      LevelManager.Instance.allGruntz.Remove(this);
      Destroy(gameObject, deathClip.length);
    }

    public void SetAnimPack(ToolName tool) {
      animationPack = tool switch {
        ToolName.Barehandz => AnimationManager.Instance.BarehandzGruntPack,
        ToolName.Gauntletz => AnimationManager.Instance.GauntletzGruntPack,
        ToolName.Shovel => AnimationManager.Instance.ShovelGruntPack,
        _ => AnimationManager.Instance.GauntletzGruntPack,
      };
    }

    public void SetAnimPack(string tool) {
      animationPack = tool switch {
        nameof(Barehandz) => AnimationManager.Instance.BarehandzGruntPack,
        nameof(Gauntletz) => AnimationManager.Instance.GauntletzGruntPack,
        nameof(Shovel) => AnimationManager.Instance.ShovelGruntPack,
        //nameof(Warpstone) => AnimationManager.Instance.WarpstonePack,
        _ => AnimationManager.Instance.BarehandzGruntPack,
      };
    }
  }
}
