using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public interface IBinaryToggleable {
    public bool IsOn { get; set; }
    public Sprite OnSprite { get; set; }
    public Sprite OffSprite { get; set; }

    void Toggle();

    void ToggleOn();

    void ToggleOff();
  }
}
