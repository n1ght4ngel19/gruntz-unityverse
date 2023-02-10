using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
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
    private GameObject ObjectContainer { get; set; }
    public List<Node> nodeList;
    public List<Vector2Int> nodeLocationsList;
    public Node nodePrefab;

    private void Start() {
      Application.targetFrameRate = 45;

      NodeContainer = GameObject.Find("NodeContainer");
      ObjectContainer = GameObject.Find("Objectz");

      helpBoxText = GameObject.Find("ScrollBox")
        .GetComponentInChildren<TMP_Text>();

      InitializeLevelManager();
    }

    private void InitializeLevelManager() {
      AssignLayerz();

      CreatePathfindingNodez();

      CollectObjectz();

      CollectGruntz();
    }

    private void AssignLayerz() {
      baseLayer = GameObject.Find("BaseLayer")
        .GetComponent<Tilemap>();

      baseLayer.CompressBounds();

      collisionLayer = GameObject.Find("CollisionLayer")
        .GetComponent<Tilemap>();

      collisionLayer.CompressBounds();

      MinMapPoint = new Vector2Int(collisionLayer.cellBounds.xMin, collisionLayer.cellBounds.yMin);
      MaxMapPoint = new Vector2Int(collisionLayer.cellBounds.xMax, collisionLayer.cellBounds.yMax);
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
      foreach (BlackPyramid pyramid in ObjectContainer.GetComponentsInChildren<BlackPyramid>()) {
        BlackPyramidz.Add(pyramid);
      }

      foreach (CheckpointPyramid pyramid in ObjectContainer.GetComponentsInChildren<CheckpointPyramid>()) {
        CheckpointPyramidz.Add(pyramid);
      }

      foreach (GreenPyramid pyramid in ObjectContainer.GetComponentsInChildren<GreenPyramid>()) {
        GreenPyramidz.Add(pyramid);
      }

      foreach (OrangePyramid pyramid in ObjectContainer.GetComponentsInChildren<OrangePyramid>()) {
        OrangePyramidz.Add(pyramid);
      }

      foreach (PurplePyramid pyramid in ObjectContainer.GetComponentsInChildren<PurplePyramid>()) {
        PurplePyramidz.Add(pyramid);
      }

      foreach (RedPyramid pyramid in ObjectContainer.GetComponentsInChildren<RedPyramid>()) {
        RedPyramidz.Add(pyramid);
      }

      foreach (SilverPyramid pyramid in ObjectContainer.GetComponentsInChildren<SilverPyramid>()) {
        SilverPyramidz.Add(pyramid);
      }
    }

    private void CollectGruntz() {
      foreach (Grunt grunt in GameObject.Find("PlayerGruntz")
        .GetComponentsInChildren<Grunt>()) {
        PlayerGruntz.Add(grunt);
      }
    }

    #region Node Methods

    public void SetBlockedAt(Vector2Int gridLocation, bool isBlocked) {
      nodeList.First(node => node.GridLocation.Equals(gridLocation))
        .isBlocked = isBlocked;
    }

    public void BlockNodeAt(Vector2Int gridLocation) {
      nodeList.First(node => node.GridLocation.Equals(gridLocation))
        .isBlocked = true;
    }

    public void FreeNodeAt(Vector2Int gridLocation) {
      nodeList.First(node => node.GridLocation.Equals(gridLocation))
        .isBlocked = false;
    }

    public bool IsBlockedAt(Vector2Int gridLocation) {
      return nodeList.First(node => node.GridLocation.Equals(gridLocation))
        .isBlocked;
    }

    public Node GetNodeAt(Vector2Int gridLocation) {
      return nodeList.First(node => node.GridLocation.Equals(gridLocation));
    }

    #endregion
  }
}
