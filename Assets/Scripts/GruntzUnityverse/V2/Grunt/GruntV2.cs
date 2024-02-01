using System;
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

    public bool IsInterrupted => flagz.interrupted;

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
    // Componentz
    // --------------------------------------------------

    #region Componentz
    public GameObject selectionMarker;
    #endregion

    // --------------------------------------------------
    // Animation
    // --------------------------------------------------

    #region Animation
    public AnimationClip idle;
    #endregion

    // --------------------------------------------------
    // Events
    // --------------------------------------------------

    #region Events
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
      node = LevelV2.Instance.levelNodes.First(n => n.location2D == location2D);
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

    // --------------------------------------------------
    // Input Actions
    // --------------------------------------------------

    #region Input Actions
    // Left click
    private void OnSelect() {
      if (GM.Instance.selector.location2D == location2D) {
        flagz.selected = true;
        GM.Instance.selectedGruntz.Add(this);
      } else {
        flagz.selected = false;
        GM.Instance.selectedGruntz.Remove(this);
      }
    }

    // Left click & Shift
    private void OnAdditionalSelect() {
      if (GM.Instance.selector.location2D != location2D) {
        return;
      }

      flagz.selected = true;
      GM.Instance.selectedGruntz.Add(this);
    }

    // Left click & Ctrl
    private void OnAction() {
      if (!flagz.selected || IsInterrupted) {
        return;
      }

      GameObject interactable =
        GameObject.FindGameObjectsWithTag("Interactable")
          .FirstOrDefault(i => i.GetComponent<GridObject>().location2D == GM.Instance.selector.location2D);

      GruntV2 grunt = GM.Instance.allGruntz
        .FirstOrDefault(g => g.location2D == GM.Instance.selector.location2D);

      if (interactable == null && grunt == null) {
        // Todo: Play voice line for not being able to interact with nothing

        return;
      }

      if (interactable != null) {
        // if (!interactable.GetComponent<IInteractable>().IsCompatibleWith(tool)) {
        //   // Todo: Play voice line for having an incompatible tool
        //   return;
        // }

        StartCoroutine(tool.Use(interactable));
      } else if (grunt != null) {
        /*
         * Todo: Take into account the following:
         * - whether the target is friendly or not
         * - the tool's reach
         * - the Grunt's ability to reach the target
         */
        StartCoroutine(tool.Use(grunt));
      }

      Debug.Log($"Acting with {gruntName}");
    }

    // Todo: Needs similar logic to OnAction
    // Left click & Alt
    private void OnGive() {
      if (!flagz.selected || IsInterrupted) {
        return;
      }

      if (toy == null) {
        // Todo: Play voice line for not having a toy

        return;
      }

      GruntV2 target = GM.Instance.allGruntz
        .FirstOrDefault(grunt => grunt.location2D == GM.Instance.selector.location2D);

      // Todo: Move beside target

      // Todo: Give toy to target
      Debug.Log($"Giving with {gruntName}");
    }

    private void OnMove() {
      if (!flagz.selected) {
        return;
      }

      if (IsInterrupted) {
        return;
      }

      NodeV2 start = LevelV2.Instance.levelNodes
        .First(n => n.location2D == location2D);

      NodeV2 target = LevelV2.Instance.levelNodes
        .First(n => n.location2D == GM.Instance.selector.location2D);

      List<NodeV2> path = Pathfinder.AstarSearch(
        start,
        target,
        LevelV2.Instance.levelNodes.ToHashSet()
      );

      if (path.Count <= 0) {
        Debug.Log("No path found");

        return;
      }

      Move(path[0]);
    }
    #endregion

    /// <summary>
    /// Moves the Grunt to the given location.
    /// </summary>
    /// <param name="target">The location to move to.</param>
    public void Move(NodeV2 next) {
      Debug.Log($"Moving {gruntName} to {next.location2D}");
      // Todo: Transform.Translate or whatever

      // Then ...
      OnMove();
    }

    public void PlaceOnGround(NodeV2 placeNode) {
      // Todo: Go beside placeNode and place the Toy on it (if there is one equipped)
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
