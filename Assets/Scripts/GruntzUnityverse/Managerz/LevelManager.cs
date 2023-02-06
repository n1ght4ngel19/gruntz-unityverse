using System.Collections.Generic;
using System.Linq;
using _Test;
using GruntzUnityverse.Objectz.Pyramidz;
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

    [field: SerializeField] public List<TGrunt> PlayerGruntz { get; set; }
    [field: SerializeField] public List<CheckpointPyramid> CheckpointPyramidz { get; set; }
    [field: SerializeField] public List<GreenPyramid> GreenPyramidz { get; set; }
    [field: SerializeField] public List<RedPyramid> RedPyramidz { get; set; }

    private GameObject NodeContainer {get;set;}
    private GameObject ObjectContainer {get;set;}
    public List<Node> nodeList;
    public List<Vector2Int> nodeLocationsList;
    public Node nodePrefab;

    private void Start() {
      Application.targetFrameRate = 60;

      NodeContainer = GameObject.Find("NodeContainer");
      ObjectContainer = GameObject.Find("Objectz");
      
      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();

      InitializeLevelManager();
    }

    private void InitializeLevelManager() {
      AssignLayerz();
      
      CreatePathfindingNodez();
      
      CollectObjectz();
      
      CollectGruntz();

      SetDefaultObjectCollisionz();
    }

    private void AssignLayerz() {
      baseLayer = GameObject.Find("BaseLayer").GetComponent<Tilemap>();
      collisionLayer = GameObject.Find("CollisionLayer").GetComponent<Tilemap>();
    }

    private void CreatePathfindingNodez() {
      for (int x = 0; x < baseLayer.cellBounds.xMax; x++) {
        for (int y = 0; y < baseLayer.cellBounds.yMax; y++) {
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
    }
    
    private void CollectObjectz() {
      foreach (CheckpointPyramid checkpointPyramid in ObjectContainer.GetComponentsInChildren<CheckpointPyramid>()) {
        CheckpointPyramidz.Add(checkpointPyramid);
      }

      foreach (RedPyramid pyramid in ObjectContainer.GetComponentsInChildren<RedPyramid>()) {
        RedPyramidz.Add(pyramid);
      }
      
      foreach (GreenPyramid pyramid in ObjectContainer.GetComponentsInChildren<GreenPyramid>()) {
        GreenPyramidz.Add(pyramid);
      }
    }
    
    private void CollectGruntz() {
      foreach (TGrunt grunt in GameObject.Find("PlayerGruntz").GetComponentsInChildren<TGrunt>()) {
        PlayerGruntz.Add(grunt);
      }
    }

    private void SetDefaultObjectCollisionz() {
      foreach (CheckpointPyramid pyramid in CheckpointPyramidz) {
        SetBlockedAt(pyramid.OwnLocation, !pyramid.IsDown);
      }

      foreach (GreenPyramid pyramid in GreenPyramidz) {
        SetBlockedAt(pyramid.OwnLocation, !pyramid.IsDown);
      }
      
      foreach (RedPyramid pyramid in RedPyramidz) {
        SetBlockedAt(pyramid.OwnLocation, !pyramid.IsDown);
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
