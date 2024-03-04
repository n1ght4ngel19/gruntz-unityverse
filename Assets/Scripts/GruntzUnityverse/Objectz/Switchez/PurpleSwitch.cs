using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;

namespace GruntzUnityverse.Objectz.Switchez {
public class PurpleSwitch : Switch {
	public List<PurplePyramid> pyramidz;
	public List<PurpleSwitch> otherSwitchez;

	public override void Setup() {
		base.Setup();

		pyramidz = transform.parent.GetComponentsInChildren<PurplePyramid>()
			.ToList();

		otherSwitchez = transform.parent.GetComponentsInChildren<PurpleSwitch>()
			.Where(sw => sw != this)
			.ToList();
	}
}
}
