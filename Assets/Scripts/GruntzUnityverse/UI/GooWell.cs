using UnityEngine;

namespace GruntzUnityverse.UI {
public class GooWell : MonoBehaviour {
	public int amount;
	public GruntOven oven1;
	public GruntOven oven2;
	public GruntOven oven3;
	public GruntOven oven4;

	public RectTransform gooFill;

	public void Fill(int toAdd) {
		amount += toAdd;
		gooFill.transform.localScale = new Vector3(1, amount * 9, 1);

		if (amount >= 4) {
			BakeNext();

			gooFill.transform.localScale = new Vector3(1, 0, 1);
			amount = 0;
		}
	}

	public void BakeNext() {
		if (!oven1.filled) {
			oven1.Bake();
		} else if (!oven2.filled) {
			oven2.Bake();
		} else if (!oven3.filled) {
			oven3.Bake();
		} else if (!oven4.filled) {
			oven4.Bake();
		}
	}
}
}
