using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeatPattern : MonoBehaviour {

    public float radius = 1.0f;

    public abstract Vector3 GetBeatPosition(float x);
}
