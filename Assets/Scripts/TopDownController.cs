using System.Collections.Generic;

using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public Rigidbody2D body;
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


    public static List<TopDownController> gruntz = new List<TopDownController>();
    
    private int tileSize = 32;
    
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

    private void Start() {
        gruntz.Add(this);
        targetPosition = transform.position;

    }

    // Update is called once per frame
    void Update() {
        if (!hasMoved) {
            float playTime = Time.time - idleTime;
            int frame = (int)((playTime * idleFrameRate) % idleSpritesSouth.Count);
                
            spriteRenderer.sprite = idleSpritesSouth[frame];
        }
        
        if (Input.GetMouseButtonDown(1) && isSelected) {
            isMoving = true;
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;

            if (!hasMoved)
                hasMoved = true;
        }
        
        diffVector = Custom.Round(targetPosition) - Custom.Round(transform.position);

        // if (diffVector.x > 0) {
        //     transform.position = Vector3.MoveTowards(
        //         transform.position,
        //         Custom.Round(Vector3.right),
        //         timeToMove * Time.deltaTime
        //     );
        // }
        // else {
            transform.position = Vector3.MoveTowards(
                transform.position,
                Custom.Round(targetPosition),
                timeToMove * Time.deltaTime
            );
        // }
        
        
        
        
        
        
        
        
        
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

        foreach (TopDownController grunt in gruntz) {
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
