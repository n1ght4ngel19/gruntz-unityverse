using System;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse {
  public class CameraMovement : MonoBehaviour {
    private Camera Camera { get; set; }
    private Transform OwnTransform { get; set; }
    public bool AreControlsDisabled { get; set; }

    // The smaller the ScrollRate, the faster the camera moves
    private const int ScrollRate = 4;

    private float _targetZoom;
    private const float ZoomFactor = 3f;
    private const float ZoomLerpSpeed = 10;


    private void Start() {
      Camera = gameObject.GetComponent<Camera>();
      OwnTransform = gameObject.GetComponent<Transform>();
    }

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

      _targetZoom -= Input.GetAxis("Mouse ScrollWheel") * ZoomFactor;
      _targetZoom = Math.Clamp(_targetZoom, 4f, 11f);
      Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, _targetZoom, Time.deltaTime * ZoomLerpSpeed);
    }

    private void ScrollWithArrowKeys() {
      if (Time.timeScale == 0) {
        return;
      }

      Vector3 currentPosition = OwnTransform.position;
      float orthographicSize = Camera.orthographicSize;
      float camHalfWidth = orthographicSize * Camera.aspect;
      bool reachedBottom = currentPosition.y - orthographicSize / 2 <= LevelManager.Instance.MinMapPoint.y + 0.25;
      bool reachedTop = currentPosition.y + orthographicSize / 2 >= LevelManager.Instance.MaxMapPoint.y - 0.25;
      bool reachedLeftSide = currentPosition.x - camHalfWidth <= LevelManager.Instance.MinMapPoint.x + 0.25;
      bool reachedRightSide = currentPosition.x + camHalfWidth >= LevelManager.Instance.MaxMapPoint.x - 0.25;


      if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !reachedTop) {
        Camera.transform.position += Vector3.up / ScrollRate;
      }

      if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !reachedBottom) {
        Camera.transform.position += Vector3.down / ScrollRate;
      }

      if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !reachedLeftSide) {
        Camera.transform.position += Vector3.left / ScrollRate;
      }

      if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !reachedRightSide) {
        Camera.transform.position += Vector3.right / ScrollRate;
      }
    }
  }
}
