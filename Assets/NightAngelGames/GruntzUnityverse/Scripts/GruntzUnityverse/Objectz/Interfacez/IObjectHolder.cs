using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Interfacez {
public interface IObjectHolder {
	public LevelItem heldItem { get; set; }
	public Hazard hiddenHazard { get; set; }
	public Switch hiddenSwitch { get; set; }

	public void RevealHidden(bool isSceneLoaded) { }
}
}
