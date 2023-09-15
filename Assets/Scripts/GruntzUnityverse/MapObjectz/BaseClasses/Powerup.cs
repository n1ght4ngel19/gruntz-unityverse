using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public class Powerup : Item {
    public PowerupName powerupName;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      category = nameof(Powerup);
    }
  }
}
