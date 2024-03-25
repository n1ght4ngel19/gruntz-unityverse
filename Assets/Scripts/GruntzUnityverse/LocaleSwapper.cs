using UnityEngine;
using UnityEngine.Localization.Settings;

namespace GruntzUnityverse {
public class LocaleSwapper : MonoBehaviour {
	private void OnSwitchLocale() {
		if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1]) {
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
		} else {
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
		}
	}
}
}
