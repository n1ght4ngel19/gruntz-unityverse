using GruntzUnityverse.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace GruntzUnityverse {
[CreateAssetMenu(fileName = "LevelData", menuName = "Gruntz Unityverse/Level Data")]
public class LevelData : ScriptableObject {
    public string levelKey;

    public string levelName;

    public Area area;

    public string areaName => StringUtils.CamelCaseToSpaced(area.ToString());

    public Sprite loadMenuBackground;

    public string areaCode => area switch {
        Area.RockyRoadz => "1RR",
        Area.Gruntziclez => "2GR",
        Area.TroubleInTheTropicz => "3TT",
        Area.HighOnSweetz => "4HS",
        Area.HighRollerz => "5HR",
        Area.HoneyIShrunkTheGruntz => "6HSG",
        Area.TheMiniatureMasterz => "7MM",
        Area.GruntzInSpace => "8GS",
        Area.VirtualReality => "9VR",
        _ => "1RR",
    };

    private void OnValidate() {
        Addressables.LoadAssetAsync<Sprite>($"AreaBackground-{areaCode}").Completed += handle => {
            if (handle.Result == null) {
                Debug.LogError($"AreaBackground-{areaCode} not found");
            }

            loadMenuBackground = handle.Result;
        };
    }
}
}
