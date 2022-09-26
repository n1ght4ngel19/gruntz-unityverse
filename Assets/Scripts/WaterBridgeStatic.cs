using UnityEngine;

public class WaterBridgeStatic : MonoBehaviour {
  private Vector3Int tileLocation;
  private Vector2Int tileKey;

  // private void Start() {
  //   tileLocation = new Vector3Int(
  //     Mathf.FloorToInt(transform.position.x),
  //     Mathf.FloorToInt(transform.position.y),
  //     0
  //   );
  //   tileKey = new Vector2Int(
  //     Mathf.FloorToInt(transform.position.x),
  //     Mathf.FloorToInt(transform.position.y)
  //   );
  //   
  //   if (MapManager.Instance.map.ContainsKey(tileKey)) {
  //     return;
  //   }
  //     
  //   NavTile navTile = Instantiate(MapManager.Instance.navtilePrefab, MapManager.Instance.tileContainer.transform);
  //   Vector3 cellWorldPosition = MapManager.Instance.baseMap.GetCellCenterWorld(tileLocation);
  //     
  //   navTile.transform.position = CustomStuff.SetNavTilePosition(cellWorldPosition);
  //   navTile.gridLocation = tileLocation;
  //     
  //   MapManager.Instance.map.Add(tileKey, navTile);
  // }
}
