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
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    [field: SerializeField] public Owner Owner { get; set; }
    public CommandMode commandMode;
    public bool isSelected;
    public bool haveActionCommand;

    // Todo: Stamina, ToyTime, PowerupTime, MoveSpeed
    [field: SerializeField] public int Health { get; set; }
    private Animator _animator;
    [HideInInspector] public AnimancerComponent animancer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Equipment equipment;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public GruntSelectedCircle selectedCircle;

    public HealthBar HealthBar { get; set; }
    public GruntAnimationPack AnimationPack { get; set; }
    [CanBeNull] public MapObject TargetObject { get; set; }
    public bool IsBehind { get; set; }
    public bool IsInterrupted { get; set; }
    public float InitialZ { get; set; }
    private bool _isDying;


    private void Start() {
      BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
      boxCollider.size = new Vector2(1f, 1f);
      _animator = gameObject.AddComponent<Animator>();
      animancer = gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = _animator;
      navigator = gameObject.AddComponent<Navigator>();
      equipment = gameObject.AddComponent<Equipment>();
      equipment.tool = GetComponents<Tool>().FirstOrDefault();
      equipment.toy = GetComponents<Toy>().FirstOrDefault();
      spriteRenderer = GetComponent<SpriteRenderer>();
      selectedCircle = GetComponentInChildren<GruntSelectedCircle>();
      HealthBar = GetComponentInChildren<HealthBar>();
      InitialZ = transform.position.z;
      Health = 20;
    }

    private void Update() {
      // foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt != this)) {
      //   Vector3 gruntPosition = grunt.transform.position;
      //
      //   // Continue if not in collision zone
      //   if (!(Vector3.Distance(gruntPosition, transform.position) < 2f)) {
      //     continue;
      //   }
      //
      //   // When other Grunt is below self
      //   if (grunt.IsBehind && gruntPosition.y < transform.position.y) {
      //     // Set other Grunt in the foreground
      //     gruntPosition = new Vector3(gruntPosition.x, gruntPosition.y, grunt.InitialZ);
      //
      //     grunt.transform.position = gruntPosition;
      //     grunt.IsBehind = false;
      //
      //     // Set self in the background
      //     transform.position += Vector3.forward * 5;
      //     IsBehind = true;
      //   }
      //
      //   // When other Grunt is above self
      //   if (!grunt.IsBehind && gruntPosition.y >= transform.position.y) {
      //     // Set other Grunt in the background
      //     grunt.transform.position += Vector3.forward;
      //     grunt.IsBehind = true;
      //
      //     // Set self in the foreground
      //     transform.position = new Vector3(transform.position.x, transform.position.y, InitialZ);
      //     IsBehind = false;
      //   }
      // }

      HealthBar.Renderer.enabled = Health != 20;
      navigator.hasMoveCommand = isSelected && Input.GetMouseButtonDown(1);


      if (isSelected
        && Input.GetMouseButtonDown(0)
        && Input.GetKey(KeyCode.LeftShift)
        && !LevelManager.Instance.allGruntz.Any(
          grunt => SelectorCircle.Instance.location.Equals(grunt.navigator.ownLocation)
        )) {
        haveActionCommand = true;

        TargetObject = GameObject.Find("===== Objectz =====")
          .GetComponentsInChildren<MapObject>()
          .FirstOrDefault(o => o.OwnNode == SelectorCircle.Instance.OwnNode);
      }

      // Handling action order
      if (haveActionCommand && !IsInterrupted) {
        // if (SelectorCircle.Instance.location.Equals(navigator.ownLocation)) {
        //   // Todo: Bring up equipment menu
        // }

        // Todo: Replace ifs with switch statement (e.g. case (ToolName.Gauntletz):)

        if (HasTool(ToolName.Gauntletz)) {
          if (TargetObject is Rock or BrickContainer or GiantRockEdge) {
            MoveOrDo();
          } else {
            Debug.Log("No can do");
            haveActionCommand = false;
          }
        }

        if (HasTool(ToolName.Shovel)) {
          if (TargetObject is Hole) {
            MoveOrDo();
          } else {
            Debug.Log("No can do");
            haveActionCommand = false;
          }
        }
      }

      // Setting move target
      if (navigator.hasMoveCommand && !AtLocation(SelectorCircle.Instance.location)) {
        haveActionCommand = false;

        // If Node is blocked
        if (SelectorCircle.Instance.OwnNode.isBlocked
          || LevelManager.Instance.allGruntz.Any(
            grunt => grunt.navigator.ownNode.Equals(SelectorCircle.Instance.OwnNode)
          )) {
          navigator.SetClosestToTarget(SelectorCircle.Instance.OwnNode);
        } else {
          // If Node is free
          navigator.targetLocation = SelectorCircle.Instance.location;
        }
      }

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

      // Handling actual movement
      if (!IsInterrupted) {
        HandleMovement();
      }
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

    /// <summary>
    /// Decides between starting the next iteration of movement while playing the walking animation,
    /// and staying put playing while the idle animation.
    /// </summary>
    private void HandleMovement() {
      if (!AtLocation(navigator.targetLocation)) {
        animancer.Play(AnimationPack.Walk[$"{equipment.tool.toolName}Grunt_Walk_{navigator.facingDirection}"]);
        navigator.MoveTowardsTarget();
      } else {
        animancer.Play(AnimationPack.Idle[$"{equipment.tool.toolName}Grunt_Idle_{navigator.facingDirection}_01"]);
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

    public void MoveOrDo() {
      if (IsNeighbour(TargetObject)) {
        StartCoroutine(equipment.tool.Use(this));
        haveActionCommand = false;
      } else {
        navigator.SetClosestToTarget(TargetObject.OwnNode);
      }
    }

    public IEnumerator Death(string deathName) {
      if (!_isDying) {
        _isDying = true;
        transform.position += Vector3.forward * 15;
      }

      HealthBar.Renderer.enabled = false;
      // Todo: Stair attribute bars, and move into separate method
      enabled = false;
      navigator.enabled = false;
      IsInterrupted = true;

      AnimationClip deathClip = AnimationManager.Instance.DeathPack[deathName];

      animancer.Play(deathClip);

      // Wait the time it takes to play the animation (based on the animation)
      yield return new WaitForSeconds(deathClip.length);

      navigator.ownLocation = Vector2IntExtra.Max();
      LevelManager.Instance.allGruntz.Remove(this);
      Destroy(gameObject, deathClip.length);
    }

    public IEnumerator Death() {
      HealthBar.Renderer.enabled = false;
      // Todo: Stair attribute bars, and move into separate method
      enabled = false;
      navigator.enabled = false;
      IsInterrupted = true;
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
        "Barehandz" => AnimationManager.Instance.BarehandzGruntPack,
        "Gauntletz" => AnimationManager.Instance.GauntletzGruntPack,
        "Shovel" => AnimationManager.Instance.ShovelGruntPack,
        _ => AnimationManager.Instance.GauntletzGruntPack,
      };
    }

    protected void OnMouseDown() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz) {
        if (grunt == this) {
          grunt.isSelected = true;
          // grunt.selectedCircle.enabled = true;
        } else {
          grunt.isSelected = false;
          // grunt.selectedCircle.enabled = false;
        }
      }
    }
  }
}
