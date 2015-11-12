// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using BarelyAPI;

public class KeyboardController : MonoBehaviour {
  public int fundamentalIndex = (int)NoteIndex.C4;
  KeyCode[] keys = {
    KeyCode.A,
    KeyCode.W,
    KeyCode.S,
    KeyCode.E,
    KeyCode.D,
    KeyCode.F,
    KeyCode.T,
    KeyCode.G,
    KeyCode.Y,
    KeyCode.H,
    KeyCode.U,
    KeyCode.J,
    KeyCode.K,
    KeyCode.O,
    KeyCode.L
  };
  Instrument instrument;

  public GUIText companionGui;
  private int numKeysDown = 0;
  private Color textColor;

  public OscillatorType OscType {
    get { return ((SynthInstrument)instrument).OscType; }
    set { ((SynthInstrument)instrument).OscType = value; }
  }

  void Awake() {
    instrument = GetComponent<Instrument>();
    textColor = companionGui.color;
  }

  void Start() {
    instrument.StopAllNotes();
  }

  void Update() {
    if (Input.anyKey) {
      companionGui.text += "" + (char)RandomNumber.NextInt(97, 123);
    }
    if (Input.anyKeyDown) {
      instrument.PlayNote(new Note(0.0f, 0.4f));
      numKeysDown++;
      companionGui.color = 
        textColor * (1.0f + Mathf.Min(0.2f, numKeysDown * 0.05f));
    } else if (!Input.anyKey) {
      instrument.PlayNote(new Note(0.0f, 0.0f));
      companionGui.text = "";
      companionGui.color = textColor;
      numKeysDown = 0;
    }
  }
}