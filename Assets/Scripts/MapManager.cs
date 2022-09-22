using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {
    private static MapManager _instance;

    public static MapManager Instance {get => _instance;}

    public Tilemap baseMap;
    public NavTile navtilePrefab;
    public GameObject tileContainer;

    public Dictionary<Vector2Int, NavTile> map;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start() {
        Tilemap tilemap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, NavTile>();

        BoundsInt bounds = tilemap.cellBounds;

        for (int y = bounds.min.y; y < bounds.max.y; y++) {
            for (int x = bounds.min.x; x < bounds.max.x; x++) {
                Vector3Int tileLocation = new Vector3Int(x, y, 0);
                Vector2Int tileKey = new Vector2Int(x, y);

                if (baseMap.HasTile(tileLocation) && !map.ContainsKey(tileKey)) {
                    NavTile navTile = Instantiate(navtilePrefab, tileContainer.transform);
                    Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(tileLocation);

                    navTile.transform.position = CustomStuff.RoundMinusHalf(cellWorldPosition);
                    navTile.gridLocation = tileLocation;
                    
                    map.Add(tileKey, navTile);
                }
            }
        }
    }
}
