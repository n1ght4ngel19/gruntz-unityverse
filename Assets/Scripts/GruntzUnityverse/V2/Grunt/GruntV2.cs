using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.DataPersistence;
using GruntzUnityverse.V2.Itemz;
using GruntzUnityverse.V2.Itemz.Toolz;
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

    public EquippedTool equippedTool;

    /// <summary>
    /// The toy currently equipped by this Grunt.
    /// </summary>
    public Toy toy;

    /// <summary>
    /// The powerup that is currently active on this Grunt.
    /// </summary>
    public Powerup powerup;
    #endregion

    public AnimationPackV2 animationPack;

    // --------------------------------------------------
    // Componentz
    // --------------------------------------------------

    #region Componentz
    public GameObject selectionMarker;
    #endregion

    public List<NodeV2> path;
    public NodeV2 targetNode;
    public NodeV2 next;

    // --------------------------------------------------
    // Events
    // --------------------------------------------------

    #region Events
    protected override void Start() {
      base.Start();

      Animancer.Play(animationPack.idle.down[0]);
    }

    private void Update() {
      ChangePosition();

      // if ()
    }
    #endregion

    public void TakeDamage(int damage) {
      statz.health -= damage;

      if (statz.health <= 0) {
        // Die();
        Debug.Log("Im dead!");
      }
    }

    // --------------------------------------------------
    // Input Actions
    // --------------------------------------------------

    #region Input Actions
    // Left click
    private void OnSelect() {
      if (GM.Instance.selector.location2D == location2D) {
        flagz.selected = true;
        selectionMarker.SetActive(true);

        GM.Instance.selectedGruntz.UniqueAdd(this);
      } else {
        flagz.selected = false;
        selectionMarker.SetActive(false);

        GM.Instance.selectedGruntz.Remove(this);
      }
    }

    // Left click & Ctrl
    private void OnAdditionalSelect() {
      if (GM.Instance.selector.location2D != location2D) {
        return;
      }

      flagz.selected = !flagz.selected;
      selectionMarker.SetActive(!selectionMarker.activeSelf);

      if (flagz.selected) {
        GM.Instance.selectedGruntz.UniqueAdd(this);
      } else {
        GM.Instance.selectedGruntz.Remove(this);
      }
    }

    // Ctrl & A
    private void OnSelectAll() {
      flagz.selected = true;
      selectionMarker.SetActive(true);

      GM.Instance.selectedGruntz.UniqueAdd(this);
    }

    // Right click
    private void OnMove() {
      // No need to check for IsInterrupted(),
      // since we need to be able to set another target while the Grunt is moving
      if (!flagz.selected) {
        return;
      }

      flagz.movingToAct = false;

      targetNode = LevelV2.Instance.levelNodes
        .First(n => n.location2D == GM.Instance.selector.location2D);

      MoveNode();
    }

    // Left click & Shift
    private async void OnAction() {
      if (!flagz.selected || IsInterrupted) {
        return;
      }

      Debug.Log("OnAction");

      GridObject interactable =
        FindObjectsByType<GridObject>(FindObjectsSortMode.None)
          .FirstOrDefault(go => go.location2D == GM.Instance.selector.location2D && go is IInteractable);

      GruntV2 grunt = GM.Instance.allGruntz
        .FirstOrDefault(g => g.location2D == GM.Instance.selector.location2D);

      // There was nothing found to interact with
      if (interactable == null && grunt == null) {
        // Todo: Play voice line for being unable to interact with nothing

        return;
      }

      // There was an interactable object found
      if (interactable != null) {
        // Check whether the Grunt has an appropriate tool equipped
        if (!((IInteractable)interactable).CompatibleItemz.Contains(equippedTool.toolName)) {
          Debug.Log("Cannot interact with this! It's not compatible!");

          // Todo: Play voice line for having an incompatible tool
          return;
        }

        flagz.movingToAct = true;
        targetNode = interactable.node;

        MoveNode();


        // await UniTask.WaitForSeconds(3f);
        await UniTask.WaitWhile(() => !node.neighbours.Contains(interactable.node), PlayerLoopTiming.Update, new CancellationToken());

        if (flagz.movingToAct) {
          Debug.Log("Alright, we're doing this!");

          Animancer.Play(animationPack.interact.down[0]);

          await UniTask.WaitForSeconds(0.5f);

          equippedTool.Use(interactable);
        }
      }
      // There was a Grunt found
      else if (grunt != null) {
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
    #endregion

    private void ChangePosition() {
      if (!flagz.moving) {
        return;
      }

      Vector3 moveVector = (next.transform.position - transform.position).normalized;
      gameObject.transform.position += moveVector * (Time.deltaTime / .6f);
    }

    /// <summary>
    /// Moves the Grunt to the current target node.
    /// </summary>
    public async void MoveNode() {
      // Only start when setting flag to false, as in, when the Grunt reaches another node
      if (flagz.moving) {
        return;
      }

      // Clicking on Grunt's own node or Grunt has reached his target
      if (node == targetNode) {
        Debug.Log("I'm there!");

        return;
      }

      // Grunt cannot move
      if (IsInterrupted) {
        Debug.Log("I'm interrupted!");

        return;
      }

      List<NodeV2> newPath = Pathfinder.AstarSearch(node, targetNode, LevelV2.Instance.levelNodes.ToHashSet());

      if (newPath.Count <= 0) {
        Debug.Log("New path is zero");

        return;
      }

      next = newPath[0];

      Vector2Int moveVector = (next.location2D - node.location2D);
      // FaceTowards(moveVector);

      flagz.moving = true;

      // DateTime startTime = DateTime.Now;

      await UniTask.WaitWhile(() => node != next);

      // DateTime endTime = DateTime.Now;
      // Debug.Log(endTime - startTime);

      flagz.moving = false;
      location2D = node.location2D;

      MoveNode();
    }

    /// <summary>
    /// Moves the Grunt to the current target node.
    /// </summary>
    public IEnumerator MoveToNode(NodeV2 otherNode) {
      flagz.interrupted = true;

      yield return new WaitUntil(() => !flagz.moving);

      targetNode = otherNode;
      next = otherNode;

      Vector2Int moveVector = (otherNode.location2D - node.location2D);
      // FaceTowards(moveVector);

      flagz.moving = true;

      yield return new WaitWhile(() => node != otherNode);

      flagz.moving = false;
      location2D = node.location2D;
      flagz.interrupted = false;
    }

    public void PlaceOnGround(NodeV2 placeNode) {
      // Todo: Go beside placeNode and place the Toy on it (if there is one equipped)
    }

    #region IAnimatable
    // --------------------------------------------------
    // IAnimatable
    // --------------------------------------------------
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public AnimancerComponent Animancer { get; set; }
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

      if (GUILayout.Button("Generate Guid")) {
        grunt.GenerateGuid();
      }

      GUILayout.Space(10);

      base.OnInspectorGUI();
    }
  }
  #endif
}
