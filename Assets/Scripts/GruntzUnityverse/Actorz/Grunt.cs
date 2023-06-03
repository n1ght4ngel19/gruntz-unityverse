﻿using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
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
    [field: SerializeField] public bool IsSelected { get; set; }
    [field: SerializeField] [CanBeNull] public MapObject TargetObject { get; set; }
    [field: SerializeField] public Equipment Equipment { get; set; }
    [field: SerializeField] public AnimancerComponent _Animancer { get; set; }
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public HealthBar HealthBar { get; set; }
    public Animator Animator { get; set; }
    public Navigator Navigator { get; set; }
    public bool IsInterrupted { get; set; }
    public GruntAnimationPack AnimationPack { get; set; }

    private void Awake() {
      _Animancer = gameObject.GetComponent<AnimancerComponent>();
      Animator = gameObject.GetComponent<Animator>();
      Navigator = gameObject.GetComponent<Navigator>();
      Equipment = gameObject.GetComponent<Equipment>();
      HealthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start() {
      Equipment.Tool = GetComponents<Tool>().FirstOrDefault();
      Equipment.Toy = GetComponents<Toy>().FirstOrDefault();
      SetAnimPack(Equipment.Tool.Name);
      Health = 20;
    }

    private void Update() {
      HealthBar.Renderer.enabled = Health != 20;

      bool haveMoveCommand = IsSelected && Input.GetMouseButtonDown(1);
      bool haveActionCommand = IsSelected && Input.GetMouseButtonDown(0);

      // Set target to previously saved target, if there is one
      if (!Navigator.IsMoving && Navigator.HaveSavedTarget) {
        Navigator.TargetLocation = Navigator.SavedTargetLocation;
        Navigator.HaveSavedTarget = false;

        return;
      }

      // Save new target for Grunt when it gets a command while moving to another tile
      if (haveMoveCommand && Navigator.IsMoving) {
        Navigator.SavedTargetLocation = SelectorCircle.Instance.OwnLocation;
        Navigator.HaveSavedTarget = true;

        return;
      }

      // Todo: Generalize
      // Handling the case when Grunt has a target already
      if (TargetObject is not null && !IsInterrupted) {
        if (Navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
          if (TargetObject is Rock) {
            StartCoroutine(Equipment.Tool.Use(this));
          }
        }
      }

      // Handling order to act
      if (haveActionCommand) {
        if (SelectorCircle.Instance.OwnLocation.Equals(Navigator.OwnLocation)) {
          // Todo: Bring up Equipment menu
        }

        // Todo: Generalize
        // Checking whether Grunt is interrupted so that its target cannot change mid-action
        if (HasTool(ToolName.Gauntletz) && !IsInterrupted) {
          Rock targetRock = LevelManager.Instance.Rockz.FirstOrDefault(
            rock => rock.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation)
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
            hole => hole.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation)
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
      if (haveMoveCommand && !IsOnLocation(SelectorCircle.Instance.OwnLocation)) {
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
          // _Animancer.Play(Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Sink"));
          StartCoroutine(Death("Sink"));
        } else {
          // Play squashed animation when on colliding Tile or Object
          // _Animancer.Play(Resources.Load<AnimationClip>("Animationz/Gruntz/Deathz/Clipz/Grunt_Death_Squash"));
          StartCoroutine(Death("Squash"));
        }
      }

      if (LevelManager.Instance.Holez.Any(hole => hole.OwnLocation.Equals(Navigator.OwnLocation) && hole.IsOpen)) {
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
        _Animancer.Play(AnimationPack.Idle[$"{Equipment.Tool.Name}Grunt_Idle_{Navigator.FacingDirection}_01"]);
      } else {
        _Animancer.Play(AnimationPack.Walk[$"{Equipment.Tool.Name}Grunt_Walk_{Navigator.FacingDirection}"]);
        Navigator.MoveTowardsTarget();
      }
    }

    public IEnumerator PickupItem(string category, string itemName) {
      switch (category) {
        case "Misc":
          _Animancer.Play(AnimationManager.Instance.PickupPack.Misc[itemName]);

          break;
        case "Tool":
          _Animancer.Play(AnimationManager.Instance.PickupPack.Tool[itemName]);

          break;
        case "Toy":
          _Animancer.Play(AnimationManager.Instance.PickupPack.Toy[itemName]);

          break;
      }

      IsInterrupted = true;

      yield return new WaitForSeconds(1f);

      SetAnimPack(itemName);

      IsInterrupted = false;
    }

    public IEnumerator Death(string deathName) {
      enabled = false;
      IsInterrupted = true;

      _Animancer.Play(AnimationManager.Instance.DeathPack[deathName]);

      yield return new WaitForSeconds(AnimationManager.Instance.DeathPack[deathName].length);

      Navigator.OwnLocation = Vector2IntCustom.Max();
      LevelManager.Instance.AllGruntz.Remove(this);
      Destroy(gameObject);
    }

    public void SetAnimPack(ToolName tool) {
      AnimationPack = tool switch {
        ToolName.Gauntletz => AnimationManager.Instance.GauntletzGruntPack,
        ToolName.Shovel => AnimationManager.Instance.ShovelGruntPack,
        _ => AnimationManager.Instance.GauntletzGruntPack,
      };
    }

    public void SetAnimPack(string tool) {
      AnimationPack = tool switch {
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
