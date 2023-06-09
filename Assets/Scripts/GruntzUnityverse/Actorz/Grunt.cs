using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Itemz;
using GruntzUnityverse.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    [field: SerializeField] public Owner Owner { get; set; }

    // Todo: Stamina, ToyTime, PowerupTime, MoveSpeed
    [field: SerializeField] public int Health { get; set; }
    private Animator Animator { get; set; }
    public AnimancerComponent Animancer { get; set; }
    public Navigator Navigator { get; set; }
    public Equipment Equipment { get; set; }
    public HealthBar HealthBar { get; set; }
    public GruntAnimationPack AnimationPack { get; set; }
    [CanBeNull] public MapObject TargetObject { get; set; }
    public bool IsSelected { get; set; }
    public bool IsBehind { get; set; }
    public bool IsInterrupted { get; set; }
    public bool IsDying { get; set; }
    public float InitialZ { get; set; }

    private void Awake() {
      Animator = gameObject.AddComponent<Animator>();
      Animancer = gameObject.AddComponent<AnimancerComponent>();
      Animancer.Animator = Animator;
      Navigator = gameObject.GetComponent<Navigator>();
      Equipment = gameObject.AddComponent<Equipment>();
      Equipment.Tool = GetComponents<Tool>().FirstOrDefault();
      Equipment.Toy = GetComponents<Toy>().FirstOrDefault();
      HealthBar = GetComponentInChildren<HealthBar>();
      InitialZ = transform.position.z;
    }

    private void Start() {
      Health = 20;
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt != this)) {
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
        SetAnimPack(Equipment.Tool.Name);
      }

      HealthBar.Renderer.enabled = Health != 20;

      bool haveMoveCommand = IsSelected && Input.GetMouseButtonDown(1);

      bool haveActionCommand = IsSelected
        && Input.GetMouseButtonDown(0)
        && !LevelManager.Instance.AllGruntz.Any(
          grunt => SelectorCircle.Instance.Location.Equals(grunt.Navigator.OwnLocation)
        );

      // Set target to previously saved target, if there is one
      if (!Navigator.IsMoving && Navigator.HaveSavedTarget) {
        Navigator.TargetLocation = Navigator.SavedTargetLocation;
        Navigator.HaveSavedTarget = false;

        return;
      }

      // Save new target for Grunt when it gets a command while moving to another tile
      if (haveMoveCommand && Navigator.IsMoving) {
        Navigator.SavedTargetLocation = SelectorCircle.Instance.Location;
        Navigator.HaveSavedTarget = true;

        return;
      }

      // Handling the case when Grunt has a target already
      if (TargetObject is not null && !IsInterrupted) {
        if (Navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
          StartCoroutine(Equipment.Tool.Use(this));
        }
      }

      // Handling order to act
      if (haveActionCommand) {
        if (SelectorCircle.Instance.Location.Equals(Navigator.OwnLocation)) {
          // Todo: Bring up Equipment menu
        }

        // Todo: Generalize
        // Checking whether Grunt is interrupted so that its target cannot change mid-action
        if (HasTool(ToolName.Gauntletz) && !IsInterrupted) {
          Rock targetRock = LevelManager.Instance.Rockz.FirstOrDefault(
            rock => rock.Location.Equals(SelectorCircle.Instance.Location)
          );

          if (targetRock is not null) {
            Navigator.SetTargetBeside(targetRock.OwnNode);
            TargetObject = targetRock;

            if (Navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
              StartCoroutine(Equipment.Tool.Use(this));
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
            Navigator.SetTargetBeside(targetHole.OwnNode);
            TargetObject = targetHole;
          }

          if (Navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
            StartCoroutine(Equipment.Tool.Use(this));
          }
        }
      }

      // Handling order to move
      if (haveMoveCommand && !IsOnLocation(SelectorCircle.Instance.Location)) {
        // Check neighbours of Node for possible destinations if Node is blocked
        if (SelectorCircle.Instance.OwnNode.isBlocked
          || LevelManager.Instance.AllGruntz.Any(
            grunt => grunt.Navigator.OwnNode.Equals(SelectorCircle.Instance.OwnNode)
          )) {
          Navigator.SetTargetBeside(SelectorCircle.Instance.OwnNode);
        } else {
          // If Node is free
          Navigator.TargetLocation = SelectorCircle.Instance.OwnNode.OwnLocation;
        }
      }

      // Handling the case when Grunt is on a blocked Node
      if (LevelManager.Instance.nodeList.Any(node => node.isBlocked && IsOnLocation(node.OwnLocation))) {
        // Play drowning animation when on a Lake (Water or Death) tile
        if (LevelManager.Instance.LakeLayer.HasTile(
          new Vector3Int(Navigator.OwnLocation.x, Navigator.OwnLocation.y, 0)
        )) {
          // Animancer.Play(Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Sink"));
          StartCoroutine(Death("Sink"));
        } else {
          // Play squashed animation when on colliding Tile or Object
          // Animancer.Play(Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Squash"));
          StartCoroutine(Death("Squash"));
        }
      }

      // Todo: Move to Hole script!!!
      if (LevelManager.Instance.Holez.Any(hole => hole.Location.Equals(Navigator.OwnLocation) && hole.IsOpen)) {
        StartCoroutine(Death("Hole"));
      }

      if (!IsInterrupted) {
        HandleMovement();
      }
    }

    public bool IsOnLocation(Vector2Int location) {
      return Navigator.OwnLocation.Equals(location);
    }

    /// <summary>
    /// Decides whether the Grunt has a Tool equipped.
    /// </summary>
    /// <param name="tool">The Tool to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasTool(ToolName tool) {
      return Equipment.Tool is not null && Equipment.Tool.Name.Equals(tool);
    }

    /// <summary>
    /// Decides whether the Grunt has a Toy equipped.
    /// </summary>
    /// <param name="toy">The Toy to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasToy(ToyName toy) {
      return Equipment.Toy is not null && Equipment.Toy.Name.Equals(toy);
    }

    /// <summary>
    /// Decides between starting the next iteration of movement while playing the walking animation,
    /// and staying put playing while the idle animation.
    /// </summary>
    private void HandleMovement() {
      if (IsOnLocation(Navigator.TargetLocation)) {
        Animancer.Play(AnimationPack.Idle[$"{Equipment.Tool.Name}Grunt_Idle_{Navigator.FacingDirection}_01"]);
      } else {
        Animancer.Play(AnimationPack.Walk[$"{Equipment.Tool.Name}Grunt_Walk_{Navigator.FacingDirection}"]);
        Navigator.MoveTowardsTarget();
      }
    }

    public IEnumerator PickupItem(string category, string itemName) {
      switch (category) {
        case "Misc":
          Animancer.Play(AnimationManager.Instance.PickupPack.Misc[itemName]);

          break;
        case "Tool":
          Animancer.Play(AnimationManager.Instance.PickupPack.Tool[itemName]);

          break;
        case "Toy":
          Animancer.Play(AnimationManager.Instance.PickupPack.Toy[itemName]);

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
      // Todo: Other attributebars, and move into separate method
      enabled = false;
      Navigator.enabled = false;
      IsInterrupted = true;

      AnimationClip deathClip = AnimationManager.Instance.DeathPack[deathName];

      Animancer.Play(deathClip);

      yield return new WaitForSeconds(deathClip.length);

      Navigator.OwnLocation = Vector2IntCustom.Max();
      LevelManager.Instance.AllGruntz.Remove(this);
      Destroy(gameObject, deathClip.length);
    }

    public IEnumerator Death() {
      HealthBar.Renderer.enabled = false;
      // Todo: Other attributebars, and move into separate method
      enabled = false;
      Navigator.enabled = false;
      IsInterrupted = true;
      AnimationClip deathClip = AnimationPack.Death[$"{Equipment.Tool.GetType().Name}Grunt_Death_01"];

      Animancer.Play(deathClip);

      yield return new WaitForSeconds(deathClip.length);

      Navigator.OwnLocation = Vector2IntCustom.Max();
      LevelManager.Instance.AllGruntz.Remove(this);
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
      IsSelected = true;

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt != this)) {
        grunt.IsSelected = false;
      }
    }
  }
}
