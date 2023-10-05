using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.Arrowz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Saving {
  public struct ObjectData {
    public int objectId;
    public Vector3 position;
    public string spriteName;
    public string area;
    public string type;
    public string optionalBallDirection;
    public string optionalDirection;
    public string optionalOppositeDirection;

    public ObjectData(MapObject mapObject) {
      objectId = mapObject.objectId;
      position = mapObject.transform.position;
      spriteName = mapObject.spriteRenderer.sprite.name;
      area = mapObject.abbreviatedArea;
      type = mapObject.GetType().Name;

      optionalBallDirection = mapObject is RollingBall rollingBall
        ? rollingBall.moveDirection.ToString()
        : Direction.None.ToString();

      optionalDirection = mapObject is Arrow oneWayArrow
        ? oneWayArrow.direction.ToString()
        : Direction.None.ToString();

      optionalOppositeDirection = mapObject is TwoWayArrow twoWayArrow
        ? DirectionUtility.OppositeOf(DirectionUtility.StringDirectionAsDirection(optionalDirection)).ToString()
        : Direction.None.ToString();
    }
  }
}
