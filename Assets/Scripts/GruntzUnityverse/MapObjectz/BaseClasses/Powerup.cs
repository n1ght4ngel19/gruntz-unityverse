using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public abstract class Powerup : Item {
    public PowerupName powerupName;
    public int duration;

    protected override void Start() {
      base.Start();

      category = nameof(Powerup);
    }
  }
}
