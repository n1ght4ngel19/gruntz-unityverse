using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

// Todo: Redo whole class!
namespace GruntzUnityverse.Objectz.Misc {
  public class HelpBox : MapObject {
    public CameraMovement mainCam;
    public SpriteRenderer spriteRenderer;

    public List<Sprite> animFrames;
    private const int FrameRate = 12;

    public bool isUntouched;
    public static bool IsTextShown;
    public string boxText;


    protected override void Start() {
      base.Start();

      animFrames = Resources.LoadAll<Sprite>("Animated Sprites/MapObjectz/Misc/HelpBox").ToList();

      isUntouched = true;
    }

    private void Update() {
      // Pausing the game when a Grunt steps onto a HelpBox and displaying the HelpBox text
      if (isUntouched && LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        DisplayBox();

        return;
      }

      // Resuming the game when user clicks the left or right mouse button while the game is paused
      if (!isUntouched && IsTextShown && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
        HideBox();

        return;
      }

      if (LevelManager.Instance.PlayerGruntz.All(grunt => !grunt.IsOnLocation(OwnLocation))) {
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
      mainCam.AreControlsDisabled = true;
    }

    private void HideBox() {
      Time.timeScale = 1;
      LevelManager.Instance.helpBoxText.text = "";
      IsTextShown = false;
      mainCam.AreControlsDisabled = false;
    }
  }
}
