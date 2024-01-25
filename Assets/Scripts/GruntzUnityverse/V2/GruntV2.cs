using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.V2.DataPersistence;
using GruntzUnityverse.V2.Objectz;
using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2 {
  public class GruntV2 : GridObject, IDataPersistence, IAnimatable {

    #region Pathfinder Testing
    [Header("Pathfinder Testing")]
    public bool colorDebug;

    public NodeV2 startNode;
    public NodeV2 endNode;

    public Material white;
    public Material green;
    public Material red;
    public Material blue;
    public Material yellow;
    #endregion

    #region Statz
    // --------------------------------------------------
    // Statz
    // --------------------------------------------------
    /// <summary>
    /// The name of this Grunt.
    /// </summary>
    [Header("Statz")]
    public string gruntName;

    /// <summary>
    /// The statz of this Grunt, such as health or stamina. 
    /// </summary>
    private Statz _statz;

    /// <summary>
    /// The flagz representing the current state of this Grunt.
    /// </summary>
    private Flagz _flagz;

    public bool Selected => _flagz.selected;

    /// <summary>
    /// The attribute barz of this Grunt.
    /// </summary>
    public Barz barz;
    #endregion

    #region Navigation
    /// <summary>
    /// The navigator responsible for movement and pathfinding.
    /// </summary>
    [Header("Navigation")]
    public Navigator navigator;

    /// <summary>
    /// The location of the this Grunt's target on the grid.
    /// </summary>
    public Vector2Int targetLocation2D;
    #endregion

    #region Animation
    [Header("Animation")]
    public AnimancerComponent animancer;

    public AnimationClip idle;
    #endregion

    public GameObject selectionMarker;

    private void Start() {
      Guid = System.Guid.NewGuid().ToString();
      animancer.Play(idle);
    }

    private void Update() {
      UpdateLocation();
    }

    private void UpdateLocation() {
      location2D = Vector2Int.RoundToInt(transform.position);
    }

    public void SetSelected(bool value) {
      // Debug.Log($"You selected me, {gruntName}!");
      _flagz.selected = value;
    }

    public void Move(Vector2Int target) {
      Debug.Log($"Moving {gruntName} to {targetLocation2D}");
      SetTargetLocation(target);
    }

    public void SetTargetLocation(Vector2Int target) {
      targetLocation2D = target;
    }

    public void TestPathfinding() {
      DateTime startTime = DateTime.Now;

      foreach (NodeV2 nodeV2 in startNode.transform.parent.GetComponentsInChildren<NodeV2>().ToHashSet()) {
        nodeV2.GetComponent<SpriteRenderer>().material = white;
      }

      List<NodeV2> path = Pathfinder.AstarSearch(
        startNode,
        endNode,
        startNode.transform.parent.GetComponentsInChildren<NodeV2>().ToHashSet(),
        colorDebug,
        yellow
      );

      if (path.Count <= 0) {
        return;
      }

      Debug.Log(
        $"Pathfinding took {(DateTime.Now - startTime).TotalMilliseconds}ms or {(DateTime.Now - startTime).TotalMilliseconds / 1000}s"
      );

      foreach (NodeV2 node in path) {
        node.GetComponent<SpriteRenderer>().material = green;
      }

      startNode.GetComponent<SpriteRenderer>().material = red;
      endNode.GetComponent<SpriteRenderer>().material = blue;
    }

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

    // --------------------------------------------------
    // IAnimatable
    // --------------------------------------------------
    public Animator Animator { get; set; }
    public AnimancerComponent Animancer { get; set; }
  }
}
