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

    private string BEAT =
@"X00000X0X0000000X0X000X0X0000000
0000X0000000X0000000X0000000X000";
    private char[] leftBeats;
    private char[] rightBeats;
    private int currentBeat = 0;
    private int currentPlayedBeat = -2  ;

    private BeatPattern beatPattern;
    private AudioSource audioSource;

    void Initialize()
    {
        var beatLines = BEAT.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        leftBeats = beatLines[0].ToCharArray();
        rightBeats = beatLines[1].ToCharArray();

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
        Invoke("StartBeat", 60.0f / bpm);
    }

    void PlayBeat()
    {

        if (leftBeats[(currentBeat + 2) % leftBeats.Length] == 'X' || rightBeats[(currentBeat + 2) % leftBeats.Length] == 'X') {
            var spawnPos = beatPattern.GetBeatPosition(currentPlayedBeat);
            Instantiate(beatPrefab, spawnPos, Quaternion.identity);
            currentPlayedBeat++;
        }

        currentBeat++;
        if (currentBeat >= leftBeats.Length)
        {
            currentBeat = 0;
        }
    }

    void StartBeat()
    {
        audioSource.Play();
    }
}
