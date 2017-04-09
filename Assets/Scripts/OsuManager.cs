using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuManager : MonoBehaviour {

    public GameObject beatPrefab;
    public int bpm = 130;
    /// <summary>
    /// The center point of the beat spawns
    /// </summary>
    public Transform beatSpawnPoint;

    public float songLength = 121;

    private string BEAT = @"0000000000000000 0000000000000000 0000X00000000000 0000X00000000000 0000X0000000X000 0000X0000000X000 0000X0000000X000 00000000X0000000 00000000X0000000 00000000Y------- ----0000X0000000  00000000Y------- ----X0000000X000 0000X0000000Y--- --------Y------- ------------X000 0000X00000000000 00000000X0000000 0000X0000000X000 00000000Y------- ----X0000000X000 00000000X0000000 00000000X000X000 0000X0000000X000 0000000000000000 0000000000000000 0000000000000000 00000000X000X000 0000Y----------- ----0000X000X000 0000X0000000X000 00000000X000X000 00000000X0000000 00000000Y------- ----0000Y------- ----0000X0000000 Y--------------- Y--------------- Y--------------- 00000000X000X000 0000X0000000X000 0000Y----------- ----X0000000X000 0000Y----------- X00000000000X000 X00000000000X000 0000Y----------- ----0000X000X000 0000Y----------- X0000000X0000000 Y-----------X000 0000X0000000X000 0000X000Y------- ----0000X0000000 0000X000Y------- ----X0000000X000 0000X000Y------- ----X000Y------- ----X0000000X000 0000X0000000X000 X0000000X0000000 00000000X000X000 Y--------------- ----000000000000";
    private char[] beats;
    private int currentBeat = 0;
    private int currentPlayedBeat = -4  ;

    private BeatPattern beatPattern;
    private AudioSource audioSource;

    void Initialize()
    {
        beats = BEAT.Replace(" ", String.Empty).ToCharArray();

        var beat = beatPrefab.GetComponent<Beat>();
        var clipLength = 3.0f;
        beat.animationSpeed = (clipLength / (60.0f / bpm)) / 2.0f;
        beat.aliveTime = (60.0f / bpm) * 2.0f;

        Invoke("NextScene", songLength + 5);
    }

	void Start () {
        Initialize();

        beatPattern = GetComponent<BeatPattern>();
        audioSource = GetComponent<AudioSource>();
	}

    private void Awake()
    {
        InvokeRepeating("PlayBeat", 0, 60.0f / bpm / 4.0f);
        Invoke("StartBeats", 60.0f / bpm);
    }

    void PlayBeat()
    {
        var curBeatIndex = (currentBeat + 4) % beats.Length;

        if (beats[curBeatIndex] == 'X' || beats[curBeatIndex] == 'Y') {
            var spawnPos = beatPattern.GetBeatPosition(currentPlayedBeat);
            Instantiate(beatPrefab, spawnPos, Quaternion.identity);
            currentPlayedBeat++;
        }

        currentBeat++;
        if (currentBeat >= beats.Length)
        {
            currentBeat = 0;
        }
    }

    void StartBeats()
    {
        audioSource.Play();
    }

    void NextScene()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
