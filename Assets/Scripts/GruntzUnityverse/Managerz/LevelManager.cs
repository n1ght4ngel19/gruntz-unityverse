using System.Collections.Generic;
using System.Linq;
using _Test;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;

namespace GruntzUnityverse.Managerz {
  public class LevelManager : MonoBehaviour {
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

    public TMP_Text helpBoxText;

    public Tilemap baseLayer;
    public Tilemap collisionLayer;

    public List<Grunt> gruntz;

    public List<TGrunt> testGruntz;
    public List<TCheckpointPyramid> testCheckpointPyramids;
    
    
    // Colliding MapObjectz
    public List<Rock> rockz;

    // Non-colliding MapObjectz
    public List<Spikez> spikezList;

    public GameObject pathfindingNodes;
    public List<Node> nodesList;
    public List<Vector2Int> nodeLocationsList;
    public Node nodePrefab;

    public HealthBar healthBarPrefab;
    public StaminaBar staminaBarPrefab;

    private void Start() {
      Application.targetFrameRate = 60;

      baseLayer = GameObject.Find("BaseLayer").GetComponent<Tilemap>();
      collisionLayer = GameObject.Find("CollisionLayer").GetComponent<Tilemap>();
      pathfindingNodes = GameObject.Find("PathfindingNodes");

      for (int x = 0; x < baseLayer.cellBounds.xMax; x++) {
        for (int y = 0; y < baseLayer.cellBounds.yMax; y++) {
          Node node = Instantiate(nodePrefab, pathfindingNodes.transform);
          node.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1);
          node.GridLocation = new Vector2Int(x, y);
          nodesList.Add(node);
          nodeLocationsList.Add(node.GridLocation);

          if (!baseLayer.HasTile(new Vector3Int(x, y, 0))) {
            node.isBlocked = true;
          }
        }
      }

      Debug.Log(nodesList.Count);

      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();

      // Collect TestGruntz
      foreach (TGrunt grunt in GameObject.Find("PlayerGruntz").GetComponentsInChildren<TGrunt>()) {
        testGruntz.Add(grunt);
      }

      CollectAllMapObjectz();

      BlockCollidingObjectNodesByDefault();

      UnblockNonCollidingObjectNodesByDefault();
    }

    private void CollectAllMapObjectz() {
      foreach (TCheckpointPyramid checkpointPyramid in baseLayer.GetComponentsInChildren<TCheckpointPyramid>()) {
        testCheckpointPyramids.Add(checkpointPyramid);
      }

      // Collect Spikez
      foreach (Spikez spikez in baseLayer.GetComponentsInChildren<Spikez>()) {
        spikezList.Add(spikez);
      }

      // Collect Rockz
      foreach (Rock rock in GameObject.Find("Rocks").GetComponentsInChildren<Rock>()) {
        rockz.Add(rock);
      }
    }

    private void BlockCollidingObjectNodesByDefault() {
      foreach (TCheckpointPyramid checkpointPyramid in testCheckpointPyramids) {
        SetBlockedAt(checkpointPyramid.OwnLocation, !checkpointPyramid.IsDown);
      }

      foreach (Rock rock in rockz) {
        BlockNodeAt(rock.OwnLocation);
      }
    }

    private void UnblockNonCollidingObjectNodesByDefault() {
      foreach (Spikez spikez in spikezList) {
        FreeNodeAt(spikez.GridLocation);
      }
    }

    #region Node Methods
      public void SetBlockedAt(Vector2Int gridLocation, bool isBlocked) {
        nodesList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = isBlocked;
      }
      
      public void BlockNodeAt(Vector2Int gridLocation) {
        nodesList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = true;
      }

      public void FreeNodeAt(Vector2Int gridLocation) {
        nodesList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = false;
      }

      public bool IsBlockedAt(Vector2Int gridLocation) {
        return nodesList.First(node => node.GridLocation.Equals(gridLocation)).isBlocked;
      }

      public Node GetNodeAt(Vector2Int gridLocation) {
        return nodesList.First(node => node.GridLocation.Equals(gridLocation));
      }
    #endregion
  }
}
