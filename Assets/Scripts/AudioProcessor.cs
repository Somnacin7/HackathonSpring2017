/*
 * Copyright (c) 2015 Allan Pichardo
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
class AudioProcessor : MonoBehaviour {

    AudioSource audioSource;
    int beatsPerMinute = 130;

    int bufferSize = 1024;

    private long lastT;
    private long nowT;
    private long diff;
    private long entries;
    private long sum;
    private int samplingRate;
    private float framePeriod;
    private float[] spec; // Spectrum of the previous step
    private int nBand = 12; // Number of bands
    public float gThresh = 0.1f; // Sensitivity
    private int sinceLast = 0;
    private int colMax = 120;
    private float[] spectrum;
    private float[] averages;
    private float[] acVals;
    private float[] onsets;
    private float[] scorefun;
    private float[] dobeat;
    private int now = 0;
    private int maxLag = 100;
    private float decay = 0.997f;
    private float alph;

    private Autoco auco;

    [Header("Events")]
    public OnBeatEventHandler onBeat;
    public OnSpectrumEventHandler onSpectrum;

    void Start() {
        init();

        audioSource = GetComponent<AudioSource>();
        float secondsPerBeat = 1f / (beatsPerMinute / 60f);
        audioSource.PlayDelayed(secondsPerBeat * 2);
        samplingRate = audioSource.clip.frequency;
        framePeriod = (float)bufferSize / (float)samplingRate;

        spec = new float[nBand];
        for(int n = 0; n < nBand; n++) {
            spec[n] = 100.0f;
        }

        auco = new Autoco(maxLag, decay, framePeriod, getBandwidth());

        lastT = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
    }

    void init() {
        onsets = new float[colMax];
        scorefun = new float[colMax];
        dobeat = new float[colMax];
        spectrum = new float[bufferSize];
        averages = new float[12];
        acVals = new float[maxLag];
        alph = 100 * gThresh;
    }
    
    // Called once per frame
    void Update() {
        if(audioSource.isPlaying) {
            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            computeAverages(spectrum);
            onSpectrum.Invoke(averages);

            float onset = 0;
            for(int n = 0; n < nBand; n++) {
                float specVal = (float)System.Math.Max(-100.0, 20.0 * System.Math.Log10(averages[n]) + 160) * 0.025f;
                float dbInc = specVal - spec[n]; // decibel increase since last frame
                spec[n] = specVal;
                onset += dbInc;
            }

            onsets[now] = onset;

            auco.newVal(onset);

            // record largest value in (weighted) autocorrelation as it will be the tempo
            float aMax = 0.0f;
            int tempopd = 0;
            for (int i = 0; i < maxLag; ++i) {
                float acVal = (float)System.Math.Sqrt(auco.autoco(i));
                if (acVal > aMax) {
                    aMax = acVal;
                    tempopd = i;
                }
                // store in array backwards, so it displays right-to-left, in line with traces
                acVals[maxLag - 1 - i] = acVal;
            }

            /* calculate DP-ish function to update the best-score function */
            float smax = -999999;
            int smaxix = 0;
            // weight can be varied dynamically with the mouse
            alph = 100 * gThresh;
            // consider all possible preceding beat times from 0.5 to 2.0 x current tempo period
            for (int i = tempopd / 2; i < System.Math.Min(colMax, 2 * tempopd); ++i) {
                // objective function - this beat's cost + score to last beat + transition penalty
                float score = onset + scorefun[(now - i + colMax) % colMax] - alph * (float)System.Math.Pow(System.Math.Log((float)i / (float)tempopd), 2);
                // keep track of the best-scoring predecesor
                if (score > smax) {
                    smax = score;
                    smaxix = i;
                }
            }

            scorefun[now] = smax;
            // keep the smallest value in the score fn window as zero, by subtracing the min val
            float smin = scorefun[0];
            for (int i = 0; i < colMax; ++i) {
                if (scorefun[i] < smin) {
                    smin = scorefun[i];
                }
            }
            for (int i = 0; i < colMax; ++i) {
                scorefun[i] -= smin;
            }

            /* find the largest value in the score fn window, to decide if we emit a blip */
            smax = scorefun[0];
            smaxix = 0;
            for (int i = 0; i < colMax; ++i) {
                if (scorefun[i] > smax) {
                    smax = scorefun[i];
                    smaxix = i;
                }
            }

            bool beat = false;

            // dobeat array records where we actally place beats
            dobeat[now] = 0;  // default is no beat this frame
            ++sinceLast;
            // if current value is largest in the array, probably means we're on a beat
            if (smaxix == now) {
                // make sure the most recent beat wasn't too recently
                if (sinceLast > tempopd / 4) {
                    onBeat.Invoke();
                    // record that we did actually mark a beat this frame
                    dobeat[now] = 1;
                    // reset counter of frames since last beat
                    sinceLast = 0;



                    beat = true;

                    System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Users\\Robin\\test.txt", true);
                    file.Write("X");
                    file.Close();
                }
            }

            if(!beat) {
                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Users\\Robin\\test.txt", true);
                file.Write("0");
                file.Close();
            }

            /* update column index (for ring buffer) */
            if (++now == colMax) {
                now = 0;
            }
        }
    }

    void computeAverages(float[] data) {
        for (int n = 0; n < nBand; n++) {
            float avg = 0;
            int lowFreq;
            if(n == 0) {
                lowFreq = 0;
            }
            else {
                lowFreq = (int)((samplingRate / 2) / System.Math.Pow(2, nBand - n));
            }
            int highFreq = (int)((samplingRate / 2) / System.Math.Pow(2, nBand - 1 - n));
            int lowBound = freqToIndex(lowFreq);
            int highBound = freqToIndex(highFreq);
            for(int k = lowBound; k <= highBound; k++) {
                avg += data[k];
            }
            avg /= (highBound - lowBound + 1);
            averages[n] = avg;
        }
    }

    int freqToIndex(int freq) {
        if(freq < getBandwidth() / 2) {
            return 0;
        }
        if(freq > (samplingRate / 2) - (getBandwidth() / 2)) {
            return (bufferSize / 2);
        }
        float fraction = (float)freq / samplingRate;
        return (int)System.Math.Round(bufferSize * fraction);
    }

    float getBandwidth() {
        return (float)((2.0 / bufferSize) * (samplingRate / 2.0));
    }

    [System.Serializable]
    public class OnBeatEventHandler : UnityEngine.Events.UnityEvent {}

    [System.Serializable]
    public class OnSpectrumEventHandler : UnityEngine.Events.UnityEvent<float[]> {}

    private class Autoco {
        private int delLength;
        private float decay;
        private float[] delays;
        private float[] outputs;
        private int index;
        private float[] bpms;
        private float[] rWeight;
        private float wmiDbpm = 120f;
        private float wOctaveWidth;

        public Autoco(int length, float alpha, float framePeriod, float bandwidth) {
            wOctaveWidth = bandwidth;
            decay = alpha;
            delLength = length;
            delays = new float[delLength];
            outputs = new float[delLength];
            index = 0;

            bpms = new float[delLength];
            rWeight = new float[delLength];
            for(int n = 0; n < delLength; n++) {
                bpms[n] = 60.0f / (framePeriod * n);
                rWeight[n] = (float)System.Math.Exp(-0.5 * System.Math.Pow(System.Math.Log(bpms[n] / wmiDbpm) / System.Math.Log(2.0) / wOctaveWidth, 2.0));
            }
        }

        public void newVal(float val) {
            delays[index] = val;

            for(int n = 0; n < delLength; n++) {
                int delix = (index - n + delLength) % delLength;
                outputs[n] += (1 - decay) * (delays[index] * delays[delix] - outputs[n]);
            }

            if(++index == delLength) {
                index = 0;
            }
        }

        public float autoco(int del) {
            return rWeight[del] * outputs[del];
        }

        public float avgBpm() {
            float sum = 0;
            for(int n = 0; n < bpms.Length; n++) {
                sum += bpms[n];
            }
            return sum / delLength;
        }
    }
}
