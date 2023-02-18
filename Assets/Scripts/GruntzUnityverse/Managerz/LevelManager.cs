using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Objectz.Pyramidz;
using GruntzUnityverse.Objectz.Switchez;
using GruntzUnityverse.Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;

namespace GruntzUnityverse.Managerz {
  public class LevelManager : MonoBehaviour {
    #region Singleton Stuff

    private static LevelManager _instance;

    public static LevelManager Instance {
      get => _instance;
    }

    private void Awake() {
      if (_instance != null && _instance != this)
        Destroy(gameObject);
      else
        _instance = this;
    }

    #endregion

    public TMP_Text helpBoxText;

    public Tilemap baseLayer;
    public Tilemap collisionLayer;

    public Vector2Int MinMapPoint { get; set; }
    public Vector2Int MaxMapPoint { get; set; }

    [field: SerializeField] public List<Grunt> AllGruntz { get; set; }
    [field: SerializeField] public List<Grunt> PlayerGruntz { get; set; }
    [field: SerializeField] public List<BlackPyramid> BlackPyramidz { get; set; }
    [field: SerializeField] public List<CheckpointPyramid> CheckpointPyramidz { get; set; }
    [field: SerializeField] public List<GreenPyramid> GreenPyramidz { get; set; }
    [field: SerializeField] public List<OrangePyramid> OrangePyramidz { get; set; }
    [field: SerializeField] public List<PurplePyramid> PurplePyramidz { get; set; }
    [field: SerializeField] public List<RedPyramid> RedPyramidz { get; set; }
    [field: SerializeField] public List<SilverPyramid> SilverPyramidz { get; set; }


    [field: SerializeField] public List<OrangeSwitch> OrangeSwitchez { get; set; }

    private GameObject NodeContainer { get; set; }
    public List<Node> nodeList;
    public List<Vector2Int> nodeLocationsList;
    public Node nodePrefab;

    private void Start() {
      Application.targetFrameRate = 60;

      NodeContainer = GameObject.Find("NodeContainer");

      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();

      InitializeLevelManager();
    }

    private void InitializeLevelManager() {
      AssignLayerz();

      CreatePathfindingNodez();

      CollectObjectz();

      CollectGruntz();
    }

    private void AssignLayerz() {
      baseLayer = GameObject.Find("BaseLayer").GetComponent<Tilemap>();

      baseLayer.CompressBounds();

      collisionLayer = GameObject.Find("CollisionLayer").GetComponent<Tilemap>();

      collisionLayer.CompressBounds();


      BoundsInt cellBounds = collisionLayer.cellBounds;

      MinMapPoint = new Vector2Int(cellBounds.xMin, cellBounds.yMin);
      MaxMapPoint = new Vector2Int(cellBounds.xMax, cellBounds.yMax);
    }

    private void CreatePathfindingNodez() {
      for (int x = collisionLayer.cellBounds.xMin; x < collisionLayer.cellBounds.xMax; x++) {
        for (int y = collisionLayer.cellBounds.yMin; y < collisionLayer.cellBounds.yMax; y++) {
          Node node = Instantiate(nodePrefab, NodeContainer.transform);
          node.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1);
          node.GridLocation = new Vector2Int(x, y);

          nodeList.Add(node);
          nodeLocationsList.Add(node.GridLocation);

          if (collisionLayer.HasTile(new Vector3Int(x, y, 0))) {
            node.isBlocked = true;
          }
        }
      }

      foreach (Node node in nodeList) {
        node.SetNeighboursOfSelf();
      }
    }

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
    }

    private void CollectGruntz() {
      foreach (Grunt grunt in FindObjectsOfType<Grunt>()) {
        AllGruntz.Add(grunt);
      }

      foreach (Grunt grunt in FindObjectsOfType<Grunt>().Where(grunt => grunt.Owner.Equals(Owner.Self))) {
        PlayerGruntz.Add(grunt);
      }
    }

    #region Node Methods

    public void SetBlockedAt(Vector2Int gridLocation, bool isBlocked) {
      nodeList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = isBlocked;
    }

    public void BlockNodeAt(Vector2Int gridLocation) {
      nodeList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = true;
    }

    public void FreeNodeAt(Vector2Int gridLocation) {
      nodeList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = false;
    }

    public bool IsBlockedAt(Vector2Int gridLocation) {
      return nodeList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked;
    }

    public Node GetNodeAt(Vector2Int gridLocation) {
      return nodeList.First(node => node.GridLocation.Equals(gridLocation));
    }

    #endregion
  }
}
