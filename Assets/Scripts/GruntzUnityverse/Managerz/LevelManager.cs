using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Brickz;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.MapObjectz.Pyramidz;
using GruntzUnityverse.MapObjectz.Switchez;
using GruntzUnityverse.Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;

namespace GruntzUnityverse.Managerz {
  public class LevelManager : MonoBehaviour {
    private static LevelManager _instance;

    public static LevelManager Instance {
      get => _instance;
    }

    public int gruntIdCounter = 0;

    #region Layerz
    // ----- Layerz -----
    [field: Header("Layerz")] public Tilemap mainLayer;
    public Tilemap backgroundLayer;
    #endregion

    #region Objectz
    // ----- Objectz -----
    [field: Header("Objectz")] public List<Grunt> playerGruntz;
    public List<Grunt> enemyGruntz;
    public List<Grunt> allGruntz;

    [field: SerializeField] public List<Grunt> PlayerGruntz { get; set; }
    [field: SerializeField] public List<BrickContainer> BrickContainerz { get; set; }
    [field: SerializeField] public List<BrickFoundation> BrickFoundationz { get; set; }
    [field: SerializeField] public List<BrickColumn> BrickColumnz { get; set; }
    [field: SerializeField] public List<BlackPyramid> BlackPyramidz { get; set; }
    [field: SerializeField] public List<CheckpointPyramid> CheckpointPyramidz { get; set; }
    [field: SerializeField] public List<GreenPyramid> GreenPyramidz { get; set; }
    [field: SerializeField] public List<OrangePyramid> OrangePyramidz { get; set; }
    [field: SerializeField] public List<PurplePyramid> PurplePyramidz { get; set; }
    [field: SerializeField] public List<RedPyramid> RedPyramidz { get; set; }
    [field: SerializeField] public List<SilverPyramid> SilverPyramidz { get; set; }

    [field: SerializeField] public List<OrangeSwitch> OrangeSwitchez { get; set; }
    [field: SerializeField] public List<Rock> Rockz { get; set; }
    public List<Hole> holez;
    #endregion

    #region Pathfinding
    // ----- Pathfinding -----
    [field: Header("Pathfinding")] private GameObject NodeContainer { get; set; }
    public List<Node> nodes;
    public List<Vector2Int> nodeLocations;
    public Node nodePrefab;

    public Vector2Int MinMapPoint { get; set; }
    public Vector2Int MaxMapPoint { get; set; }
    #endregion

    #region Flags
    public bool isLevelCompleted;
    #endregion

    #region Others
    public List<Checkpoint> checkpointz;
    public TMP_Text helpBoxText;
    public GameObject mapObjectContainer;
    public List<MapObject> mapObjectz;
    #endregion


    private void Awake() {
      if (_instance is not null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      Application.targetFrameRate = 60;
      NodeContainer = GameObject.Find("NodeContainer");
      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();

      InitializeLevel();
    }

    private void Update() {
      if (isLevelCompleted) {
        StartCoroutine(LevelWin());

        enabled = false;
      }
    }

    private IEnumerator LevelWin() {
      int idx = Random.Range(1, 4);

      float delay = idx switch {
        1 => 1.875f,
        2 => 3.625f,
        3 => 1.625f,
        _ => 0f,
      };

      foreach (Grunt grunt in playerGruntz) {
        grunt.enabled = false;
        StartCoroutine(grunt.Exit(idx, delay));
      }

      // Wait for Grunt exit animations to finish
      yield return new WaitForSeconds(delay);

      // foreach (Grunt grunt in playerGruntz) {
      //   grunt.enabled = false;
      //   grunt.animancer.Play(AnimationManager.Instance.exitPack["Grunt_Exit_End"]);
      // }

      // Wait for Grunt exit end animation to finish
      yield return new WaitForSeconds(2.5f);

      // Todo: Play King voice and dance animations
      yield return null;

      Addressables.LoadSceneAsync("Menuz/StatzMenu.unity");
    }

    private void InitializeLevel() {
      AssignLayerz();

      CreatePathfindingNodez();

      CollectObjectz();

      CollectGruntz();

      checkpointz = FindObjectsOfType<Checkpoint>().ToList();

      foreach (GameObject go in GameObject.FindGameObjectsWithTag("Blocked")) {
        Instance.SetBlockedAt(Vector2Int.FloorToInt(go.transform.position), true);
        Destroy(go);
      }

      mapObjectContainer = GameObject.Find(GlobalNamez.MapObjectContainer);
      mapObjectz = mapObjectContainer.GetComponentsInChildren<MapObject>().ToList();
    }

    private void AssignLayerz() {
      mainLayer = GameObject.Find("MainLayer").GetComponent<Tilemap>();
      mainLayer.CompressBounds();

      backgroundLayer = GameObject.Find("BackgroundLayer").GetComponent<Tilemap>();
      backgroundLayer.CompressBounds();

      BoundsInt mainCellBounds = mainLayer.cellBounds;
      MinMapPoint = new Vector2Int(mainCellBounds.min.x, mainCellBounds.min.y);
      MaxMapPoint = new Vector2Int(mainCellBounds.max.x, mainCellBounds.max.y);

      // Todo? Procedurally place Tiles on BackgroundLayer (WFC)
    }

    private void CreatePathfindingNodez() {
      for (int x = MinMapPoint.x; x < MaxMapPoint.x; x++) {
        for (int y = MinMapPoint.y; y < MaxMapPoint.y; y++) {
          Node node = Instantiate(nodePrefab, NodeContainer.transform);
          node.transform.position = new Vector3(x + 1f, y + 1f, 100);
          node.location = new Vector2Int(x + 1, y + 1);
          node.GetComponent<SpriteRenderer>().enabled = false;

          nodes.Add(node);
          nodeLocations.Add(node.location);

          // Todo: What about Toobz?
          if (mainLayer.HasTile(new Vector3Int(x, y, 0))) {
            TileBase currentTile = mainLayer.GetTile(new Vector3Int(x, y, 0));

            if (currentTile.name.Contains("Collision")) {
              node.isBlocked = true;
            }

            if (currentTile.name.Contains("Water")) {
              node.isWater = true;
            }

            if (currentTile.name.Contains("Burn")) {
              node.isBurn = true;
            }
          }
        }
      }

      foreach (Node node in nodes) {
        node.SetNeighbours();
      }
    }

    // Todo: Move to individual object classes?
    private void CollectObjectz() {
      foreach (BlackPyramid pyramid in FindObjectsOfType<BlackPyramid>()) {
        BlackPyramidz.Add(pyramid);
      }

      foreach (CheckpointPyramid pyramid in FindObjectsOfType<CheckpointPyramid>()) {
        CheckpointPyramidz.Add(pyramid);
      }

      foreach (GreenPyramid pyramid in FindObjectsOfType<GreenPyramid>()) {
        GreenPyramidz.Add(pyramid);
      }

      foreach (OrangePyramid pyramid in FindObjectsOfType<OrangePyramid>()) {
        OrangePyramidz.Add(pyramid);
      }

      foreach (PurplePyramid pyramid in FindObjectsOfType<PurplePyramid>()) {
        PurplePyramidz.Add(pyramid);
      }

      foreach (RedPyramid pyramid in FindObjectsOfType<RedPyramid>()) {
        RedPyramidz.Add(pyramid);
      }

      foreach (SilverPyramid pyramid in FindObjectsOfType<SilverPyramid>()) {
        SilverPyramidz.Add(pyramid);
      }

      foreach (Rock rock in FindObjectsOfType<Rock>()) {
        Rockz.Add(rock);
      }

      foreach (Hole hole in FindObjectsOfType<Hole>()) {
        holez.Add(hole);
      }
    }

    private void CollectGruntz() {
      foreach (Grunt grunt in FindObjectsOfType<Grunt>()) {
        grunt.gruntId = gruntIdCounter;
        gruntIdCounter++;

        if (grunt.owner.Equals(Owner.Player)) {
          playerGruntz.Add(grunt);
        } else {
          enemyGruntz.Add(grunt);
        }

        allGruntz.Add(grunt);
      }
    }

    #region Node Methods
    public void SetBlockedAt(Vector2Int gridLocation, bool isBlocked) {
      NodeAt(gridLocation).isBlocked = isBlocked;
    }

    public void SetBurnAt(Vector2Int gridLocation, bool isBurn) {
      NodeAt(gridLocation).isBurn = isBurn;
    }

    public void SetDeathAt(Vector2Int gridLocation, bool isDeath) {
      NodeAt(gridLocation).isDeath = isDeath;
    }

    public void SetEdgeAt(Vector2Int gridLocation, bool isEdge) {
      NodeAt(gridLocation).isEdge = isEdge;
    }

    public void SetHardTurnAt(Vector2Int gridLocation, bool isHardTurn) {
      NodeAt(gridLocation).isHardTurn = isHardTurn;
    }

    public void SetVoidAt(Vector2Int gridLocation, bool isVoid) {
      NodeAt(gridLocation).isVoid = isVoid;
    }

    public void SetWaterAt(Vector2Int gridLocation, bool isWater) {
      NodeAt(gridLocation).isWater = isWater;
    }

    public bool IsBlockedAt(Vector2Int gridLocation) {
      return NodeAt(gridLocation).isBlocked;
    }

    public bool IsBurnAt(Vector2Int gridLocation) {
      return NodeAt(gridLocation).isBurn;
    }

    public bool IsDeathAt(Vector2Int gridLocation) {
      return NodeAt(gridLocation).isDeath;
    }

    public bool IsEdgeAt(Vector2Int gridLocation) {
      return NodeAt(gridLocation).isEdge;
    }

    public bool IsHardTurnAt(Vector2Int gridLocation) {
      return NodeAt(gridLocation).isHardTurn;
    }

    public bool IsVoidAt(Vector2Int gridLocation) {
      return NodeAt(gridLocation).isVoid;
    }

    public bool IsWaterAt(Vector2Int gridLocation) {
      return NodeAt(gridLocation).isWater;
    }

    public Node NodeAt(Vector2Int gridLocation) {
      return nodes.First(node => node.location.Equals(gridLocation));
    }
    #endregion
  }
}
