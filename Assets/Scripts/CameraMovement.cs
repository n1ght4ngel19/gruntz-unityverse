using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
  [SerializeField] private int mapWidthInTiles;
  [SerializeField] private int mapHeightInTiles;
  [SerializeField] private Camera cam;

  // The smaller the ScrollRate, the faster the camera moves
  private const int ScrollRate = 4;

  private float targetZoom;
  private const float ZoomFactor = 3f;
  private const float ZoomLerpSpeed = 10;
  public bool areConstrolsDisabled;
  
  private void Update() {
    if (areConstrolsDisabled) {
      return;
    }

    ScrollWithArrowKeys();
    ZoomWithMouse();
  }

  private void ZoomWithMouse() {
    if (Time.timeScale == 0) {
      return;
    }

    targetZoom -= Input.GetAxis("Mouse ScrollWheel") * ZoomFactor;
    targetZoom = Math.Clamp(targetZoom, 4f, 11f);
    cam.orthographicSize = Mathf.Lerp(
      cam.orthographicSize,
      targetZoom,
      Time.deltaTime * ZoomLerpSpeed
    );
  }

  private void ScrollWithArrowKeys() {
    if (Time.timeScale == 0) {
      return;
    }
    
    float camHalfWidth = cam.orthographicSize * cam.aspect;

    if (cam.transform.position.y + cam.orthographicSize >= mapHeightInTiles - 0.1)
      LimitMovement("up");
    else if (cam.transform.position.y - cam.orthographicSize <= 0.1)
      LimitMovement("down");
    else if (cam.transform.position.x - camHalfWidth <= 0.1)
      LimitMovement("left");
    else if (cam.transform.position.x + camHalfWidth >= mapWidthInTiles - 0.1)
      LimitMovement("right");
    else
      MoveFreely();
  }

  private void LimitMovement(string direction) {
    switch (direction)
    {
      case "down": {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
          cam.transform.position += Vector3.up / ScrollRate;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
          cam.transform.position += Vector3.left / ScrollRate;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
          cam.transform.position += Vector3.right / ScrollRate;
        break;
      }
      case "up": {
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
          cam.transform.position += Vector3.down / ScrollRate;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
          cam.transform.position += Vector3.left / ScrollRate;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
          cam.transform.position += Vector3.right / ScrollRate;
        break;
      }
      case "left": {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
          cam.transform.position += Vector3.up / ScrollRate;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
          cam.transform.position += Vector3.down / ScrollRate;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
          cam.transform.position += Vector3.right / ScrollRate;
        break;
      }
      case "right": {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
          cam.transform.position += Vector3.up / ScrollRate;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
          cam.transform.position += Vector3.down / ScrollRate;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
          cam.transform.position += Vector3.left / ScrollRate;
        break;
      }
    }
  }

  private void MoveFreely() {
    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
      cam.transform.position += Vector3.up / ScrollRate;
    if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
      cam.transform.position += Vector3.down / ScrollRate;
    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
      cam.transform.position += Vector3.left / ScrollRate;
    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
      cam.transform.position += Vector3.right / ScrollRate;
  }
}
