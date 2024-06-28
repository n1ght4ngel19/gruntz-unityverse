using UnityEngine;

namespace GruntzUnityverse.Objectz.Interfacez {
public interface IBinaryToggleable {
	public bool IsPressed { get; set; }
	public Sprite PressedSprite { get; set; }
	public Sprite ReleasedSprite { get; set; }

	void Toggle();

	void ToggleOn();

	void ToggleOff();
}
}
