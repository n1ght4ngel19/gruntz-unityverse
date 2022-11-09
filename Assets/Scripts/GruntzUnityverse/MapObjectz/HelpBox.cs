using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class HelpBox : MonoBehaviour, IMapObject {
    public CameraMovement mainCam;
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> animFrames;
    private const int FrameRate = 12;

    private bool isUntouched;
    public static bool IsTextShown;
    public string boxText;


    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);

      animFrames = Resources.LoadAll<Sprite>("Animations/MapObjectz/Misc/HelpBox").ToList();
      isUntouched = true;
    }

    private void Update() {
      // Pausing the game when a Grunt steps onto a HelpBox and displaying the HelpBox text
      if (isUntouched && MapManager.Instance.gruntz.Any(grunt => grunt.ownGridLocation.Equals(GridLocation))) {
        DisplayBox();

        return;
      }

      // Resuming the game when user clicks the Left Button while the game is paused.
      if (
        !isUntouched
        && IsTextShown
        && Input.GetMouseButtonDown(0)
      ) {
        HideBox();

        return;
      }

      if (MapManager.Instance.gruntz
          .All(grunt => grunt.ownGridLocation.Equals(GridLocation))) {
        isUntouched = true;
      }

      int frame = (int)(Time.time * FrameRate % animFrames.Count);
      spriteRenderer.sprite = animFrames[frame];
    }

    private void DisplayBox() {
      isUntouched = false;
      Time.timeScale = 0;
      MapManager.Instance.helpBoxText.text = boxText;
      IsTextShown = true;
      mainCam.areConstrolsDisabled = true;
    }

    private void HideBox() {
      Time.timeScale = 1;
      MapManager.Instance.helpBoxText.text = "";
      IsTextShown = false;
      mainCam.areConstrolsDisabled = false;
    }
  }
}
