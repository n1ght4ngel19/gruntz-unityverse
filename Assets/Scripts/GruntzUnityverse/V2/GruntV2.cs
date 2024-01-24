using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.V2 {
  public class GruntV2 : MonoBehaviour, IGridObject {

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

    #region Navigation
    /// <summary>
    /// The navigator responsible for movement and pathfinding.
    /// </summary>
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

    // --------------------------------------------------
    // IGridObject
    // --------------------------------------------------
    public Vector2Int Location2D { get; set; }

    private void Awake() {
      animancer.Play(idle);
    }

    private void Update() {
      UpdateLocation();
    }

    private void UpdateLocation() {
      Location2D = Vector2Int.RoundToInt(transform.position);
    }

    public void Select(bool doSelect) {
      // Debug.Log($"You selected me, {gruntName}!");
      flagz.selected = doSelect;
    }

    public void Deselect() {
      flagz.selected = false;
    }

    public void Move(Vector2Int target) {
      Debug.Log($"Moving {gruntName} to {targetLocation2D}");
      SetTargetLocation(target);
    }

    public void SetTargetLocation(Vector2Int target) {
      targetLocation2D = target;
    }

    public void TestPathfinding() {
      System.DateTime startTime = System.DateTime.Now;

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
        $"Pathfinding took {(System.DateTime.Now - startTime).TotalMilliseconds}ms or {(System.DateTime.Now - startTime).TotalMilliseconds / 1000}s"
      );

      foreach (NodeV2 node in path) {
        node.GetComponent<SpriteRenderer>().material = green;
      }

      startNode.GetComponent<SpriteRenderer>().material = red;
      endNode.GetComponent<SpriteRenderer>().material = blue;
    }
  }
}
