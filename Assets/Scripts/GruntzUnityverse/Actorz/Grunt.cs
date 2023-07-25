using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Brickz;
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
    public bool isInterrupted;
    public bool haveActionCommand;
    private bool _isDying;

    #endregion

    #region Components

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Equipment equipment;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public AnimancerComponent animancer;
    private Animator _animator;

    #endregion

    [Header("Action")][CanBeNull] public MapObject targetObject;
    [CanBeNull] public Grunt targetGrunt;
    public GruntAnimationPack AnimationPack;
    private Node _clickedNode;


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
      boxCollider.size = new Vector2(1f, 1f);
    }

    private void Update() {
      healthBar.spriteRenderer.enabled = isSelected;
      bool leftCLick = Input.GetMouseButtonDown(0);
      bool rightClick = Input.GetMouseButtonDown(1);
      bool leftShiftDown = Input.GetKey(KeyCode.LeftShift);
      bool isInCircle = SelectorCircle.Instance.OwnNode == navigator.ownNode;

      // Selection Command
      if (isInCircle && leftCLick && !leftShiftDown) {
        SetSingleSelected();

        return;
      }

      // Move or Action command
      if (isSelected) {
        if (rightClick) {
          navigator.hasMoveCommand = true;

          return;
        }

        if (leftCLick && leftShiftDown) {
          haveActionCommand = true;
          _clickedNode = SelectorCircle.Instance.OwnNode;
        }
      }

      if (haveActionCommand && !isInterrupted) {
        targetGrunt = LevelManager.Instance.allGruntz.FirstOrDefault(
          grunt => grunt.navigator.ownNode.Equals(SelectorCircle.Instance.OwnNode)
        );

        if (targetGrunt is null) {
          targetObject = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<MapObject>()
            .FirstOrDefault(o => o.OwnNode == _clickedNode);
        }

        if (targetObject is null) {
          haveActionCommand = false;
          _clickedNode = null;

          return;
        }

        if (equipment.tool is Gauntletz) {
          if (targetObject is Rock or BrickContainer or GiantRockEdge) {
            MoveOrDo();
          } else {
            Debug.Log("No can do");
            haveActionCommand = false;
            targetObject = null;
            _clickedNode = null;
          }
        }

        if (equipment.tool is Shovel) {
          if (targetObject is Hole) {
            MoveOrDo();
          } else {
            Debug.Log("No can do");
            haveActionCommand = false;
            targetObject = null;
            _clickedNode = null;
          }
        }
      }

      // Setting move target
      if (navigator.hasMoveCommand && !AtLocation(SelectorCircle.Instance.location)) {
        navigator.hasMoveCommand = false;

        if (SelectorCircle.Instance.OwnNode.IsUnavailable() || SelectorCircle.Instance.OwnNode.IsOccupied()) {
          navigator.SetClosestToTarget(SelectorCircle.Instance.OwnNode);
        } else {
          navigator.targetLocation = SelectorCircle.Instance.location;
        }
      }

      #region Death handling

      // Handling the case when Grunt is on a blocked Node
      Node node = navigator.ownNode;

      // Handling death
      if (node.isBurn) {
        StartCoroutine(Death("Burn"));
      }

      if (node.isWater) {
        StartCoroutine(Death("Sink"));
      }

      if (node.isBlocked && !node.isBurn && !node.isWater) {
        StartCoroutine(Death("Squash"));
      }

      // Todo: Move to Hole script!!!
      if (LevelManager.Instance.Holez.Any(hole => hole.location.Equals(navigator.ownLocation) && hole.IsOpen)) {
        StartCoroutine(Death("Hole"));
      }

      #endregion

      if (!isInterrupted) {
        navigator.MoveTowardsTarget();
      }

      PlayAppropriateAnimation();
    }

    public bool AtLocation(Vector2Int location) {
      return navigator.ownLocation.Equals(location);
    }

    public bool IsNeighbour(MapObject mapObject) {
      return navigator.ownNode.Neighbours.Contains(mapObject.OwnNode);
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

    private void PlayAppropriateAnimation() {
      if (!AtLocation(navigator.targetLocation)) {
        animancer.Play(AnimationPack.Walk[$"{equipment.tool.toolName}Grunt_Walk_{navigator.facingDirection}"]);
      } else {
        animancer.Play(AnimationPack.Idle[$"{equipment.tool.toolName}Grunt_Idle_{navigator.facingDirection}_01"]);
      }
    }

    private void SetSingleSelected() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt1 => grunt1 != this)) {
        grunt.isSelected = false;
      }

      isSelected = true;
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
      if (IsNeighbour(targetObject)) {
        StartCoroutine(equipment.tool.Use(this));
        haveActionCommand = false;
        targetObject = null;
        _clickedNode = null;
      } else {
        Debug.Log("Setting target closest to target");
        Debug.Log("Target's node is: " + targetObject.OwnNode);
        navigator.SetClosestToTarget(targetObject.OwnNode);
      }
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
