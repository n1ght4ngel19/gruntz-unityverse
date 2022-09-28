using System.Collections.Generic;

using Singletonz;

using Switchez;

using UnityEngine;

namespace Pyramidz {
  public class CheckpointPyramid : MonoBehaviour {
    public CheckpointSwitch checkpointSwitch;
    public List<Sprite> animFrames;
    public SpriteRenderer spriteRenderer;

    private void Update() {
      if (!checkpointSwitch.isChecked) {
        return;
      }

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

      foreach (Sprite frame in animFrames) {
        spriteRenderer.sprite = frame;
      }
    }
  }
}
