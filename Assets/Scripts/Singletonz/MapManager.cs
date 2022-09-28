using System.Collections.Generic;

using Bridgez;

using Switchez;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Singletonz {
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
    public NavTile navtilePrefab;
    public GameObject tileContainer;
    public List<Grunt> gruntz;

    public Dictionary<Vector2Int, NavTile> map;

    private void Start() {
      foreach (Grunt grunt in GameObject.Find("PlayerGruntz").GetComponentsInChildren<Grunt>()) {
        gruntz.Add(grunt);
      }
    
      map = new Dictionary<Vector2Int, NavTile>();
      BoundsInt bounds = baseMap.cellBounds;
    
      AddNavTilesBasedOnMap(bounds);
      AddNavTilesForStaticBridges();
      AddNavTilesForBlueToggleSwitches();
      AddNavTilesForBlueHoldSwitches();
      AddNavTilesForCheckpointSwitches();
    }

    // TODO: Refactor this! Only check if changed, make bridgez and otherz play animationz instead of iterating through arrayz and setting framez there
    private void Update() {

    }

    private void AddNavTilesForStaticBridges() {
      WaterBridgeStatic[] staticWaterBridges = baseMap.GetComponentsInChildren<WaterBridgeStatic>();
    
      foreach (WaterBridgeStatic bridge in staticWaterBridges) {
        Vector3Int tileLocation = new(Mathf.FloorToInt(bridge.transform.position.x), Mathf.FloorToInt(bridge.transform.position.y), 0);
        Vector2Int tileKey = new(Mathf.FloorToInt(bridge.transform.position.x), Mathf.FloorToInt(bridge.transform.position.y));

        if (map.ContainsKey(tileKey))
          continue;

        NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
        Vector3 cellWorldPosition = baseMap.GetCellCenterWorld(tileLocation);
        
        navTile.transform.position = CustomStuff.SetNavTilePosition(cellWorldPosition);
        navTile.gridLocation = tileLocation;
        
        map.Add(tileKey, navTile);
      }
    }

    private void AddNavTilesForBlueToggleSwitches() {
      BlueToggleSwitch[] blueToggleSwitches = baseMap.GetComponentsInChildren<BlueToggleSwitch>();
    
      foreach (BlueToggleSwitch blueToggleSwitch in blueToggleSwitches) {
        Vector3Int tileLocation = new(
          Mathf.FloorToInt(blueToggleSwitch.transform.position.x),
          Mathf.FloorToInt(blueToggleSwitch.transform.position.y),
          0
        );
        Vector2Int tileKey = new(
          Mathf.FloorToInt(blueToggleSwitch.transform.position.x),
          Mathf.FloorToInt(blueToggleSwitch.transform.position.y)
        );

        if (map.ContainsKey(tileKey))
          continue;

        NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
        Vector3 cellWorldPosition = baseMap.GetCellCenterWorld(tileLocation);
        
        navTile.transform.position = CustomStuff.SetNavTilePosition(cellWorldPosition);
        navTile.gridLocation = tileLocation;
        
        map.Add(tileKey, navTile);
      }
    }
  
    private void AddNavTilesForBlueHoldSwitches() {
      BlueHoldSwitch[] blueToggleSwitches = baseMap.GetComponentsInChildren<BlueHoldSwitch>();
    
      foreach (BlueHoldSwitch blueHoldSwitch in blueToggleSwitches) {
        Vector3Int tileLocation = new(
          Mathf.FloorToInt(blueHoldSwitch.transform.position.x),
          Mathf.FloorToInt(blueHoldSwitch.transform.position.y),
          0
        );
        Vector2Int tileKey = new(
          Mathf.FloorToInt(blueHoldSwitch.transform.position.x),
          Mathf.FloorToInt(blueHoldSwitch.transform.position.y)
        );

        if (map.ContainsKey(tileKey))
          continue;

        NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
        Vector3 cellWorldPosition = baseMap.GetCellCenterWorld(tileLocation);
        
        navTile.transform.position = CustomStuff.SetNavTilePosition(cellWorldPosition);
        navTile.gridLocation = tileLocation;
        
        map.Add(tileKey, navTile);
      }
    }

    private void AddNavTilesBasedOnMap(BoundsInt bounds) {
      for (int y = bounds.min.y; y < bounds.max.y; y++) {
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
          Vector3Int tileLocation = new(x, y, 0);
          Vector2Int tileKey = new(x, y);

          if (!baseMap.HasTile(tileLocation) || map.ContainsKey(tileKey))
            continue;

          NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
          Vector3 cellWorldPosition = baseMap.GetCellCenterWorld(tileLocation);

          navTile.transform.position = CustomStuff.SetNavTilePosition(cellWorldPosition);
          navTile.gridLocation = tileLocation;

          map.Add(tileKey, navTile);
        }
      }
    }
    
    private void AddNavTilesForCheckpointSwitches() {
      CheckpointSwitch[] checkpointSwitches = baseMap.GetComponentsInChildren<CheckpointSwitch>();
    
      foreach (CheckpointSwitch checkpointSwitch in checkpointSwitches) {
        Vector3Int tileLocation = new(
          Mathf.FloorToInt(checkpointSwitch.transform.position.x),
          Mathf.FloorToInt(checkpointSwitch.transform.position.y),
          0
        );
        Vector2Int tileKey = new(
          Mathf.FloorToInt(checkpointSwitch.transform.position.x),
          Mathf.FloorToInt(checkpointSwitch.transform.position.y)
        );

        if (map.ContainsKey(tileKey))
          continue;

        NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
        Vector3 cellWorldPosition = baseMap.GetCellCenterWorld(tileLocation);
        
        navTile.transform.position = CustomStuff.SetNavTilePosition(cellWorldPosition);
        navTile.gridLocation = tileLocation;
        
        map.Add(tileKey, navTile);
      }
    }
  }
}
