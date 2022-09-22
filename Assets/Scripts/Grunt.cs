using System.Collections.Generic;

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

    public static List<Grunt> gruntz = new List<Grunt>();
    
    // TODO: 10f / 6f
    private float timeToMove = 5f;
    private Vector3 targetPosition;
    private bool isSelected;
    private bool isMoving;
    private bool hasMoved;

    private Vector3 diffVector;
    private Vector3 moveVector;
    private string facingDirection = "SOUTH";

    private float walkFrameRate = 12;
    private float idleFrameRate = 8;
    private float idleTime;

    private PathFinder pathFinder;
    private List<NavTile> path;
    private NavTile start;
    private NavTile end;

    public SelectorCircle selectorCircle;
    
    private void Start() {
        gruntz.Add(this);
        targetPosition = transform.position;

        pathFinder = new PathFinder();
    }

    void Update() {
        if (!hasMoved) {
            float playTime = Time.time - idleTime;
            int frame = (int)((playTime * idleFrameRate) % idleSpritesSouth.Count);
                
            spriteRenderer.sprite = idleSpritesSouth[frame];
        }
        
        if (Input.GetMouseButtonDown(1) && isSelected) {
            isMoving = true;
            targetPosition = selectorCircle.transform.position;
            targetPosition.z = transform.position.z;

            if (!hasMoved)
                hasMoved = true;
        }
        
        var startKey = new Vector2Int(
            (int)Mathf.Floor(transform.position.x),
            (int)Mathf.Floor(transform.position.y)
        );
        
        if (MapManager.Instance.map.ContainsKey(startKey)) {
            start = MapManager.Instance.map[startKey];            
        } else {
            start = MapManager.Instance.map[startKey + Vector2Int.up];
        }

        var endKey = new Vector2Int(
            (int)Mathf.Floor(targetPosition.x),
            (int)Mathf.Floor(targetPosition.y)
        );

        if (MapManager.Instance.map.ContainsKey(endKey)) {
            end = MapManager.Instance.map[endKey];
        } else {
            end = MapManager.Instance.map[endKey + Vector2Int.up];
        }
        
        path = pathFinder.FindPath(start, end);

        var destination = new Vector2(path[0].gridLocation.x, path[0].gridLocation.y);
        
        transform.position = Vector2.MoveTowards(
            transform.position,
            destination,
            timeToMove * Time.deltaTime
        );

        path.RemoveAt(0);
        
        if (isMoving) {
            List<Sprite> walkSprites = GetWalkSprites();

            if (walkSprites != null) {
                float playTime = Time.time - idleTime;
                int frame = (int)((playTime * walkFrameRate) % walkSprites.Count);
                
                spriteRenderer.sprite = walkSprites[frame];
            } else {
                isMoving = false;
                idleTime = Time.time;
            }
        } else {
            List<Sprite> idleSprites = GetIdleSprites();
            
            if (idleSprites != null) {
                float playTime = Time.time - idleTime;
                int frame = (int)((playTime * idleFrameRate) % idleSprites.Count);
                
                spriteRenderer.sprite = idleSprites[frame];
            } else {
                idleTime = Time.time;
            }
        }
    }

    private void OnMouseDown() {
        isSelected = true;

        foreach (Grunt grunt in gruntz) {
            if (grunt != this)
                grunt.isSelected = false;
        }
    }

    private List<Sprite> GetWalkSprites() {
        List<Sprite> selectedSprites = null;

        if (diffVector.y < 0) {
            selectedSprites = walkSpritesSouth;
            facingDirection = "SOUTH";

            if (diffVector.x > 0) {
                selectedSprites = walkSpritesSouthEast;
                facingDirection = "SOUTHEAST";
            }

            if (diffVector.x < 0) {
                selectedSprites = walkSpritesSouthWest;
                facingDirection = "SOUTHWEST";
            }
        } else if (diffVector.y > 0) {
            selectedSprites = walkSpritesNorth;
            facingDirection = "NORTH";

            if (diffVector.x > 0) {
                selectedSprites = walkSpritesNorthEast;
                facingDirection = "NORTHEAST";
            }

            if (diffVector.x < 0) {
                selectedSprites = walkSpritesNorthWest;
                facingDirection = "NORTHWEST";
            }
        } else if (diffVector.x > 0) {
            selectedSprites = walkSpritesEast;
            facingDirection = "EAST";
        } else if (diffVector.x < 0) {
            selectedSprites = walkSpritesWest;
            facingDirection = "WEST";
        }

        return selectedSprites;
    }

    private List<Sprite> GetIdleSprites() {
        List<Sprite> selectedSprites = null;

        switch (facingDirection) {
            case "NORTH":
                selectedSprites = idleSpritesNorth;
                break;
            case "NORTHEAST":
                selectedSprites = idleSpritesNorthEast;
                break;
            case "NORTHWEST":
                selectedSprites = idleSpritesNorthWest;
                break;
            case "SOUTH":
                selectedSprites = idleSpritesSouth;
                break;
            case "SOUTHEAST":
                selectedSprites = idleSpritesSouthEast;
                break;
            case "SOUTHWEST":
                selectedSprites = idleSpritesSouthWest;
                break;
            case "EAST":
                selectedSprites = idleSpritesEast;
                break;
            case "WEST":
                selectedSprites = idleSpritesWest;
                break;
        }

        return selectedSprites;
    }

    // private IEnumerator MoveActor(Vector3 directionToMove)
    // {
    //     isMoving = true;
    //
    //     float elapsedTime = 0;
    //
    //     originalPosition = transform.position;
    //     // targetPosition = originalPosition + directionToMove;
    //     targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     targetPosition.z = transform.position.z;
    //     
    //     while (elapsedTime < timeToMove)
    //     {
    //         transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / timeToMove));
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //
    //     transform.position = targetPosition;
    //         
    //     isMoving = false;
    // }
}
