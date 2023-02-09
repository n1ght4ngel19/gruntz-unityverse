using System;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse {
  public class CameraMovement : MonoBehaviour {
    private Camera Camera { get; set; }
    public bool AreControlsDisabled { get; set; }

    // The smaller the ScrollRate, the faster the camera moves
    private const int ScrollRate = 4;

    private float targetZoom;
    private const float ZoomFactor = 3f;
    private const float ZoomLerpSpeed = 10;


    private void Start() { Camera = Camera.main; }

    private void Update() {
      if (AreControlsDisabled) {
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
      Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, targetZoom, Time.deltaTime * ZoomLerpSpeed);
    }

    private void ScrollWithArrowKeys() {
      if (Time.timeScale == 0) {
        return;
      }
      
      float camHalfWidth = Camera.orthographicSize * Camera.aspect;
      bool reachedBottom = Camera.transform.position.y - Camera.orthographicSize <= LevelManager.Instance.MinMapPoint.y + 0.25;
      bool reachedTop = Camera.transform.position.y + Camera.orthographicSize >= LevelManager.Instance.MaxMapPoint.y - 0.25;
      bool reachedLeftSide = Camera.transform.position.x - camHalfWidth <= LevelManager.Instance.MinMapPoint.x + 0.25;
      bool reachedRightSide = Camera.transform.position.x + camHalfWidth >= LevelManager.Instance.MaxMapPoint.x - 0.25;


      if (reachedTop) {
        LimitMovement("up");
      } else if (reachedBottom)
        LimitMovement("down");
      else if (reachedLeftSide)
        LimitMovement("left");
      else if (reachedRightSide)
        LimitMovement("right");
      else
        MoveFreely();
    }

    private void LimitMovement(string direction) {
      switch (direction) {
        case "down": {
          if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            Camera.transform.position += Vector3.up / ScrollRate;

          if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            Camera.transform.position += Vector3.left / ScrollRate;

          if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            Camera.transform.position += Vector3.right / ScrollRate;

          break;
        }
        case "up": {
          if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            Camera.transform.position += Vector3.down / ScrollRate;

          if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            Camera.transform.position += Vector3.left / ScrollRate;

          if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            Camera.transform.position += Vector3.right / ScrollRate;

          break;
        }
        case "left": {
          if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            Camera.transform.position += Vector3.up / ScrollRate;

          if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            Camera.transform.position += Vector3.down / ScrollRate;

          if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            Camera.transform.position += Vector3.right / ScrollRate;

          break;
        }
        case "right": {
          if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            Camera.transform.position += Vector3.up / ScrollRate;

          if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            Camera.transform.position += Vector3.down / ScrollRate;

          if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            Camera.transform.position += Vector3.left / ScrollRate;

          break;
        }
      }
    }

    private void MoveFreely() {
      if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        Camera.transform.position += Vector3.up / ScrollRate;

      if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        Camera.transform.position += Vector3.down / ScrollRate;

      if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        Camera.transform.position += Vector3.left / ScrollRate;

      if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        Camera.transform.position += Vector3.right / ScrollRate;
    }
  }
}
