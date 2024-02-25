using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz.Hazardz;

namespace GruntzUnityverse.Objectz.Interfacez {
public interface IObjectHolder {
	public LevelItem HeldItem { get; set; }
	public Hazard HiddenHazard { get; set; }

	public void RevealHidden(bool isSceneLoaded) { }
}
}
