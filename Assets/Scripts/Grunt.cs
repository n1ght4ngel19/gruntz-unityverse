using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Grunt : MonoBehaviour {
  public SpriteRenderer spriteRenderer;

  public List<Sprite> walkSpritesNorth;
  public List<Sprite> walkSpritesNorthEast;
  public List<Sprite> walkSpritesNorthWest;
  public List<Sprite> walkSpritesSouth;
  public List<Sprite> walkSpritesSouthEast;
  public List<Sprite> walkSpritesSouthWest;
  public List<Sprite> walkSpritesEast;
  public List<Sprite> walkSpritesWest;

  public List<Sprite> idleSpritesNorth;
  public List<Sprite> idleSpritesNorthEast;
  public List<Sprite> idleSpritesNorthWest;
  public List<Sprite> idleSpritesSouth;
  public List<Sprite> idleSpritesSouthEast;
  public List<Sprite> idleSpritesSouthWest;
  public List<Sprite> idleSpritesEast;
  public List<Sprite> idleSpritesWest;
  
  public List<Sprite> attackSpritesNorth;
  public List<Sprite> attackSpritesNorthEast;
  public List<Sprite> attackSpritesNorthWest;
  public List<Sprite> attackSpritesSouth;
  public List<Sprite> attackSpritesSouthEast;
  public List<Sprite> attackSpritesSouthWest;
  public List<Sprite> attackSpritesEast;
  public List<Sprite> attackSpritesWest;

  public List<Sprite> struckSpritesNorth;
  public List<Sprite> struckSpritesNorthEast;
  public List<Sprite> struckSpritesNorthWest;
  public List<Sprite> struckSpritesSouth;
  public List<Sprite> struckSpritesSouthEast;
  public List<Sprite> struckSpritesSouthWest;
  public List<Sprite> struckSpritesEast;
  public List<Sprite> struckSpritesWest;
  
  public List<Sprite> itemSpritesNorth;
  public List<Sprite> itemSpritesNorthEast;
  public List<Sprite> itemSpritesNorthWest;
  public List<Sprite> itemSpritesSouth;
  public List<Sprite> itemSpritesSouthEast;
  public List<Sprite> itemSpritesSouthWest;
  public List<Sprite> itemSpritesEast;
  public List<Sprite> itemSpritesWest;

  private static List<Grunt> gruntz = new();

  // TODO: 10f / 6f
  private const float TimeToMove = 5f;
  private Vector3 targetPosition;
  private bool isSelected;
  private bool isMoving;
  private bool hasMoved;

  private Vector3 diffVector;
  private Vector3 moveVector;
  private string facingDirection = "SOUTH";

  private const float WalkFrameRate = 12;
  private const float IdleFrameRate = 8;
  private float idleTime;

  private List<NavTile> path;
  private NavTile startNode;
  private NavTile endNode;

  private void Start() {
    gruntz.Add(this);
    targetPosition = transform.position;
  }

  private void Update() {
    PlaySouthIdleAnimationByDefault();
    
    if (Input.GetMouseButtonDown(1) && isSelected) {
      isMoving = true;
      targetPosition = SelectorCircle.Instance.transform.position;
      
      targetPosition.z = -5;

      if (!hasMoved)
        hasMoved = true;
    }

    Vector2Int startKey = new(
      (int)Mathf.Floor(transform.position.x),
      (int)Mathf.Floor(transform.position.y)
    );

    startNode = MapManager.Instance.map.ContainsKey(startKey)
      ? MapManager.Instance.map[startKey]
      : MapManager.Instance.map[startKey + Vector2Int.up];

    Vector2Int endKey = new(
      (int)Mathf.Floor(targetPosition.x),
      (int)Mathf.Floor(targetPosition.y)
    );

    endNode = MapManager.Instance.map.ContainsKey(endKey)
      ? MapManager.Instance.map[endKey]
      : MapManager.Instance.map[endKey + Vector2Int.up];

    path = PathFinder.FindPath(startNode, endNode);

    Vector3 destination = new(
      Mathf.Floor(path[0].gridLocation.x) + 0.5f,
      Mathf.Floor(path[0].gridLocation.y) + 0.5f,
      -5
    );

    transform.position = destination;

    path.RemoveAt(0);

    PlayWalkAndIdleAnimations();
  }

  private void OnMouseDown() {
    isSelected = true;

    foreach (Grunt grunt in gruntz.Where(grunt => grunt != this)) {
      grunt.isSelected = false;
    }
  }

  private List<Sprite> GetWalkSprites() {
    List<Sprite> selectedSprites = null;

    switch (diffVector.y) {
      case < 0: {
        selectedSprites = walkSpritesSouth;
        facingDirection = "SOUTH";

        switch (diffVector.x) {
          case > 0: {
            selectedSprites = walkSpritesSouthEast;
            facingDirection = "SOUTHEAST";
            break;
          }
          case < 0: {
            selectedSprites = walkSpritesSouthWest;
            facingDirection = "SOUTHWEST";
            break;
          }
        }
        break;
      }
      case > 0: {
        selectedSprites = walkSpritesNorth;
        facingDirection = "NORTH";

        switch (diffVector.x) {
          case > 0: {
            selectedSprites = walkSpritesNorthEast;
            facingDirection = "NORTHEAST";
            break;
          }
          case < 0: {
            selectedSprites = walkSpritesNorthWest;
            facingDirection = "NORTHWEST";
            break;
          }
        }
        break;
      }
      default: {
        switch (diffVector.x) {
          case > 0: {
            selectedSprites = walkSpritesEast;
            facingDirection = "EAST";
            break;
          }
          case < 0: {
            selectedSprites = walkSpritesWest;
            facingDirection = "WEST";
            break;
          }
        }
        break;
      }
    }

    return selectedSprites;
  }

  private List<Sprite> GetIdleSprites() {
    List<Sprite> selectedSprites = facingDirection switch {
      "NORTH" => idleSpritesNorth,
      "NORTHEAST" => idleSpritesNorthEast,
      "NORTHWEST" => idleSpritesNorthWest,
      "SOUTH" => idleSpritesSouth,
      "SOUTHEAST" => idleSpritesSouthEast,
      "SOUTHWEST" => idleSpritesSouthWest,
      "EAST" => idleSpritesEast,
      "WEST" => idleSpritesWest,
      _ => null
    };

    return selectedSprites;
  }

  private void PlayWalkAndIdleAnimations() {
    if (isMoving) {
      List<Sprite> walkSprites = GetWalkSprites();

      if (walkSprites != null) {
        float playTime = Time.time - idleTime;
        int frame = (int)(playTime * WalkFrameRate % walkSprites.Count);

        spriteRenderer.sprite = walkSprites[frame];
      }
      else {
        isMoving = false;
        idleTime = Time.time;
      }
    } else {
      List<Sprite> idleSprites = GetIdleSprites();

      if (idleSprites != null) {
        float playTime = Time.time - idleTime;
        int frame = (int)(playTime * IdleFrameRate % idleSprites.Count);

        spriteRenderer.sprite = idleSprites[frame];
      } else {
        idleTime = Time.time;
      }
    }
  }

  private void PlaySouthIdleAnimationByDefault() {
    if (hasMoved)
      return;

    float playTime = Time.time - idleTime;
    int frame = (int)((playTime * IdleFrameRate) % idleSpritesSouth.Count);

    spriteRenderer.sprite = idleSpritesSouth[frame];
  }
}
