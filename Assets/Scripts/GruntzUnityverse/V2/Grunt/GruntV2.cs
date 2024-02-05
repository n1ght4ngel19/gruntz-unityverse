using System;
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
using UnityEditor;
using UnityEngine;
using Tool = GruntzUnityverse.V2.Itemz.Tool;

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

    public List<NodeV2> path;
    public NodeV2 targetNode;
    public NodeV2 next;

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
      if (flagz.moving) {
        Vector3 moveVector = (next.transform.position - transform.position).normalized;
        gameObject.transform.position += moveVector * (Time.deltaTime / .6f);
        location2D = node.location2D;
      }
    }
    #endregion

    public void TakeDamage(int damage) {
      statz.health -= damage;

      if (statz.health <= 0) {
        // Die();
        Debug.Log("Im dead!");
      }
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
        Debug.Log($"Selected {gruntName}");
        flagz.selected = true;
        GM.Instance.selectedGruntz.UniqueAdd(this);
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
      GM.Instance.selectedGruntz.UniqueAdd(this);
    }

    // Left click & Ctrl
    private void OnAction() {
      if (!flagz.selected || IsInterrupted) {
        return;
      }

      Debug.Log("OnAction");

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

      Debug.Log("OnGive");

      GruntV2 target = GM.Instance.allGruntz
        .FirstOrDefault(grunt => grunt.location2D == GM.Instance.selector.location2D);

      // Todo: Move beside target

      // Todo: Give toy to target
      Debug.Log($"Giving with {gruntName}");
    }

    // Right click
    private void OnMove() {
      // No need to check for IsInterrupted(),
      // since we need to be able to set another target while the Grunt is moving
      if (!flagz.selected) {
        return;
      }

      targetNode = LevelV2.Instance.levelNodes
        .First(n => n.location2D == GM.Instance.selector.location2D);

      StartCoroutine(Move());
    }
    #endregion

    /// <summary>
    /// Moves the Grunt to the current target node.
    /// </summary>
    public IEnumerator Move() {
      if (flagz.moving) {
        yield break;
      }

      // Clicking on Grunt's own node or Grunt has reached his target
      if (node == targetNode) {
        Debug.Log("I'm there!");

        yield break;
      }

      // Grunt cannot move
      if (IsInterrupted) {
        Debug.Log("I'm interrupted!");

        // return;
        yield break;
      }

      List<NodeV2> newPath = Pathfinder.AstarSearch(node, targetNode, LevelV2.Instance.levelNodes.ToHashSet());

      if (newPath.Count <= 0) {
        Debug.Log("New path is zero");

        yield break;
      }

      next = newPath[0];

      Vector2Int moveVector = (next.location2D - node.location2D);
      // FaceTowards(moveVector);

      flagz.moving = true;

      DateTime startTime = DateTime.Now;

      // It takes 0.6 seconds to move to the next node
      yield return new WaitWhile(() => node != next);

      DateTime endTime = DateTime.Now;

      Debug.Log(endTime - startTime);

      flagz.moving = false;

      StartCoroutine(Move());
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

      path = Pathfinder.AstarSearch(
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

    // --------------------------------------------------
    // IDataPersistence
    // --------------------------------------------------

    #region IDataPersistence
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

      data.gruntData.CheckNotExistsAdd(saveData);

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

  #if UNITY_EDITOR
  [CustomEditor(typeof(GruntV2))]
  public class GruntV2Editor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      GruntV2 grunt = (GruntV2)target;

      GUILayout.Space(10);

      if (GUILayout.Button("Test Pathfinding")) {
        grunt.TestPathfinding();
      }

      GUILayout.Space(10);

      if (GUILayout.Button("Generate Guid")) {
        grunt.GenerateGuid();
      }

      GUILayout.Space(10);

      base.OnInspectorGUI();
    }
  }
  #endif
}
