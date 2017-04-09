using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
[RequireComponent(typeof(VRHand))]
public class OsuHand : MonoBehaviour {

    public float radius = 0.1f;

    public delegate void DestroyBeat();
    public event DestroyBeat OnDestroyBeat;

    private VRHand vrHand;

    void Start () {
        vrHand = GetComponent<VRHand>();
	}

	// Update is called once per frame
	void Update () {
        var colliders = Physics.OverlapSphere(transform.position, radius);
        if (vrHand.ButtonDown)
        {
            foreach (var col in colliders)
            {
                if (col.tag == "Beat")
                {
                    Destroy(col.gameObject);

                    if (OnDestroyBeat != null)
                    {
                        OnDestroyBeat();
                    }

                    vrHand.HapticPulse(2000);
                }
            }
        }
	}
}
