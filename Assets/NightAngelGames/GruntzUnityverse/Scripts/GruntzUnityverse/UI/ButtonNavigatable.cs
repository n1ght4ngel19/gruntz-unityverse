using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.UI {
public class ButtonNavigatable : MonoBehaviour {
	public ButtonNavigatable previous =>
		transform.parent.GetComponentsInChildren<ButtonNavigatable>()
			.FirstOrDefault(b => b.next == this);

	public ButtonNavigatable next;

	private LevelLoader levelLoader => GetComponent<LevelLoader>();

	private DisableUITextOnButtonPress disableUITextOnButtonPress => GetComponent<DisableUITextOnButtonPress>();

	public void Activate() {
		if (levelLoader != null) {
			levelLoader.LoadLevel();
		}

		if (disableUITextOnButtonPress != null) {
			disableUITextOnButtonPress.HandleTargetz();
		}
	}
}
}
