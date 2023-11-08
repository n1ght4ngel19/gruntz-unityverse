using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Itemz.MiscItemz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.BaseClasses;
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
    public int gruntIdCounter = 1;
    public int playerGruntIdCounter = 1;
    public int mapObjectIdCounter = 1;
    public int puddleCounter = 0;

    public Transform arrowzParent;
    public Transform brickzParent;
    public Transform bridgezParent;
    public Transform eyeCandyParent;
    public Transform spikezParent;
    public Transform staticHazardzParent;
    public Transform holezParent;
    public Transform puzzlezParent;
    public Transform rockzParent;
    public Transform mapItemzParent;
    public Transform dizgruntledParent;
    public Transform playerGruntzParent;
    public Transform utilityparent;

    public King king;

    #region Layerz
    // ----- Layerz -----
    [field: Header("Layerz")] public Tilemap mainLayer;
    public Tilemap background;
    #endregion

    #region Objectz
    // ----- Objectz -----
    [field: Header("Objectz & Actorz")]
    public List<Grunt> player1Gruntz;
    public List<Grunt> ai1Gruntz;
    public List<Grunt> allGruntz;
    public List<RollingBall> rollingBallz;

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
    [field: Header("Pathfinding")] public GameObject nodeContainer;
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

    private void OnEnable() {
      Application.targetFrameRate = 60;
      nodeContainer = GameObject.FindGameObjectWithTag("NodeContainer");
      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();

      arrowzParent = GameObject.Find("Arrowz").transform;
      brickzParent = GameObject.Find("Brickz").transform;
      bridgezParent = GameObject.Find("Bridgez").transform;
      eyeCandyParent = GameObject.Find("EyeCandy").transform;
      spikezParent = GameObject.Find("Spikez").transform;
      staticHazardzParent = GameObject.Find("Static Hazardz").transform;
      holezParent = GameObject.Find("Holez").transform;
      puzzlezParent = GameObject.Find("Puzzlez").transform;
      rockzParent = GameObject.Find("Rockz").transform;
      dizgruntledParent = GameObject.Find("Dizgruntled").transform;
      playerGruntzParent = GameObject.Find("Player Gruntz").transform;
      mapItemzParent = GameObject.Find("MapItemz").transform;
      utilityparent = GameObject.Find("Utility").transform;

      king = FindObjectOfType<King>();

      InitializeLevel();
    }

    private void Update() {
      if (!isLevelCompleted) {
        return;
      }

      StartCoroutine(LevelWin());

      enabled = false;
    }

    private IEnumerator LevelWin() {
      int idx = Random.Range(1, 4);

      float delay = idx switch {
        1 => 1.875f,
        2 => 3.625f,
        3 => 1.625f,
        _ => 0f,
      };

      foreach (Grunt grunt in player1Gruntz) {
        grunt.enabled = false;
        StartCoroutine(grunt.Exit(idx, delay));
      }

      // Wait for Grunt exit animations to finish
      yield return new WaitForSeconds(delay);

      // Wait for Grunt exit end animation to finish
      yield return new WaitForSeconds(2.5f);

      king.StopAllCoroutines();

      StartCoroutine(king.Joy());

      yield return new WaitForSeconds(10f);

      GatherStatz();

      Addressables.LoadSceneAsync("Menuz/StatzMenu.unity").Completed += handle => {
        GameManager.Instance.hasChangedMusic = false;
      };
    }

    private void GatherStatz() {
      StatzManager.maxToolz =
        GameManager.Instance.currentLevelManager.mapObjectContainer
          .GetComponentsInChildren<Tool>()
          .Length;

      StatzManager.maxToyz =
        GameManager.Instance.currentLevelManager.mapObjectContainer
          .GetComponentsInChildren<Toy>()
          .Length;

      StatzManager.maxPowerupz =
        GameManager.Instance.currentLevelManager.mapObjectContainer
          .GetComponentsInChildren<Powerup>()
          .Length;

      StatzManager.maxCoinz =
        GameManager.Instance.currentLevelManager.mapObjectContainer
          .GetComponentsInChildren<Coin>()
          .Length;

      StatzManager.maxSecretz =
        GameManager.Instance.currentLevelManager.mapObjectContainer
          .GetComponentsInChildren<SecretSwitch>()
          .Length;

      StatzManager.maxWarpletterz =
        GameManager.Instance.currentLevelManager.mapObjectContainer
          .GetComponentsInChildren<Warpletter>()
          .Length;
    }

    private void InitializeLevel() {
      AssignLayerz();

      CreatePathfindingNodez();

      CollectObjectz();

      CollectGruntz();

      // foreach (Grunt grunt in FindObjectsOfType<Grunt>()) {
      //   grunt.SetupGrunt();
      // }

      checkpointz = FindObjectsOfType<Checkpoint>().ToList();

      foreach (GameObject go in GameObject.FindGameObjectsWithTag("Blocked")) {
        SetBlockedAt(Vector2Int.FloorToInt(go.transform.position), true);
        Destroy(go);
      }

      mapObjectContainer = GameObject.FindGameObjectWithTag("MapObjectContainer");

      mapObjectz = FindObjectsOfType<MapObject>()
        .Where(mo => mo.gameObject.GetComponent<SecretObject>() == null) // Filter Secret Objects
        .ToList();

      mapObjectz.ForEach(obj => {
        obj.Setup();
      });
    }

    private void AssignLayerz() {
      mainLayer = GameObject.Find("MainLayer").GetComponent<Tilemap>();
      mainLayer.CompressBounds();

      background = GameObject.Find("Background").GetComponent<Tilemap>();
      background.CompressBounds();

      BoundsInt mainCellBounds = mainLayer.cellBounds;
      MinMapPoint = new Vector2Int(mainCellBounds.min.x, mainCellBounds.min.y);
      MaxMapPoint = new Vector2Int(mainCellBounds.max.x, mainCellBounds.max.y);

      // Todo? Procedurally place Tiles on BackgroundLayer (WFC)
    }

    private void CreatePathfindingNodez() {
      for (int x = MinMapPoint.x; x < MaxMapPoint.x; x++) {
        for (int y = MinMapPoint.y; y < MaxMapPoint.y; y++) {
          Node node = Instantiate(nodePrefab, nodeContainer.transform);
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

      foreach (OrangeSwitch sw in FindObjectsOfType<OrangeSwitch>()) {
        OrangeSwitchez.Add(sw);
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
        grunt.gruntId = gruntIdCounter++;

        if (grunt.team == Team.Player1) {
          grunt.playerGruntId = playerGruntIdCounter++;
        }

        if (grunt.team.Equals(Team.Player1)) {
          player1Gruntz.Add(grunt);
        } else {
          ai1Gruntz.Add(grunt);
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
