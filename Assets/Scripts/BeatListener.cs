using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatListener : MonoBehaviour {

    public GameObject beatPrefab;
    public int bpm = 130;

    /// <summary>
    /// The center point of the beat spawns
    /// </summary>
    public Transform beatSpawnPoint;
    private int currentBeat = 0;
    private int currentPlayedBeat = -2;

    private BeatPattern beatPattern;
    private AudioSource audioSource;

    private int currentSpectrumValue; // The largest parts of the spectrum at the moment

    private float[] xSpecValues;

    void Initialize() {
        var beat = beatPrefab.GetComponent<Beat>();
        var clipLength = 3.0f;
        beat.animationSpeed = (clipLength / (60.0f / bpm)) / 2.0f;
        beat.aliveTime = (60.0f / bpm) * 2.0f;
        xSpecValues = new float[] { -1.1f, -.09f, -0.7f, -0.5f, -0.3f, -0.1f, 0.1f, 0.3f, 0.5f, 0.7f, 0.9f, 1.1f };
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
        Invoke("StartBeat", 60.0f / bpm);
    }

    public void onBeatDetected() {
        var spawnPos = beatPattern.GetBeatPosition(currentPlayedBeat);
        Instantiate(beatPrefab, spawnPos, Quaternion.identity);
        currentPlayedBeat++;
        currentBeat++;
    }

    //This event will be called every frame while music is playing
    void onSpectrumDetected(float[] spectrum) {
        //The spectrum is logarithmically averaged
        //to 12 bands

        int maxIndex = -1;
        float max = 0;

        for (int n = 0; n < spectrum.Length; n++) {
            Vector3 start = new Vector3(xSpecValues[n], 0, 1);
            Vector3 end = new Vector3(xSpecValues[n], spectrum[n] * 10, 1);
            Debug.DrawLine(start, end);
            if (spectrum[n] > max) {
                max = spectrum[n];
                maxIndex = n;
            }
            currentSpectrumValue = maxIndex;
        }
    }

    void StartBeat() {
        audioSource.Play();
    }
}
