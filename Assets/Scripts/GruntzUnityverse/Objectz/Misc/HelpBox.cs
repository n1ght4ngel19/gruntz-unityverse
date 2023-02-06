using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Misc {
  public class HelpBox : MonoBehaviour {
    public CameraMovement mainCam;
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation { get; set; }

    public List<Sprite> animFrames;
    private const int FrameRate = 12;

    private bool isUntouched;
    public static bool IsTextShown;
    public string boxText;


    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);

      animFrames = Resources.LoadAll<Sprite>("Animated Sprites/MapObjectz/Misc/HelpBox")
        .ToList();

      isUntouched = true;
    }

    private void Update() {
      // Pausing the game when a Grunt steps onto a HelpBox and displaying the HelpBox text
      if (isUntouched
        && LevelManager.Instance.testGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(GridLocation))) {
        DisplayBox();

        return;
      }

      // Resuming the game when user clicks the left or right mouse button while the game is paused
      if (!isUntouched && IsTextShown && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
        HideBox();

        return;
      }

      if (LevelManager.Instance.testGruntz
        .All(grunt => !grunt.NavComponent.OwnLocation.Equals(GridLocation))) {
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
