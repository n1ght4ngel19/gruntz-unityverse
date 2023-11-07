using System;
using UnityEngine;

namespace GruntzUnityverse {
  /// <summary>
  /// Camera movement controller.
  /// </summary>
  public class CameraMovement : MonoBehaviour {
    /// <summary>
    /// Exposed field for being able to observe from outside if the camera's controls are disabled.
    /// </summary>
    public bool areControlsDisabled;

    private Camera _cameraComponent;
    private Transform _ownTransform;
    private const int ScrollRate = 4; // The smaller the ScrollRate, the faster the camera moves
    private float _targetZoom;
    private const float ZoomFactor = 3f;
    private const float ZoomLerpSpeed = 10;

    private void Start() {
      _cameraComponent = gameObject.GetComponent<Camera>();
      _ownTransform = gameObject.GetComponent<Transform>();
    }

    private void Update() {
      if (areControlsDisabled) {
        return;
      }

      ScrollWithKeys();
      ZoomWithMouse();
    }

    /// <summary>
    /// Zooming by scrolling the mouse.
    /// </summary>
    private void ZoomWithMouse() {
      if (Time.timeScale == 0) {
        return;
      }

      _targetZoom -= Input.GetAxis("Mouse ScrollWheel") * ZoomFactor;
      _targetZoom = Math.Clamp(_targetZoom, 4f, 11f);

      _cameraComponent.orthographicSize = Mathf.Lerp(
        _cameraComponent.orthographicSize,
        _targetZoom,
        Time.deltaTime * ZoomLerpSpeed
      );
    }

    /// <summary>
    /// Moving the camera by certain keys.
    /// </summary>
    private void ScrollWithKeys() {
      if (Time.timeScale == 0) {
        return;
      }

      // Todo: Change KeyCode.XY constants to be changeable in Settings.

      Vector3 currentPosition = _ownTransform.position;
      float orthographicSize = _cameraComponent.orthographicSize;
      float camHalfWidth = orthographicSize * _cameraComponent.aspect;

      bool reachedBottom = currentPosition.y - orthographicSize / 2
        <= GameManager.Instance.currentLevelManager.MinMapPoint.y + 0.25;

      bool reachedTop = currentPosition.y + orthographicSize / 2
        >= GameManager.Instance.currentLevelManager.MaxMapPoint.y - 0.25;

      bool reachedLeftSide = currentPosition.x - camHalfWidth
        <= GameManager.Instance.currentLevelManager.MinMapPoint.x + 0.25;

      bool reachedRightSide = currentPosition.x + camHalfWidth
        >= GameManager.Instance.currentLevelManager.MaxMapPoint.x - 0.25;


      if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !reachedTop) {
        _cameraComponent.transform.position += Vector3.up / ScrollRate;
      }

      if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !reachedBottom) {
        _cameraComponent.transform.position += Vector3.down / ScrollRate;
      }

      if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !reachedLeftSide) {
        _cameraComponent.transform.position += Vector3.left / ScrollRate;
      }

      if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !reachedRightSide) {
        _cameraComponent.transform.position += Vector3.right / ScrollRate;
      }
    }
  }
}
