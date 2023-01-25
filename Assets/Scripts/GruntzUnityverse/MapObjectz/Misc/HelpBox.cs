using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Misc {
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

      animFrames = Resources.LoadAll<Sprite>("Animated Sprites/MapObjectz/Misc/HelpBox").ToList();
      isUntouched = true;
    }

    private void Update() {
      // Pausing the game when a Grunt steps onto a HelpBox and displaying the HelpBox text
      if (isUntouched && LevelManager.Instance.gruntz.Any(grunt => grunt.NavComponent.OwnGridLocation.Equals(GridLocation))) {
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

      if (LevelManager.Instance.gruntz
          .All(grunt => grunt.NavComponent.OwnGridLocation.Equals(GridLocation))) {
        isUntouched = true;
      }

      int frame = (int)(Time.time * FrameRate % animFrames.Count);
      spriteRenderer.sprite = animFrames[frame];
    }

    private void DisplayBox() {
      isUntouched = false;
      Time.timeScale = 0;
      LevelManager.Instance.helpBoxText.text = boxText;
      IsTextShown = true;
      mainCam.areControlsDisabled = true;
    }

    private void HideBox() {
      Time.timeScale = 1;
      LevelManager.Instance.helpBoxText.text = "";
      IsTextShown = false;
      mainCam.areControlsDisabled = false;
    }
  }
}
