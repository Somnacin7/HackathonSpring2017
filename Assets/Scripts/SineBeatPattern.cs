using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class SineBeatPattern : BeatPattern
    {
        public float heightOffset = 0;
        public float sineFreq = 1.0f;
        public float sineAmp = 1.0f;

        public float circleFreq = 1.0f; 
            
        public override Vector3 GetBeatPosition(float input)
        {
            var pos = transform.position;

            float x, y, z;

            x = pos.x + -Mathf.Abs(Mathf.Sin(input * circleFreq)) * radius;
            z = pos.z + Mathf.Cos(input * circleFreq) * radius;

            y = pos.y + sineAmp * Mathf.Sin(input * sineFreq) + heightOffset;

            return new Vector3(x, y, z);
        }
    }
}
