using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Itemz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    [field: SerializeField] public Owner Owner { get; set; }

    // Todo: Stamina, ToyTime, PowerupTime, MoveSpeed
    [field: SerializeField] public int Health { get; set; }
    private Animator _animator;
    [HideInInspector] public AnimancerComponent animancer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Equipment equipment;
    public HealthBar HealthBar { get; set; }
    public GruntAnimationPack AnimationPack { get; set; }
    [CanBeNull] public MapObject TargetObject { get; set; }
    private bool IsSelected { get; set; }
    public bool IsBehind { get; set; }
    public bool IsInterrupted { get; set; }
    public bool IsDying { get; set; }
    public float InitialZ { get; set; }


    private void Start() {
      gameObject.AddComponent<BoxCollider2D>();
      _animator = gameObject.AddComponent<Animator>();
      animancer = gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = _animator;
      navigator = gameObject.AddComponent<Navigator>();
      equipment = gameObject.AddComponent<Equipment>();
      equipment.Tool = GetComponents<Tool>().FirstOrDefault();
      equipment.Toy = GetComponents<Toy>().FirstOrDefault();
      HealthBar = GetComponentInChildren<HealthBar>();
      InitialZ = transform.position.z;
      Health = 20;
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt != this)) {
        Vector3 gruntPosition = grunt.transform.position;

        // Continue if not in collision zone
        if (!(Vector3.Distance(gruntPosition, transform.position) < 2f)) {
          continue;
        }

        // When other Grunt is below self
        if (grunt.IsBehind && gruntPosition.y < transform.position.y) {
          // Set other Grunt in the foreground
          gruntPosition = new Vector3(gruntPosition.x, gruntPosition.y, grunt.InitialZ);

          grunt.transform.position = gruntPosition;
          grunt.IsBehind = false;

          // Set self in the background
          transform.position += Vector3.forward * 5;
          IsBehind = true;
        }

        // When other Grunt is above self
        if (!grunt.IsBehind && gruntPosition.y >= transform.position.y) {
          // Set other Grunt in the background
          grunt.transform.position += Vector3.forward;
          grunt.IsBehind = true;

          // Set self in the foreground
          transform.position = new Vector3(transform.position.x, transform.position.y, InitialZ);
          IsBehind = false;
        }
      }

      if (AnimationPack is null) {
        SetAnimPack(equipment.Tool.Name);
      }

      HealthBar.Renderer.enabled = Health != 20;

      bool haveMoveCommand = IsSelected && Input.GetMouseButtonDown(1);

      bool haveActionCommand = IsSelected
        && Input.GetMouseButtonDown(0)
        && !LevelManager.Instance.allGruntz.Any(
          grunt => SelectorCircle.Instance.Location.Equals(grunt.navigator.OwnLocation)
        );

      // Set target to previously saved target, if there is one
      if (!navigator.IsMoving && navigator.HaveSavedTarget) {
        navigator.TargetLocation = navigator.SavedTargetLocation;
        navigator.HaveSavedTarget = false;

        return;
      }

      // Save new target for Grunt when it gets a command while moving to another tile
      if (haveMoveCommand && navigator.IsMoving) {
        navigator.SavedTargetLocation = SelectorCircle.Instance.Location;
        navigator.HaveSavedTarget = true;

        return;
      }

      // Handling the case when Grunt has a target already
      if (TargetObject is not null && !IsInterrupted) {
        if (navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
          StartCoroutine(equipment.Tool.Use(this));
        }
      }

      // Handling order to act
      if (haveActionCommand) {
        if (SelectorCircle.Instance.Location.Equals(navigator.OwnLocation)) {
          // Todo: Bring up equipment menu
        }

        // Todo: Generalize
        // Checking whether Grunt is interrupted so that its target cannot change mid-action
        if (HasTool(ToolName.Gauntletz) && !IsInterrupted) {
          MapObject target = LevelManager.Instance.Rockz.FirstOrDefault(
            mapObject => mapObject.Location.Equals(SelectorCircle.Instance.Location)
          );

          if (target is null) {
            target = LevelManager.Instance.BrickContainerz.FirstOrDefault(
              container => container.Location.Equals(SelectorCircle.Instance.Location)
            );
          }

          if (target is not null) {
            navigator.SetTargetBeside(target.OwnNode);
            TargetObject = target;

            if (navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
              StartCoroutine(equipment.Tool.Use(this));
            }
          }
        }

        // Todo: Generalize
        // Checking whether Grunt is interrupted so that its target cannot change mid-action
        if (HasTool(ToolName.Shovel) && !IsInterrupted) {
          Hole targetHole = LevelManager.Instance.Holez.FirstOrDefault(
            hole => hole.Location.Equals(SelectorCircle.Instance.Location)
          );

          if (targetHole is not null) {
            navigator.SetTargetBeside(targetHole.OwnNode);
            TargetObject = targetHole;
          }

          if (navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
            StartCoroutine(equipment.Tool.Use(this));
          }
        }
      }

      // Handling order to move
      if (haveMoveCommand && !AtLocation(SelectorCircle.Instance.Location)) {
        // Check neighbours of Node for possible destinations if Node is blocked
        if (SelectorCircle.Instance.OwnNode.isBlocked
          || LevelManager.Instance.allGruntz.Any(
            grunt => grunt.navigator.OwnNode.Equals(SelectorCircle.Instance.OwnNode)
          )) {
          navigator.SetTargetBeside(SelectorCircle.Instance.OwnNode);
        } else {
          // If Node is free
          navigator.TargetLocation = SelectorCircle.Instance.Location;
        }
      }

      // Handling the case when Grunt is on a blocked Node
      Node node = navigator.OwnNode;

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
      if (LevelManager.Instance.Holez.Any(hole => hole.Location.Equals(navigator.OwnLocation) && hole.IsOpen)) {
        StartCoroutine(Death("Hole"));
      }

      if (!IsInterrupted) {
        HandleMovement();
      }
    }

    public bool AtLocation(Vector2Int location) {
      return navigator.OwnLocation.Equals(location);
    }

    /// <summary>
    /// Decides whether the Grunt has a Tool equipped.
    /// </summary>
    /// <param name="tool">The Tool to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasTool(ToolName tool) {
      return equipment.Tool is not null && equipment.Tool.Name.Equals(tool);
    }

    /// <summary>
    /// Decides whether the Grunt has a Toy equipped.
    /// </summary>
    /// <param name="toy">The Toy to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasToy(ToyName toy) {
      return equipment.Toy is not null && equipment.Toy.Name.Equals(toy);
    }

    /// <summary>
    /// Decides between starting the next iteration of movement while playing the walking animation,
    /// and staying put playing while the idle animation.
    /// </summary>
    private void HandleMovement() {
      if (AtLocation(navigator.TargetLocation)) {
        animancer.Play(AnimationPack.Idle[$"{equipment.Tool.Name}Grunt_Idle_{navigator.FacingDirection}_01"]);
      } else {
        animancer.Play(AnimationPack.Walk[$"{equipment.Tool.Name}Grunt_Walk_{navigator.FacingDirection}"]);
        navigator.MoveTowardsTarget();
      }
    }

    public IEnumerator PickupItem(string category, string itemName) {
      switch (category) {
        case "Misc":
          animancer.Play(AnimationManager.Instance.PickupPack.Misc[itemName]);

          break;
        case "Tool":
          animancer.Play(AnimationManager.Instance.PickupPack.Tool[itemName]);

          break;
        case "Toy":
          animancer.Play(AnimationManager.Instance.PickupPack.Toy[itemName]);

          break;
      }

      IsInterrupted = true;

      // Wait the time it takes to pick up an item (subject to change)
      yield return new WaitForSeconds(0.8f);

      if (category == "Tool") {
        SetAnimPack(itemName);
      }

      IsInterrupted = false;
    }

    public IEnumerator Death(string deathName) {
      if (!IsDying) {
        IsDying = true;
        transform.position += Vector3.forward * 15;
      }

      HealthBar.Renderer.enabled = false;
      // Todo: Stair attributebars, and move into separate method
      enabled = false;
      navigator.enabled = false;
      IsInterrupted = true;

      AnimationClip deathClip = AnimationManager.Instance.DeathPack[deathName];

      animancer.Play(deathClip);

      // Wait the time it takes to play the animation (based on the animation)
      yield return new WaitForSeconds(deathClip.length);

      navigator.OwnLocation = Vector2IntExtra.Max();
      LevelManager.Instance.allGruntz.Remove(this);
      Destroy(gameObject, deathClip.length);
    }

    public IEnumerator Death() {
      HealthBar.Renderer.enabled = false;
      // Todo: Stair attributebars, and move into separate method
      enabled = false;
      navigator.enabled = false;
      IsInterrupted = true;
      AnimationClip deathClip = AnimationPack.Death[$"{equipment.Tool.GetType().Name}Grunt_Death_01"];

      animancer.Play(deathClip);

      yield return new WaitForSeconds(deathClip.length);

      navigator.OwnLocation = Vector2IntExtra.Max();
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
        "Barehandz" => AnimationManager.Instance.BarehandzGruntPack,
        "Gauntletz" => AnimationManager.Instance.GauntletzGruntPack,
        "Shovel" => AnimationManager.Instance.ShovelGruntPack,
        _ => AnimationManager.Instance.GauntletzGruntPack,
      };
    }


    /// <summary>
    /// Selects the Grunt under the mouse and deselects all others.
    /// </summary>
    protected void OnMouseDown() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz) {
        grunt.IsSelected = grunt == this;
      }
    }
  }
}
