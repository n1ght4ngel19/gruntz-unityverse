using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace GruntzUnityverse.UI {
public class LevelLoader : MonoBehaviour {
    [Expandable]
    public LevelData levelData;

    public TMP_Text buttonText;

    [BoxGroup("Old")]
    public string levelKey;

    [BoxGroup("Old")]
    public string levelName;

    [BoxGroup("Old")]
    public string areaName;

    [BoxGroup("Old")]
    public Sprite loadMenuBackground;

    public virtual void LoadLevel() {
        Addressables.LoadSceneAsync("LoadMenu", LoadSceneMode.Additive, false).Completed += handle1 => {
            handle1.Result.ActivateAsync().completed += _ => {
                GameObject.Find(Namez.LevelNameDisplayName).GetComponent<TMP_Text>().SetText(levelData.levelName);
                GameObject.Find(Namez.AreaNameDisplayName).GetComponent<TMP_Text>().SetText(levelData.areaName);

                Addressables.LoadAssetAsync<Sprite>($"AreaBackground-{levelData.areaCode}").Completed += handle2 => {
                    if (handle2.Result == null) {
                        Debug.LogError($"AreaBackground-{levelData.areaCode} not found");
                    }

                    GameObject.Find(Namez.LoadMenuBackgroundName).GetComponent<Image>().sprite = handle2.Result;
                };

                FindFirstObjectByType<ContinueToLevel>().LoadLevel(levelData.levelKey);
            };
        };
    }

    public void LocalizeLevelName(string toSet) {
        buttonText.SetText(toSet);
        levelName = toSet;
    }

    public void LocalizeAreaName(string toSet) {
        areaName = toSet;
    }
}
}
