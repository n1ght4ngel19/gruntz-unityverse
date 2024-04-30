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
	/// Move the camera with certain keys.
	/// </summary>
	private void ScrollWithKeys() {
		transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) / moveRate;
	}
}
}
