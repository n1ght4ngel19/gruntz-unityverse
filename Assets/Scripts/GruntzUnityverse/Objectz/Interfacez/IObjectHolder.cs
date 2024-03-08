using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Interfacez {
public interface IObjectHolder {
	public LevelItem HeldItem { get; set; }
	public Hazard HiddenHazard { get; set; }
	public Switch HiddenSwitch { get; set; }

	public void RevealHidden(bool isSceneLoaded) { }
}
}
