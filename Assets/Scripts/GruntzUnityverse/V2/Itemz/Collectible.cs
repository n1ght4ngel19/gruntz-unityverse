using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Itemz {
  public abstract class Collectible : ItemV2 {
    // Calling this last in child classes so that the gameObject is destroyed only after the pickup animation has finished
    // This way it's easier to keep track of destroying each collectible
    protected override void Pickup(GruntV2 grunt) {
      Destroy(gameObject);
    }
  }
}
