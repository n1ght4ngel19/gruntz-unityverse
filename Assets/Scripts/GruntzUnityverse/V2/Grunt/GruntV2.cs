﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.DataPersistence;
using GruntzUnityverse.V2.Itemz;
using GruntzUnityverse.V2.Objectz;
using GruntzUnityverse.V2.Pathfinding;
using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2.Grunt {
  /// <summary>
  /// The class representing a Grunt in the game.
  /// </summary>
  public class GruntV2 : GridObject, IDataPersistence, IAnimatable {

    #region Pathfinder Testing
    [Header("Pathfinder Testing")]
    public NodeV2 testStartNode;

    public NodeV2 testEndNode;
    #endregion

    // --------------------------------------------------
    // Statz
    // --------------------------------------------------

    #region Statz
    /// <summary>
    /// The name of this Grunt.
    /// </summary>
    [Header("Statz")]
    public string gruntName;

    /// <summary>
    /// The statz of this Grunt, such as health or stamina. 
    /// </summary>
    public Statz statz;

    /// <summary>
    /// The flagz representing the current state of this Grunt.
    /// </summary>
    public Flagz flagz;

    /// <summary>
    /// The attribute barz of this Grunt.
    /// </summary>
    public Barz barz;
    #endregion

    // --------------------------------------------------
    // Equipment
    // --------------------------------------------------

    #region Equipment
    /// <summary>
    /// The tool currently equipped by this Grunt.
    /// </summary>
    [Header("Equipment")]
    public Tool tool;

    /// <summary>
    /// The toy currently equipped by this Grunt.
    /// </summary>
    public Toy toy;

    /// <summary>
    /// The powerup that is currently active on this Grunt.
    /// </summary>
    public Powerup powerup;
    #endregion

    // --------------------------------------------------
    // Navigation
    // --------------------------------------------------

    #region Navigation
    /// <summary>
    /// The navigator responsible for movement and pathfinding.
    /// </summary>
    [Header("Navigation")]
    public NavigatorV2 navigator;

    /// <summary>
    /// The location of the this Grunt's target on the grid.
    /// </summary>
    public Vector2Int targetLocation2D;
    #endregion

    #region Componentz
    // --------------------------------------------------
    // Componentz
    // --------------------------------------------------
    public GameObject selectionMarker;
    #endregion

    #region Animation
    // --------------------------------------------------
    // Animation
    // --------------------------------------------------
    public AnimationClip idle;
    #endregion

    #region Events
    // --------------------------------------------------
    // Events
    // --------------------------------------------------
    protected override void Awake() {
      base.Awake();

      Animator = GetComponent<Animator>();
      Animancer = GetComponent<AnimancerComponent>();
    }

    protected override void Start() {
      base.Start();

      Animancer.Play(idle);
    }

    private void Update() {
      UpdateLocation(); // Todo: Only update when the Grunt moves
    }
    #endregion

    /// <summary>
    /// Updates the location of the Grunt.
    /// </summary>
    private void UpdateLocation() {
      location2D = Vector2Int.RoundToInt(transform.position);
      node = GM.Instance.level.levelNodes.First(n => n.location2D == location2D);
      node.isOccupied = true;
    }

    /// <summary>
    /// Sets the selected state of the Grunt.
    /// </summary>
    /// <param name="value">The state to set.</param>
    public void SetSelected(bool value) {
      // Debug.Log($"You selected me, {gruntName}!");
      flagz.selected = value;
    }

    /// <summary>
    /// Moves the Grunt to the given location.
    /// </summary>
    /// <param name="target">The location to move to.</param>
    public void Move(Vector2Int target) {
      Debug.Log($"Moving {gruntName} to {targetLocation2D}");
      SetTargetLocation(target);

      List<NodeV2> path = Pathfinder.AstarSearch(
        navigator.node,
        navigator.targetNode,
        GM.Instance.level.levelNodes.ToHashSet()
      );
    }

    public void SetTargetLocation(Vector2Int target) {
      targetLocation2D = target;
    }

    // /// <summary>
    // /// For testing purposes.
    // /// </summary>
    public void TestPathfinding() {
      foreach (NodeV2 nodeV2 in testStartNode.transform.parent.GetComponentsInChildren<NodeV2>().ToHashSet()) {
        nodeV2.SetColor(Color.white);
      }

      DateTime startTime = DateTime.Now;

      List<NodeV2> path = Pathfinder.AstarSearch(
        testStartNode,
        testEndNode,
        GameObject.Find("NodeGrid").GetComponentsInChildren<NodeV2>().ToHashSet()
        // GM.Instance.level.levelNodes.ToHashSet()
      );

      if (path.Count <= 0) {
        Debug.Log(
          $"Pathfinding took {(DateTime.Now - startTime).TotalMilliseconds}ms or {(DateTime.Now - startTime).TotalMilliseconds / 1000}s"
        );

        return;
      }

      Debug.Log($"Path is {path.Count} nodes long");

      Debug.Log(
        $"Pathfinding took {(DateTime.Now - startTime).TotalMilliseconds}ms or {(DateTime.Now - startTime).TotalMilliseconds / 1000}s"
      );

      // foreach (NodeV2 node in path) {
      //   node.GetComponent<SpriteRenderer>().material = green;
      // }
      //
      // testStartNode.GetComponent<SpriteRenderer>().material = red;
      // testEndNode.GetComponent<SpriteRenderer>().material = blue;
    }

    #region IAnimatable
    // --------------------------------------------------
    // IAnimatable
    // --------------------------------------------------
    public Animator Animator { get; set; }
    public AnimancerComponent Animancer { get; set; }
    #endregion

    #region IDataPersistence
    // --------------------------------------------------
    // IDataPersistence
    // --------------------------------------------------
    public string Guid { get; set; }

    /// <summary>
    /// Saves the data to a GruntDataV2 object.
    /// </summary>
    /// <param name="data"></param>
    public void Save(ref GameData data) {
      GruntDataV2 saveData = new GruntDataV2 {
        guid = Guid,
        gruntName = gruntName,
        position = transform.position,
      };

      data.gruntData.SafeAdd(saveData);

      Debug.Log($"Saving {gruntName} at {transform.position} with GUID {Guid}");
    }

    // ?Unnecessary
    public void Load(GameData data) {
      // GruntDataV2 loadData = data.gruntData.First(); // Remove the data from the list so it doesn't get loaded again
      //
      // Guid = loadData.guid;
      // gruntName = loadData.gruntName;
      // transform.position = loadData.position;
    }

    /// <summary>
    /// Loads the data from a GruntDataV2 object.
    /// </summary>
    /// <param name="data"></param>
    public void Load(GruntDataV2 data) {
      Guid = data.guid;
      gruntName = data.gruntName;
      // transform.position = data.position;
    }

    public void GenerateGuid() {
      Guid = System.Guid.NewGuid().ToString();
    }
    #endregion

  }
}