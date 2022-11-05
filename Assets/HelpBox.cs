using System.Collections.Generic;
using System.Linq;

using GruntzUnityverse.Singletonz;

using UnityEngine;

public class HelpBox : MonoBehaviour {
  public CameraMovement mainCam;
  public SpriteRenderer spriteRenderer;
  private bool isUntouched;
  public static bool IsTextShown;
  public string boxText;
  
  public List<Sprite> animFrames;
  private const int FrameRate = 12;

  private void Start() {
    animFrames = Resources.LoadAll<Sprite>("Animations/MapObjectz/Misc/HelpBox").ToList();
    isUntouched = true;
  }

  private void Update() {
    // Pausing the game when a Grunt steps onto a HelpBox and displaying the HelpBox text
    if (
      isUntouched
      && MapManager.Instance.gruntz.Any(grunt1 => (Vector2)grunt1.transform.position == (Vector2)transform.position)
    ) {
      displayBox();

      return;
    }

    // Resuming the game when user clicks the Left Button while the game is paused.
    if (
      !isUntouched
      && IsTextShown
      && Input.GetMouseButtonDown(0)
    ) {
      hideBox();

      return;
    }

    if (MapManager.Instance.gruntz.All(grunt1 => (Vector2)grunt1.transform.position != (Vector2)transform.position)) {
      isUntouched = true;
    }
    
    int frame = (int)(Time.time * FrameRate % animFrames.Count);
    spriteRenderer.sprite = animFrames[frame];
  }

  public void displayBox() {
    Debug.Log(boxText);

    isUntouched = false;
    Time.timeScale = 0;
    MapManager.Instance.helpBoxText.text = boxText;
    IsTextShown = true;
    mainCam.areConstrolsDisabled = true;
  }

  public void hideBox() {
    Debug.Log(boxText);

    Time.timeScale = 1;
    MapManager.Instance.helpBoxText.text = "";
    IsTextShown = false;
    mainCam.areConstrolsDisabled = false;
  }
}
