using UnityEngine;
using System.Collections;
using BarelyAPI;

public class MonsterController : MonoBehaviour {

  public GUIText speechGui;

  public Transform jawUp;
  public Transform jawDown;

  public Instrument instrument;
  public int fundamentalIndex = (int)NoteIndex.C4 - 12;

  public float speakSpeed = 0.1f;
  public float speakLoudness = 0.5f;

  private MarkovChain markov;
  private ModeGenerator scale;

  private string currentSpeech = "";
  private bool shouldSpeak = false;

  public float MouthOutput {
    get { return 3.0f * instrument.lastOutput; }
  }

  const float jawMaxAngle = 180.0f;

  // Use this for initialization
  void Start() {
    instrument.StopAllNotes();
    markov = new MarkovChain(8);
    scale = new ModeGenerator();
    scale.SetScale(MusicalScale.Major, MusicalMode.Dorian);
  }

  // Update is called once per frame
  void Update() {
    UpdateJaw(MouthOutput);
    if (shouldSpeak) {
      speechGui.text = "";
      if (currentSpeech.Length > 0) {
        StartCoroutine(StartSpeaking(currentSpeech));
      }
      shouldSpeak = false;
    }
  }

  public void SetLeftEnd(Vector3 position) {
    transform.position =
      position + 1.5f * Vector3.right * transform.lossyScale.x;
  }

  public Vector3 GetRightEnd() {
    return transform.position + Vector3.right * 0.75f * transform.lossyScale.x;
  }

  public void UpdateJaw(float sample) {
    float angle = Mathf.Abs(sample) * jawMaxAngle;
    jawUp.rotation = Quaternion.Slerp(jawUp.rotation,
      Quaternion.AngleAxis(angle, Vector3.forward), 4.0f * Time.deltaTime);
    jawDown.rotation = Quaternion.Slerp(jawDown.rotation,
      Quaternion.AngleAxis(angle, Vector3.back), 4.0f * Time.deltaTime);
  }

  public void Speak(string speech) {
    currentSpeech = speech;
    shouldSpeak = true;
  }

  private IEnumerator StartSpeaking(string speech) {
    foreach (string word in speech.Split(' ')) {
      markov.GenerateNextState();
      float noteIndex =
        fundamentalIndex + scale.GetNoteOffset(markov.CurrentState);
      Note note =
        new Note(noteIndex, RandomNumber.NextFloat(0.9f, 1.0f) * speakLoudness);
      instrument.PlayNote(note);
      speechGui.text += " " + word;
      yield return new WaitForSeconds(word.Length * speakSpeed);
      instrument.PlayNote(new Note(note.Index, 0.0f));
      yield return new WaitForSeconds(RandomNumber.NextFloat() * speakSpeed);
    }
  }
}
