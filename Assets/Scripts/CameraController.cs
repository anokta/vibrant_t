using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

  public Vector3 ScreenSize {
    get { return screenSize; }
  }

  public Vector3 ScreenLeft {
    get { return Vector3.Scale(0.5f * Vector3.left, screenSize); }
  }

  public Vector3 ScreenRight {
    get { return Vector3.Scale(0.5f * Vector3.right, screenSize); }
  }

  // Main camera.
  private Camera mainCamera = null;

  // Screen boundaries.
  private Vector3 screenSize = Vector3.zero;

  void Awake() {
    mainCamera = Camera.main;
    screenSize = 2.0f * mainCamera.ScreenToWorldPoint(
      new Vector3(Screen.width, Screen.height, 0.0f));
  }

  // Update is called once per frame
  void Update() {

  }
}
