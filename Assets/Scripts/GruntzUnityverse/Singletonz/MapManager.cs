using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Bridgez;
using GruntzUnityverse.MapObjectz.Switchez;
using GruntzUnityverse.PathFinding;
using GruntzUnityverse.Utilitiez;
using TMPro;
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

    public TMP_Text helpBoxText;
    public Tilemap baseMap;
    public Tilemap collisionMap;
    public Tilemap secretMap;
    public NavTile navtilePrefab;
    public GameObject tileContainer;
    public GameObject playerGruntz;
    public List<Grunt> gruntz;
    public List<Rock> rockz;
    public List<GiantRock> giantRockz;
    public List<Arrow> arrowz;
    public List<SecretTile> secretTilez;
    public List<BrickFoundation> brickFoundationz;

    public Dictionary<Vector2Int, NavTile> map;

    private void Start() {
      Application.targetFrameRate = 60;

      helpBoxText = GameObject.Find("ScrollBox").GetComponentInChildren<TMP_Text>();

      // Collect Gruntz
      foreach (Grunt grunt in playerGruntz.GetComponentsInChildren<Grunt>()) {
        gruntz.Add(grunt);
      }

      // Collect Rockz
      foreach (Rock rock in GameObject.Find("Rockz").GetComponentsInChildren<Rock>()) {
        rockz.Add(rock);
      }

      // Collect GiantRockz
      foreach (GiantRock giantRock in GameObject.Find("GiantRockz").GetComponentsInChildren<GiantRock>()) {
        giantRockz.Add(giantRock);
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

      // Collect BrickFoundationz
      foreach (BrickFoundation brickFoundation in baseMap.GetComponentsInChildren<BrickFoundation>()) {
        brickFoundationz.Add(brickFoundation);
      }

      map = new Dictionary<Vector2Int, NavTile>();
      BoundsInt bounds = baseMap.cellBounds;

      AddNavTilesBasedOnMap(bounds);
      AddNavTilesForSwitches();
      AddNavTilesForBridges();
      AddNavTilesForBrickFoundations();

      RemoveNavTilesForRocks();
      RemoveNavTilesForGiantRocks();
      RemoveNavTilesForFort();
    }

    private void AddNavTilesForSwitches() {
      AddNavTilesForBlueToggleSwitches();
      AddNavTilesForBlueHoldSwitches();
      AddNavTilesForToolCheckpointSwitches();
      AddNavTilesForToyCheckpointSwitches();
      AddNavTilesForSecretSwitches();
    }

    private void RemoveNavTilesForRocks() {
      foreach (Rock rock in rockz) {
        RemoveNavTileAt(rock.transform.position);
      }
    }

    private void RemoveNavTilesForGiantRocks() {
      foreach (Vector3 giantRockPosition in giantRockz.Select(giantRock => giantRock.transform.position)) {
        RemoveNavTileAt(giantRockPosition);
        RemoveNavTileAt(giantRockPosition + Vector3.up);
        RemoveNavTileAt(giantRockPosition + Vector3.down);
        RemoveNavTileAt(giantRockPosition + Vector3.left);
        RemoveNavTileAt(giantRockPosition + Vector3.right);
        RemoveNavTileAt(giantRockPosition + Vector3Plus.upleft);
        RemoveNavTileAt(giantRockPosition + Vector3Plus.upright);
        RemoveNavTileAt(giantRockPosition + Vector3Plus.downleft);
        RemoveNavTileAt(giantRockPosition + Vector3Plus.downright);
      }
    }

    private void RemoveNavTilesForPyramids() {}

    private void RemoveNavTilesForFort() {
      Vector3 fortPosition = baseMap.GetComponentInChildren<Fort>().transform.position;

      RemoveNavTileAt(fortPosition);
      RemoveNavTileAt(fortPosition + Vector3.up);
      RemoveNavTileAt(fortPosition + Vector3.down);
      RemoveNavTileAt(fortPosition + Vector3.left);
      RemoveNavTileAt(fortPosition + Vector3.right);
      RemoveNavTileAt(fortPosition + Vector3Plus.upleft);
      RemoveNavTileAt(fortPosition + Vector3Plus.upright);
      RemoveNavTileAt(fortPosition + Vector3Plus.downleft);
      RemoveNavTileAt(fortPosition + Vector3Plus.downright);
    }

    private void AddNavTilesForBridges() {
      AddNavTilesForStaticBridges();
    }

    private void AddNavTilesForBrickFoundations() {
      foreach (BrickFoundation brickFoundation in brickFoundationz) {
        AddNavTileAt(brickFoundation.transform.position);
      }
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

      foreach (
        NavTile navTile in tileContainer
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
