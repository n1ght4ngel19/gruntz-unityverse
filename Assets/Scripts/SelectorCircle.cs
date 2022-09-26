using UnityEngine;

public class SelectorCircle : MonoBehaviour {
  public Camera mainCamera;

  private void Update() {
    transform.position = CustomStuff.SetSelectorCirclePosition(mainCamera.ScreenToWorldPoint(Input.mousePosition));
  }
}
