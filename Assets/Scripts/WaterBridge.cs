using System.Collections.Generic;
using System.Linq;

using Singletonz;

using Switchez;

using UnityEngine;

public class WaterBridge : MonoBehaviour {
  // You can only assign a BTS or a BHS to a WaterBridge
  public BlueToggleSwitch blueToggleSwitch;
  // You can only assign a BTS or a BHS to a WaterBridge
  public BlueHoldSwitch blueHoldSwitch;
  public List<Sprite> animFrames;

  private void Update() {
    if (blueToggleSwitch) {
      HandleBlueSwitch(blueToggleSwitch.isPressed);
    } else if (blueHoldSwitch) {
      HandleBlueSwitch(blueHoldSwitch.isPressed);
    }
  }

  private void HandleBlueSwitch(bool isPressed) {
    switch (isPressed) {
      case true: {
        Vector3Int tileLocation = new(
          Mathf.FloorToInt(transform.position.x),
          Mathf.FloorToInt(transform.position.y),
          0
        );
        Vector2Int tileKey = new(
          Mathf.FloorToInt(transform.position.x),
          Mathf.FloorToInt(transform.position.y)
        );

        if (MapManager.Instance.map.ContainsKey(tileKey)) {
          return;
        }

        NavTile navTile = Instantiate(MapManager.Instance.navtilePrefab, MapManager.Instance.tileContainer.transform);
        Vector3 cellWorldPosition = MapManager.Instance.baseMap.GetCellCenterWorld(tileLocation);

        navTile.transform.position = CustomStuff.SetNavTilePosition(cellWorldPosition);
        navTile.gridLocation = tileLocation;

        MapManager.Instance.map.Add(tileKey, navTile);

        for (int i = animFrames.Count - 1; i >= 0; i--) {
          gameObject.GetComponent<SpriteRenderer>()
            .sprite = animFrames[i];
        }

        break;
      }
      case false: {
        Vector3Int tileLocation = new(
          Mathf.FloorToInt(transform.position.x),
          Mathf.FloorToInt(transform.position.y),
          0
        );
        Vector2Int tileKey = new(
          Mathf.FloorToInt(transform.position.x),
          Mathf.FloorToInt(transform.position.y)
        );

        if (!MapManager.Instance.map.ContainsKey(tileKey)) {
          return;
        }

        MapManager.Instance.map.Remove(tileKey);

        foreach (NavTile navTile in MapManager.Instance.tileContainer
                   .GetComponentsInChildren<NavTile>()
                   .Where(
                     navTile => navTile.transform.position.x == tileLocation.x + 0.5f
                                && navTile.transform.position.y == tileLocation.y + 0.5f
                   )
                ) {
          Destroy(navTile);
        }

        foreach (Sprite frame in animFrames) {
          gameObject.GetComponent<SpriteRenderer>()
            .sprite = frame;
        }

        break;
      }
    }
  }
}
