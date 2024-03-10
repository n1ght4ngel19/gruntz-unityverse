using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GruntzUnityverse.UI {
/// <summary>
/// Camera movement controller.
/// </summary>
public class CameraMovement : MonoBehaviour {
	public GameActions gameActions;

	private Camera _cameraComponent;
	private Transform _ownTransform;
	private float _targetZoom;

	public float minZoom;
	public float maxZoom;
	public float zoomFactor;
	public float zoomLerpSpeed;
	public int moveRate;

	private void Start() {
		_cameraComponent = gameObject.GetComponent<Camera>();
		_ownTransform = gameObject.GetComponent<Transform>();
	}

	private void FixedUpdate() {
		ScrollWithKeys();
	}

	public void OnZoom(InputValue value) {
		float zoom = value.Get<float>();

		_targetZoom -= zoom * zoomFactor;
		_targetZoom = Math.Clamp(_targetZoom, minZoom, maxZoom);

		GetComponent<Camera>().orthographicSize = Mathf.Lerp(
			GetComponent<Camera>().orthographicSize,
			_targetZoom,
			Time.deltaTime * zoomLerpSpeed
		);
	}

	// public void OnMove(InputAction.CallbackContext ctx) {
	// 	while (!ctx.canceled) {
	// 		Vector2 move = ctx.ReadValue<Vector2>();
	//
	// 		transform.position += new Vector3(move.x, move.y, 0) / moveRate;
	// 	}
	// }

	/// <summary>
	/// Zooming by scrolling the mouse.
	/// </summary>
	private void ZoomWithMouse() {
		if (Time.timeScale == 0) {
			return;
		}

		_targetZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomFactor;
		_targetZoom = Math.Clamp(_targetZoom, 4f, 11f);

		_cameraComponent.orthographicSize = Mathf.Lerp(
			_cameraComponent.orthographicSize,
			_targetZoom,
			Time.deltaTime * zoomLerpSpeed
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

		// bool reachedBottom = currentPosition.y - orthographicSize / 2
		//   <= Level.Instance.MinMapPoint.y + 0.25;
		//
		// bool reachedTop = currentPosition.y + orthographicSize / 2
		//   >= GameManager.Instance.currentLevelManager.MaxMapPoint.y - 0.25;
		//
		// bool reachedLeftSide = currentPosition.x - camHalfWidth
		//   <= GameManager.Instance.currentLevelManager.MinMapPoint.x + 0.25;
		//
		// bool reachedRightSide = currentPosition.x + camHalfWidth
		//   >= GameManager.Instance.currentLevelManager.MaxMapPoint.x - 0.25;

		transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) / moveRate;


		// if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !reachedTop) {
		// 	_cameraComponent.transform.position += Vector3.up / ScrollRate;
		// }
		//
		// if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !reachedBottom) {
		// 	_cameraComponent.transform.position += Vector3.down / ScrollRate;
		// }
		//
		// if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !reachedLeftSide) {
		// 	_cameraComponent.transform.position += Vector3.left / ScrollRate;
		// }
		//
		// if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !reachedRightSide) {
		// 	_cameraComponent.transform.position += Vector3.right / ScrollRate;
		// }
	}
}
}
