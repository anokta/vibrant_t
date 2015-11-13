using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineController : MonoBehaviour {
  // Line frame to be drawn in (normalized for the screen).
  public Rect frame = new Rect(Vector2.zero, new Vector2(1.0f, 0.5f));

  // Line resolution (number of vertices on the screen).
  public int resolution = 2000;

  // Line movement speed per second (normalized for the screen).
  public float speed = 1.0f;

  // Line renderer instance.
  private LineRenderer lineRenderer = null;

  // Last time in miliseconds when the next sample was added.
  private float lastSampleTime = 0.0f;

  // Next sample to be added to |samples|.
  private float nextSample = 0.0f;

  // Number of waiting samples to be added to |samples|.
  private int numNextSamples = 0;

  // Line samples.
  private List<float> samples = null;

  // Camera controller.
  private CameraController cameraController = null;


  void Awake() {
    cameraController = FindObjectOfType<CameraController>();
    lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.SetVertexCount(resolution + 1);
    samples = new List<float>(resolution);
    for (int i = 0; i < resolution; ++i) {
      samples.Add(0.0f);
    }
    lastSampleTime = Time.time;
  }

  void Update() {
    // Add next sample to the line.
    if (Time.time - lastSampleTime >= 1.0f / (resolution * speed)) {
      samples.RemoveAt(samples.Count - 1);
      samples.Insert(0, 0.5f * nextSample);
      lastSampleTime = Time.time;
      nextSample = 0.0f;
      numNextSamples = 0;
    }
    // Update the line with the current samples.
    for (int i = 0; i < resolution + 1; ++i) {
      float x = frame.x + i * frame.width / resolution;
      float y = frame.y + (i < resolution ? samples[i] : 0.0f) * frame.height;
      lineRenderer.SetPosition(i, new Vector3(x, y, 0.0f));
    }
  }

  public void SetStartPoint(Vector3 start) {
    frame.x = start.x;
    frame.y = start.y;
  }

  public void SetLength(Vector3 size) {
    frame.width = size.x;
    frame.height = size.y;
  }

  public void AddSample(float value) {
    nextSample = (nextSample * numNextSamples + value) / (numNextSamples + 1);
    ++numNextSamples;
  }
}
