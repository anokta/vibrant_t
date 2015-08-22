using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineController : MonoBehaviour {
  // Line frame to be drawn in (normalized for the screen).
  public Rect frame = new Rect(Vector2.zero, new Vector2(1.0f, 0.5f));

  // Line resolution (number of vertices on the screen).
  public int resolution = 40;

  // Line movement speed per second (normalized for the screen).
  public float speed = 1.0f;

  // Line renderer instance.
  private LineRenderer lineRenderer = null;

  // Next sample to be added to the samples.
  private float nextSample = 0.0f;

  // Last time in miliseconds when the next sample was added.
  private float lastSampleTime = 0.0f;
 
  // Line samples.
  private List<float> samples = null;

  // Screen boundaries.
  private Vector3 screenSize = Vector3.zero;

  void Awake () {
    lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.SetVertexCount(resolution + 1);
    samples = new List<float>(resolution);
    for (int i = 0; i < resolution; ++i) {
      samples.Add(0.0f);
    }
    lastSampleTime = Time.time;
    screenSize = 2.0f * Camera.main.ScreenToWorldPoint(
      new Vector3(Screen.width, Screen.height, 0.0f));
  }
  
  void Update () {
    // Add next sample to the line.
    if (Time.time - lastSampleTime >= 1.0f / (resolution * speed)) {
      samples.RemoveAt(samples.Count - 1);
      samples.Insert(0, 0.5f * nextSample);
      lastSampleTime = Time.time;
      nextSample = 0.0f;
    }
    // Update the line with the current samples.
    for (int i = 0; i < resolution + 1; ++i) {
      float x = frame.x - 0.5f * frame.width + i * frame.width / resolution;
      float y = frame.y + (i < resolution ? samples[i] : 0.0f) * frame.height;
      lineRenderer.SetPosition(
        i, Vector3.Scale(screenSize, new Vector3(x, y, 0.0f)));
    }
  }

  public void AddSample (float value) {
    nextSample = value;
  }
}
