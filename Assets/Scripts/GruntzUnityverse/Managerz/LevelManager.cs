using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Objectz.Brickz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Pyramidz;
using GruntzUnityverse.Objectz.Switchez;
using GruntzUnityverse.Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;

namespace GruntzUnityverse.Managerz {
  public class LevelManager : MonoBehaviour {
    private static LevelManager _instance;

    public static LevelManager Instance {
      get => _instance;
    }

    // ----- Layerz -----
    [field: Header("Layerz")]
    [field: SerializeField]
    public Tilemap MainLayer { get; set; }

    // [field: SerializeField] public Tilemap TransitionLayer { get; set; }
    // [field: SerializeField] public Tilemap LakeLayer { get; set; }
    // [field: SerializeField] public Tilemap DeathLayer { get; set; }
    // [field: SerializeField] public Tilemap VoidLayer { get; set; }
    [field: SerializeField] public Tilemap BackgroundLayer { get; set; }

    // Only for testing
    [field: SerializeField] public TileBase TileAsset { get; set; }

    // ----- Objectz -----
    [field: Header("Objectz")]
    [field: SerializeField]
    public List<Grunt> AllGruntz { get; set; }

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
    [field: SerializeField] public List<Hole> Holez { get; set; }

    // ----- Pathfinding -----
    [field: Header("Pathfinding")] private GameObject NodeContainer { get; set; }
    public List<Node> nodes;
    public List<Vector2Int> nodeLocations;
    public Node nodePrefab;

    public Vector2Int MinMapPoint { get; set; }
    public Vector2Int MaxMapPoint { get; set; }

    // ----- Stair -----
    [field: Header("Stair")] public TMP_Text helpBoxText;


    private void Awake() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      Application.targetFrameRate = 60;
      NodeContainer = GameObject.Find("NodeContainer");
      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();

      InitializeLevel();
    }

    private void InitializeLevel() {
      AssignLayerz();

      CreatePathfindingNodez();

      CollectObjectz();

      CollectGruntz();

      foreach (GameObject go in GameObject.FindGameObjectsWithTag("Inaccessible")) {
        go.SetActive(false);
        Instance.SetBlockedAt(Vector2Int.FloorToInt(go.transform.position), true);
      }
    }

    private void AssignLayerz() {
      MainLayer = GameObject.Find("MainLayer").GetComponent<Tilemap>();
      MainLayer.CompressBounds();

      BackgroundLayer = GameObject.Find("BackgroundLayer").GetComponent<Tilemap>();
      BackgroundLayer.CompressBounds();

      BoundsInt mainCellBounds = MainLayer.cellBounds;
      MinMapPoint = new Vector2Int(mainCellBounds.min.x, mainCellBounds.min.y);
      MaxMapPoint = new Vector2Int(mainCellBounds.max.x, mainCellBounds.max.y);

      // Todo: Procedurally place Tiles on BackgroundLayer
      // Todo: Wave Function Collapse
    }

    private void CreatePathfindingNodez() {
      for (int x = MinMapPoint.x; x < MaxMapPoint.x; x++) {
        for (int y = MinMapPoint.y; y < MaxMapPoint.y; y++) {
          Node node = Instantiate(nodePrefab, NodeContainer.transform);
          node.transform.position = new Vector3(x + 0.5f, y + 0.5f, 100);
          node.OwnLocation = new Vector2Int(x, y);

          nodes.Add(node);
          nodeLocations.Add(node.OwnLocation);

          // Todo: What about Toobz?
          if (MainLayer.HasTile(new Vector3Int(x, y, 0))) {
            // Todo: Replace 100 with NodeDepth constant
            if (MainLayer.GetTile(new Vector3Int(x, y, 0)).name.Contains("Colliding")) {
              node.isColliding = true;
              node.GetComponent<SpriteRenderer>().enabled = true;
            } else {
              node.GetComponent<SpriteRenderer>().enabled = false;
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
        Holez.Add(hole);
      }
    }

    private void CollectGruntz() {
      foreach (Grunt grunt in FindObjectsOfType<Grunt>()) {
        AllGruntz.Add(grunt);
      }

      foreach (Grunt grunt in FindObjectsOfType<Grunt>().Where(grunt => grunt.Owner.Equals(Owner.Self))) {
        PlayerGruntz.Add(grunt);
      }
    }

    public void SetBlockedAt(Vector2Int gridLocation, bool isBlocked) {
      NodeAt(gridLocation).isColliding = isBlocked;
    }

    public void SetHardTurnAt(Vector2Int gridLocation, bool isHardTurn) {
      NodeAt(gridLocation).isHardTurn = isHardTurn;
    }

    public bool IsBlockedAt(Vector2Int gridLocation) {
      return nodes.First(node => node.OwnLocation.Equals(gridLocation)).isColliding;
    }

    public Node NodeAt(Vector2Int gridLocation) {
      return nodes.First(node => node.OwnLocation.Equals(gridLocation));
    }
  }
}
