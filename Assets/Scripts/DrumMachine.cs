using UnityEngine;
using System.Collections;
using BarelyAPI;

public class DrumMachine : MonoBehaviour {

  PercussiveInstrument drums;
  Sequencer sequencer;

  float nextNote = -1.0f;

  // Use this for initialization
  void Start() {
    drums = GetComponent<PercussiveInstrument>();
    sequencer = FindObjectOfType<Sequencer>();
    sequencer.OnNextPulse += OnNextPulse;
  }

  void Update() {
    if (nextNote >= 0.0f) {
      StartCoroutine(Play(nextNote, 0.05f));
      nextNote = -1.0f;
    }
  }

  void OnNextPulse(Sequencer sequencer) {
    if (sequencer.CurrentPulse % 4 == 0) {
      nextNote = 12.0f * (sequencer.CurrentPulse % 32) / 4.0f;
    } else if (sequencer.CurrentPulse % 2 == 0 &&
                RandomNumber.NextFloat() < 0.1f) {
      nextNote = 12.0f * RandomNumber.NextInt(0, 8);
    }
  }

  IEnumerator Play(float noteIndex, float duration) {
    drums.PlayNote(new Note(noteIndex, RandomNumber.NextFloat(0.75f, 1.0f)));
    yield return new WaitForSeconds(duration);
    drums.PlayNote(new Note(noteIndex, 0.0f));
  }
}
