using System.ComponentModel;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.MiscItemz {
  public class Warpletter : MiscItem {
    public WarpletterType warpletterType;

    protected override void Start() {
      mapItemName = nameof(Warpletter);

      string spriteName = spriteRenderer.sprite.name;

      warpletterType = spriteName.Contains("WarpletterW")
        ? WarpletterType.W
        : spriteName.Contains("WarpletterA")
          ? WarpletterType.A
          : spriteName.Contains("WarpletterR")
            ? WarpletterType.R
            : spriteName.Contains("WarpletterP")
              ? WarpletterType.P
              : throw new InvalidEnumArgumentException("Warpletter type cannot be identified!");

      base.Start();
    }
  }
}
