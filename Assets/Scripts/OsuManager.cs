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

    private BeatPattern beatPattern;

    void Initialize()
    {
        var beatLines = BEAT.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        leftBeats = beatLines[0].ToCharArray();
        rightBeats = beatLines[1].ToCharArray();
    }

	void Start () {
        Initialize();

        beatPattern = GetComponent<BeatPattern>();
	}

    private void Awake()
    {
        InvokeRepeating("PlayBeat", 0, 60.0f / bpm / 4);
    }

    void PlayBeat()
    {
        // TODO: add math spawner
        var spawnPos = beatPattern.GetBeatPosition(currentBeat);
        Instantiate(beatPrefab, spawnPos, Quaternion.identity);

        currentBeat = (currentBeat + 1) % leftBeats.Length;
    }
}
