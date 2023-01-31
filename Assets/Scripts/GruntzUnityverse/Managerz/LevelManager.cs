using System.Collections.Generic;
using System.Linq;
using _Test;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Brickz;
using GruntzUnityverse.MapObjectz.Bridgez;
using GruntzUnityverse.MapObjectz.Hazardz;
using GruntzUnityverse.MapObjectz.Switchez;
using GruntzUnityverse.PathFinding;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using BlueHoldSwitch = GruntzUnityverse.MapObjectz.Switchez.BlueHoldSwitch;
using BlueToggleSwitch = GruntzUnityverse.MapObjectz.Switchez.BlueToggleSwitch;
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
    public Tilemap baseMap;
    public Tilemap collisionMap;

    public GameObject playerGruntz;
    public List<Grunt> gruntz;

    
    public List<TGrunt> testGruntz;
    public List<TCheckpointPyramid> testCheckpointPyramids;
    
    
    // Colliding MapObjectz
    public List<Rock> rockz;
    public List<GiantRock> giantRockz;
    public List<Fort> fortz;

    // Non-colliding MapObjectz
    public List<Arrow> arrowz;
    public List<Spikez> spikezList;
    public List<SecretTile> secretTilez;
    public List<BrickFoundation> brickFoundationz;

    // Non-colliding MapObjectz - Bridgez
    public List<WaterBridgeStatic> staticWaterBridgez;
    public List<WaterBridge> waterBridgez;

    // Non-colliding MapObjectz - Switchez
    public List<BlueToggleSwitch> blueToggleSwitchez;
    public List<BlueHoldSwitch> blueHoldSwitchez;
    public List<CheckpointSwitchTool> toolCheckpointSwitchez;
    public List<CheckpointSwitchToy> toyCheckpointSwitchez;

    // CheckpointSwitches
    public List<SecretSwitch> secretSwitchez;

    public GameObject nodeContainer;
    public List<Node> mapNodes;
    public List<Vector2Int> mapNodeLocations;
    public Node nodePrefab;

    public HealthBar healthBarPrefab;
    public StaminaBar staminaBarPrefab;

    private void Start() {
      Application.targetFrameRate = 60;

      baseMap = GameObject.Find("BaseMap").GetComponent<Tilemap>();
      collisionMap = GameObject.Find("CollisionMap").GetComponent<Tilemap>();
      nodeContainer = GameObject.Find("NodeContainer");

      for (int x = 0; x < baseMap.cellBounds.xMax; x++) {
        for (int y = 0; y < baseMap.cellBounds.yMax; y++) {
          Node node = Instantiate(nodePrefab, nodeContainer.transform);
          node.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1);
          node.GridLocation = new Vector2Int(x, y);
          mapNodes.Add(node);
          mapNodeLocations.Add(node.GridLocation);

          if (!baseMap.HasTile(new Vector3Int(x, y, 0))) {
            node.isBlocked = true;
          }
        }
      }

      Debug.Log(mapNodes.Count);

      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();


      // Collect Gruntz
      foreach (Grunt grunt in playerGruntz.GetComponentsInChildren<Grunt>()) {
        gruntz.Add(grunt);
      }
      
      // Collect TestGruntz
      foreach (TGrunt grunt in playerGruntz.GetComponentsInChildren<TGrunt>()) {
        testGruntz.Add(grunt);
      }

      CollectAllMapObjectz();

      BlockCollidingObjectNodesByDefault();

      UnblockNonCollidingObjectNodesByDefault();
    }

    private void CollectAllMapObjectz() {
      foreach (TCheckpointPyramid checkpointPyramid in baseMap.GetComponentsInChildren<TCheckpointPyramid>()) {
        testCheckpointPyramids.Add(checkpointPyramid);
      }



      // Collect Arrowz
      foreach (Arrow arrow in baseMap.GetComponentsInChildren<Arrow>()) {
        arrowz.Add(arrow);
      }

      // Collect Spikez
      foreach (Spikez spikez in baseMap.GetComponentsInChildren<Spikez>()) {
        spikezList.Add(spikez);
      }

      // Collect BrickFoundationz
      foreach (BrickFoundation brickFoundation in baseMap.GetComponentsInChildren<BrickFoundation>()) {
        brickFoundationz.Add(brickFoundation);
      }

      // Collect Rockz
      foreach (Rock rock in GameObject.Find("Rocks").GetComponentsInChildren<Rock>()) {
        rockz.Add(rock);
      }

      // Collect GiantRockz
      foreach (GiantRock giantRock in GameObject.Find("GiantRocks").GetComponentsInChildren<GiantRock>()) {
        giantRockz.Add(giantRock);
      }

      // Collect BlueHoldSwitchez
      foreach (BlueHoldSwitch blueHoldSwitch in baseMap.GetComponentsInChildren<BlueHoldSwitch>()) {
        blueHoldSwitchez.Add(blueHoldSwitch);
      }

      // Collect BlueToggleSwitchez
      foreach (BlueToggleSwitch blueToggleSwitch in baseMap.GetComponentsInChildren<BlueToggleSwitch>()) {
        blueToggleSwitchez.Add(blueToggleSwitch);
      }

      // Collect CheckpointSwitchez
      foreach (CheckpointSwitchTool checkpointSwitchTool in baseMap.GetComponentsInChildren<CheckpointSwitchTool>()) {
        toolCheckpointSwitchez.Add(checkpointSwitchTool);
      }

      foreach (CheckpointSwitchToy checkpointSwitchToy in baseMap.GetComponentsInChildren<CheckpointSwitchToy>()) {
        toyCheckpointSwitchez.Add(checkpointSwitchToy);
      }

      // Collect SecretSwitchez
      foreach (SecretSwitch secretSwitch in baseMap.GetComponentsInChildren<SecretSwitch>()) {
        secretSwitchez.Add(secretSwitch);
      }

      // Collect static WaterBridgez
      foreach (WaterBridgeStatic waterBridgeStatic in baseMap.GetComponentsInChildren<WaterBridgeStatic>()) {
        staticWaterBridgez.Add(waterBridgeStatic);
      }

      // Collect WaterBridgez
      foreach (WaterBridge waterBridge in baseMap.GetComponentsInChildren<WaterBridge>()) {
        waterBridgez.Add(waterBridge);
      }
    }

    private void BlockCollidingObjectNodesByDefault() {
      foreach (TCheckpointPyramid checkpointPyramid in testCheckpointPyramids) {
        SetBlockedAt(checkpointPyramid.OwnLocation, !checkpointPyramid.IsDown);
      }

      foreach (Rock rock in rockz) {
        BlockNodeAt(rock.GridLocation);
      }

      foreach (GiantRock giantRock in giantRockz) {
        BlockNodeAt(giantRock.GridLocation);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.up);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.down);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.right);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.left);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.up + Vector2Int.right);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.up + Vector2Int.left);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.down + Vector2Int.right);
        BlockNodeAt(giantRock.GridLocation + Vector2Int.down + Vector2Int.left);
      }

      foreach (Fort fort in fortz) {
        BlockNodeAt(fort.GridLocation);
        BlockNodeAt(fort.GridLocation + Vector2Int.up);
        BlockNodeAt(fort.GridLocation + Vector2Int.down);
        BlockNodeAt(fort.GridLocation + Vector2Int.right);
        BlockNodeAt(fort.GridLocation + Vector2Int.left);
        BlockNodeAt(fort.GridLocation + Vector2Int.up + Vector2Int.right);
        BlockNodeAt(fort.GridLocation + Vector2Int.up + Vector2Int.left);
        BlockNodeAt(fort.GridLocation + Vector2Int.down + Vector2Int.right);
        BlockNodeAt(fort.GridLocation + Vector2Int.down + Vector2Int.left);
      }
    }

    private void UnblockNonCollidingObjectNodesByDefault() {
      // Underwater Arrowz?
      foreach (Arrow arrow in arrowz) {
        FreeNodeAt(arrow.GridLocation);
      }

      // Underwater Spikez?
      foreach (Spikez spikez in spikezList) {
        FreeNodeAt(spikez.GridLocation);
      }

      foreach (BrickFoundation brickFoundation in brickFoundationz) {
        FreeNodeAt(brickFoundation.GridLocation);
      }

      foreach (BlueHoldSwitch blueHoldSwitch in blueHoldSwitchez) {
        FreeNodeAt(blueHoldSwitch.GridLocation);
      }

      foreach (BlueToggleSwitch blueToggleSwitch in blueToggleSwitchez) {
        FreeNodeAt(blueToggleSwitch.GridLocation);
      }

      foreach (CheckpointSwitchTool checkpointSwitch in toolCheckpointSwitchez) {
        FreeNodeAt(checkpointSwitch.GridLocation);
      }

      foreach (CheckpointSwitchToy checkpointSwitch in toyCheckpointSwitchez) {
        FreeNodeAt(checkpointSwitch.GridLocation);
      }

      foreach (SecretSwitch secretSwitch in secretSwitchez) {
        FreeNodeAt(secretSwitch.GridLocation);
      }

      foreach (WaterBridgeStatic staticWaterBridge in staticWaterBridgez) {
        FreeNodeAt(staticWaterBridge.GridLocation);
      }
    }

    public void SetBlockedAt(Vector2Int gridLocation, bool isBlocked) {
      mapNodes.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = isBlocked;
    }
    
    public void BlockNodeAt(Vector2Int gridLocation) {
      mapNodes.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = true;
    }

    public void FreeNodeAt(Vector2Int gridLocation) {
      mapNodes.First(node => node.GridLocation.Equals(gridLocation)).isBlocked = false;
    }

    public Node GetNodeAt(Vector2Int gridLocation) {
      return mapNodes.First(node => node.GridLocation.Equals(gridLocation));
    }

    public bool IsBlockedAt(Vector2Int gridLocation) {
      return mapNodes.First(node => node.GridLocation.Equals(gridLocation)).isBlocked;
    }
  }
}
