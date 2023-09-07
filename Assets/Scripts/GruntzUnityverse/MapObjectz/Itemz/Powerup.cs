using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public class Powerup : Item {
    public PowerupName powerupName;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      category = nameof(Powerup);
    }
  }
}
