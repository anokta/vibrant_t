﻿using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GameManager : MonoBehaviour {

  public GameObject monsterPrefab;

  public GUIText monsterSpeechGui;
  public string[] monsterLines;
  public string[] monsterRandomLinebreaks;
  private int currentMonsterLine = 0;

  public GameObject groundPrefab;

  public int tempo = 124;

  private Sequencer sequencer;

  private CameraController cameraController;

  private MonsterController monster;

  private LineController line;

  void Awake() {
    sequencer = GetComponent<Sequencer>();
    sequencer.tempo = tempo;
    sequencer.OnNextBar += OnNextBar;
    cameraController = GetComponent<CameraController>();
    monster = 
      GameObject.Instantiate(monsterPrefab).GetComponent<MonsterController>();
    monster.speechGui = monsterSpeechGui;
    monster.SetLeftEnd(cameraController.ScreenLeft);
    monster.speakSpeed = 8.0f / tempo;
    line = GameObject.Instantiate(groundPrefab).GetComponent<LineController>();
    line.SetStartPoint(monster.GetRightEnd());
    line.SetLength(0.5f * cameraController.ScreenSize - monster.GetRightEnd());
  }
  
  // Update is called once per frame
  void Update() {
    line.AddSample(0.75f * monster.MouthOutput);
  }

  void OnNextBar(Sequencer sequencer) {
    string speech = "";
    if(RandomNumber.NextFloat() < 0.2f) {
      speech = monsterRandomLinebreaks[RandomNumber.NextInt(0, 
        monsterRandomLinebreaks.Length)];
    } else {
      speech = monsterLines[(int)Mathf.Min(monsterLines.Length - 1, 
        currentMonsterLine++)];
    }
    monster.Speak(speech);
  }
}
