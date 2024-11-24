using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace GruntzUnityverse.UI {
public class QuickStartLevelLoader : LevelLoader {
    public override void LoadLevel() {
        Addressables.LoadSceneAsync(Namez.LoadMenuName, LoadSceneMode.Additive, false).Completed += handle1 => {
            handle1.Result.ActivateAsync().completed += _ => {
                GameObject.Find(Namez.LevelNameDisplayName).GetComponent<TMP_Text>().SetText(levelData.levelName);

                GameObject.Find(Namez.AreaNameDisplayName).GetComponent<TMP_Text>().SetText(levelData.areaName);

                Addressables.LoadAssetAsync<Sprite>($"AreaBackground-{levelData.areaCode}").Completed += handle2 => {
                    if (handle2.Result == null) {
                        Debug.LogError($"AreaBackground-{levelData.areaCode} not found");
                    }

                    GameObject.Find(Namez.LoadMenuBackgroundName).GetComponent<Image>().sprite = handle2.Result;
                };

                Addressables.LoadAssetAsync<GameSettings>(Namez.GameSettingsAssetName).Completed += settingsHandle => {
                    FindFirstObjectByType<ContinueToLevel>().LoadLevel(settingsHandle.Result.quickStartLevel.levelKey);
                };
            };
        };
    }
}
}
