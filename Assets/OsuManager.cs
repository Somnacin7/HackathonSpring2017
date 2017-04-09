using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuManager : MonoBehaviour {

    public GameObject beatPrefab;
    public int bpm = 130;
    public Transform spawnLeft;
    public Transform spawnRight;

    private string BEAT =
@"X00000X0X0000000X0X000X0X0000000
0000X0000000X0000000X0000000X000";
    private char[] leftBeats;
    private char[] rightBeats;
    private int currentBeat = 0;

    void Initialize()
    {
        var beatLines = BEAT.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        leftBeats = beatLines[0].ToCharArray();
        rightBeats = beatLines[1].ToCharArray();
    }

	void Start () {
        Initialize();
	}

    private void Awake()
    {
        InvokeRepeating("PlayBeat", 0, 60.0f / bpm / 4);
    }

    void PlayBeat()
    {
        Debug.Log("BEAT");
        if (leftBeats[currentBeat] == 'X')
        {
            Instantiate(beatPrefab, spawnLeft);
            Debug.Log("LEFT");
        }

        if (rightBeats[currentBeat] == 'X')
        {
            Instantiate(beatPrefab, spawnRight);
            Debug.Log("RIGHT");
        }

        currentBeat = (currentBeat + 1) % leftBeats.Length;
    }
}
