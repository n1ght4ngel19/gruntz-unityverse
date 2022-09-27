using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class WaterBridge : MonoBehaviour {
  public BlueToggleSwitch controllerSwitch;
  public List<Sprite> animFrames;

  private void Update() {
    HandleBlueToggleSwitch();
    // HandleBlueHoldSwitch();
  }

  private void HandleBlueHoldSwitch() {
    switch (controllerSwitch.isPressed) {
      case true: {
        Debug.Log("I'M PRESSED");
      
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

        break;
      }
      case false: {
        Debug.Log("I AIN'T BEIN' PRESSED");
      
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
                   .Where(navTile => navTile.transform.position.x == tileLocation.x + 0.5f
                                     && navTile.transform.position.y == tileLocation.y + 0.5f)
        ) {
          Destroy(navTile);
        }

        break;
      }
    }    
  }
  
  private void HandleBlueToggleSwitch() {
    switch (controllerSwitch.isPressed) {
      case true: {
        Debug.Log("I'M PRESSED");
      
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
          gameObject.GetComponent<SpriteRenderer>().sprite = animFrames[i];
        }

        break;
      }
      case false: {
        Debug.Log("I AIN'T BEIN' PRESSED");
      
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
                   .Where(navTile => navTile.transform.position.x == tileLocation.x + 0.5f
                                     && navTile.transform.position.y == tileLocation.y + 0.5f)
        ) {
          Destroy(navTile);
        }

        foreach (Sprite frame in animFrames) {
          gameObject.GetComponent<SpriteRenderer>().sprite = frame;
        }

        break;
      }
    }
  }
}
