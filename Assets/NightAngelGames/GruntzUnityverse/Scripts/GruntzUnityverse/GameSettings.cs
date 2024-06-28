using UnityEngine;

namespace GruntzUnityverse {
[CreateAssetMenu(fileName = "GameSettings", menuName = "Gruntz Unityverse/GameSettings", order = 1)]
public class GameSettings : ScriptableObject {
	public bool showHelpboxez;
	public bool playMusic;
	public bool playSoundz;

	public void SetShowHelpboxez(bool value) {
		showHelpboxez = value;
	}

	public void SetPlayMusic(bool value) {
		playMusic = value;
	}

	public void SetPlaySoundz(bool value) {
		playSoundz = value;
	}
}
}
