using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

// Todo: Redo whole class!
namespace GruntzUnityverse.Objectz.Misc {
  public class HelpBox : MapObject {
    [field: SerializeField] public CameraMovement MainCam { get; set; }
    [field: SerializeField] public bool IsUntouched { get; set; }
    [field: SerializeField] public string BoxText { get; set; }
    public static bool IsTextShown { get; set; }


    protected override void Start() {
      base.Start();

      IsUntouched = true;
    }

    private void Update() {
      // Pausing the game when a Grunt steps onto a HelpBox and displaying the HelpBox text
      if (IsUntouched && LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.IsOnLocation(Location))) {
        DisplayBox();

        return;
      }

      // Resuming the game when user clicks the left or right mouse button while the game is paused
      if (!IsUntouched && IsTextShown && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
        HideBox();

        return;
      }

      if (LevelManager.Instance.PlayerGruntz.All(grunt => !grunt.IsOnLocation(Location))) {
        IsUntouched = true;
      }
    }

    private void DisplayBox() {
      IsUntouched = false;
      Time.timeScale = 0;
      LevelManager.Instance.helpBoxText.text = BoxText;
      IsTextShown = true;
      MainCam.AreControlsDisabled = true;
    }

    private void HideBox() {
      Time.timeScale = 1;
      LevelManager.Instance.helpBoxText.text = "";
      IsTextShown = false;
      MainCam.AreControlsDisabled = false;
    }
  }
}
