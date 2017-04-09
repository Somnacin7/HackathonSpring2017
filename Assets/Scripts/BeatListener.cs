using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatListener : MonoBehaviour {

    public GameObject volBarPrefab;
    public int bpm = 130;

    /// <summary>
    /// The center point of the beat spawns
    /// </summary>
    private int currentBeat = 0;
    private int currentPlayedBeat = -2;

    private BeatPattern beatPattern;
    private AudioSource audioSource;

    private GameObject[] specBars;

    //private int currentSpectrumValue; // The largest parts of the spectrum at the moment

    private float[] xSpecValues;
    private VolBarSet volBarSet;

    void Initialize() {
        specBars = new GameObject[12];
        var clipLength = 3.0f;
        xSpecValues = new float[] { -1.1f, -.09f, -0.7f, -0.5f, -0.3f, -0.1f, 0.1f, 0.3f, 0.5f, 0.7f, 0.9f, 1.1f };

        volBarSet = new VolBarSet();
    }

    void Start() {
        Initialize();

        beatPattern = GetComponent<BeatPattern>();
        audioSource = GetComponent<AudioSource>();

        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onBeatDetected);
        processor.onSpectrum.AddListener(onSpectrumDetected);
    }

    private void Awake() {
        //Invoke("StartBeat", 60.0f / bpm);
    }

    public void onBeatDetected() {
        var spawnPos = beatPattern.GetBeatPosition(currentPlayedBeat);
        currentPlayedBeat++;
        currentBeat++;
    }

    //This event will be called every frame while music is playing
    void onSpectrumDetected(float[] spectrum) {
        volBarSet.resize(spectrum);
    }
    
    void StartBeat() {
        audioSource.Play();
    }

    class VolBarSet {

        float width1 = 2.85f / 12f;
        float width2 = 2.15f / 12f;
        float thickness = 0.1f;
        float zOffset1 = 1.1f;
        float zOffset2 = 1.45f;
        float[] xOffsets1;
        float[] xOffsets2;
        GameObject[] cubes1;
        GameObject[] cubes2;
        GameObject[] cubes3;

        public VolBarSet() {

            xOffsets1 = new float[12];
            xOffsets2 = new float[12];

            cubes1 = new GameObject[12];
            cubes2 = new GameObject[12];
            cubes3 = new GameObject[12];

            for (int n = 0; n < 12; n++) {
                xOffsets1[n] = -(2.85f / 2f) + (width1 * n) + (width1 / 2f);
                xOffsets2[n] = -(2.15f / 2f) + (width2 * n) + (width2 / 2f);
                cubes1[n] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubes1[n].transform.position = new Vector3(xOffsets1[n], 0, zOffset1);
                ((Renderer)(cubes1[n].GetComponent(typeof(Renderer)))).material.color = new Color(1, 0, 1);


                cubes2[n] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubes2[n].transform.position = new Vector3(-xOffsets1[n], 0, -zOffset1);
                ((Renderer)(cubes2[n].GetComponent(typeof(Renderer)))).material.color = new Color(1, 0, 1);


                cubes3[n] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubes3[n].transform.position = new Vector3(zOffset2, 0, xOffsets2[n]);
                ((Renderer)(cubes3[n].GetComponent(typeof(Renderer)))).material.color = new Color(0, 1, 1);

            }

        }

        public void resize(float[] spectrum) {
            for (int n = 0; n < 12; n++) {
                float height = spectrum[n] * 20;
                cubes1[n].transform.localScale = new Vector3(width1, height, thickness);
                cubes2[n].transform.localScale = new Vector3(width1, height, thickness);
                cubes3[11 - n].transform.localScale = new Vector3(thickness, height, width2);
            }
        }

    }
}
