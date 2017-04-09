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

    private string BEAT = @"0000 0000 0X00 0X00 0X0X 0X0X 0X0X 00X0 00X0 00Y- -0X0  00Y- -X0X 0X0Y --Y- ---X 0X00 00X0 0X0X 00Y- -X0X 00X0 00XX 0X0X 0000 0000 0000 00XX 0Y-- -0XX 0X0X 00XX 00X0 00Y- -0Y- -0X0 Y--- Y--- Y--- 00XX 0X0X 0Y-- -X0X 0Y-- X00X X00X 0Y-- -0XX 0Y-- X0X0 Y--X 0X0X 0XY- -0X0 0XY- -X0X 0XY- -XY- -X0X 0X0X X0X0 00XX Y--- -000";
    private char[] beats;
    private int currentBeat = 0;
    private int currentPlayedBeat = -2  ;

    private BeatPattern beatPattern;
    private AudioSource audioSource;

    void Initialize()
    {
        beats = BEAT.Replace(" ", String.Empty).ToCharArray();

        var beat = beatPrefab.GetComponent<Beat>();
        var clipLength = 3.0f;
        beat.animationSpeed = (clipLength / (60.0f / bpm)) / 2.0f;
        beat.aliveTime = (60.0f / bpm) * 2.0f;
    }

	void Start () {
        Initialize();

        beatPattern = GetComponent<BeatPattern>();
        audioSource = GetComponent<AudioSource>();
	}

    private void Awake()
    {
        InvokeRepeating("PlayBeat", 0, 60.0f / bpm / 4.0f);
        audioSource.PlayDelayed(60.0f / bpm);
    }

    void PlayBeat()
    {
        var curBeatIndex = (currentBeat + 2) % beats.Length;

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
}
