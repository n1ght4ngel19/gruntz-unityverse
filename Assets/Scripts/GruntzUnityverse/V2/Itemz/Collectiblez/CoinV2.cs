using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Itemz.Collectiblez {
  public class CoinV2 : Collectible {

    protected override void Pickup(GruntV2 grunt) {
      // Todo: 1. Add coin to Level Statz
      // Todo: 2. Make Grunt play coin pickup animation

      base.Pickup(grunt);
    }
  }
}
