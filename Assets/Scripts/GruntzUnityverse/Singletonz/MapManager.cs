using System.Collections.Generic;
using System.Linq;

using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Bridgez;
using GruntzUnityverse.MapObjectz.Switchez;
using GruntzUnityverse.PathFinding;
using GruntzUnityverse.Utilitiez;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace GruntzUnityverse.Singletonz {
  public class MapManager : MonoBehaviour {
    private static MapManager _instance;

    public static MapManager Instance {
      get => _instance;
    }
  
    private void Awake() {
      if (_instance != null && _instance != this)
        Destroy(gameObject);
      else
        _instance = this;
    }

    public Tilemap baseMap;
    public Tilemap collisionMap;
    public Tilemap secretMap;
    public NavTile navtilePrefab;
    public GameObject tileContainer;
    public GameObject playerGruntz;
    public List<Grunt> gruntz;
    public List<Rock> rockz;
    public List<Arrow> arrowz;
    public List<SecretTile> secretTilez;

    public Dictionary<Vector2Int, NavTile> map;

    private void Start() {
      Application.targetFrameRate = 30;
      
      // Collect Gruntz
      foreach (Grunt grunt in playerGruntz.GetComponentsInChildren<Grunt>()) {
        gruntz.Add(grunt);
      }
      // Collect Rockz
      foreach (Rock rock in GameObject.Find("Rockz").GetComponentsInChildren<Rock>()) {
        rockz.Add(rock);
      }
      // Collect SecretTilez
      foreach (SecretTile secretTile in secretMap.GetComponentsInChildren<SecretTile>()) {
        secretTilez.Add(secretTile);
      }
      // Collect Arrowz
      foreach (Arrow arrow in baseMap.GetComponentsInChildren<Arrow>()) {
        arrowz.Add(arrow);
      }
      foreach (Arrow arrow in secretMap.GetComponentsInChildren<Arrow>()) {
        arrowz.Add(arrow);
      }

      map = new Dictionary<Vector2Int, NavTile>();
      BoundsInt bounds = baseMap.cellBounds;
    
      AddNavTilesBasedOnMap(bounds);
      AddNavTilesForSwitches();
      AddNavTilesForBridges();
      AddNavTilesForRocks();
    }

    private void AddNavTilesForSwitches() {
      AddNavTilesForBlueToggleSwitches();
      AddNavTilesForBlueHoldSwitches();
      AddNavTilesForToolCheckpointSwitches();
      AddNavTilesForToyCheckpointSwitches();
      AddNavTilesForSecretSwitches();
    }

    private void AddNavTilesForRocks() {
      Rock[] rocks = baseMap.GetComponentsInChildren<Rock>();

      foreach (Rock rock in rocks) {
        RemoveNavTileAt(rock.transform.position);
      }
    }
    
    private void AddNavTilesForPyramids() {

    }

    private void AddNavTilesForBridges() {
      AddNavTilesForStaticBridges();
    }
    
    public void AddNavTileAt(Vector3 position) {
      Vector3Int tileLocation = new(
        Mathf.FloorToInt(position.x),
        Mathf.FloorToInt(position.y),
        0
      );
      Vector2Int tileKey = new(
        tileLocation.x,
        tileLocation.y
      );

      if (map.ContainsKey(tileKey)) {
        return;
      }

      NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
      Vector3 cellWorldPosition = baseMap.GetCellCenterWorld(tileLocation);
        
      navTile.transform.position = Positioning.SetNavTilePosition(cellWorldPosition);
      navTile.gridLocation = tileLocation;
        
      map.Add(tileKey, navTile);
    }

    public void RemoveNavTileAt(Vector3 position) {
      Vector3Int tileLocation = new(
        Mathf.FloorToInt(position.x),
        Mathf.FloorToInt(position.y),
        0
      );
      Vector2Int tileKey = new(
        Mathf.FloorToInt(position.x),
        Mathf.FloorToInt(position.y)
      );

      if (!map.ContainsKey(tileKey)) {
        return;
      }

      map.Remove(tileKey);

      foreach (NavTile navTile in tileContainer
                .GetComponentsInChildren<NavTile>()
                .Where(navTile => navTile.transform.position.x == tileLocation.x + 0.5f
                                  && navTile.transform.position.y == tileLocation.y + 0.5f)
      ) {
        Destroy(navTile);
      }
    }
    
    private void AddNavTilesForStaticBridges() {
      WaterBridgeStatic[] staticWaterBridges = baseMap.GetComponentsInChildren<WaterBridgeStatic>();

      foreach (WaterBridgeStatic bridge in staticWaterBridges) {
        AddNavTileAt(bridge.transform.position);
      }
    }

    private void AddNavTilesForBlueToggleSwitches() {
      BlueToggleSwitch[] blueToggleSwitches = baseMap.GetComponentsInChildren<BlueToggleSwitch>();
    
      foreach (BlueToggleSwitch blueToggleSwitch in blueToggleSwitches) {
        AddNavTileAt(blueToggleSwitch.transform.position);
      }
    }
  
    private void AddNavTilesForBlueHoldSwitches() {
      BlueHoldSwitch[] blueHoldSwitches = baseMap.GetComponentsInChildren<BlueHoldSwitch>();
    
      foreach (BlueHoldSwitch blueHoldSwitch in blueHoldSwitches) {
        AddNavTileAt(blueHoldSwitch.transform.position);
      }
    }

    private void AddNavTilesForToolCheckpointSwitches() {
      CheckpointSwitchTool[] checkpointSwitches = baseMap.GetComponentsInChildren<CheckpointSwitchTool>();
    
      foreach (CheckpointSwitchTool checkpointSwitch in checkpointSwitches) {
        AddNavTileAt(checkpointSwitch.transform.position);
      }
    }

    private void AddNavTilesForToyCheckpointSwitches() {
      CheckpointSwitchToy[] checkpointSwitches = baseMap.GetComponentsInChildren<CheckpointSwitchToy>();
    
      foreach (CheckpointSwitchToy checkpointSwitch in checkpointSwitches) {
        AddNavTileAt(checkpointSwitch.transform.position);
      }
    }
    
    private void AddNavTilesForSecretSwitches() {
      SecretSwitch[] secretSwitches = baseMap.GetComponentsInChildren<SecretSwitch>();
    
      foreach (SecretSwitch secretSwitch in secretSwitches) {
        AddNavTileAt(secretSwitch.transform.position);
      }
    }
    
    private void AddNavTilesBasedOnMap(BoundsInt bounds) {
      for (int y = bounds.min.y; y < bounds.max.y; y++) {
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
          Vector3Int tileLocation = new(x, y, 0);
          Vector2Int tileKey = new(x, y);

          if (!baseMap.HasTile(tileLocation) || map.ContainsKey(tileKey)) {
            continue;
          }

          NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
          Vector3 cellWorldPosition = baseMap.GetCellCenterWorld(tileLocation);

          navTile.transform.position = Positioning.SetNavTilePosition(cellWorldPosition);
          navTile.gridLocation = tileLocation;

          map.Add(tileKey, navTile);
        }
      }
    }
  }
}
