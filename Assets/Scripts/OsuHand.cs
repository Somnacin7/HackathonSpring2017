using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
[RequireComponent(typeof(VRHand))]
public class OsuHand : MonoBehaviour {

    public SphereCollider col;

    private VRHand vrHand;

    void Start () {
        vrHand = GetComponent<VRHand>();
        col = GetComponent<SphereCollider>();
	}

	// Update is called once per frame
	void Update () {
        var colliders = Physics.OverlapSphere(transform.position, col.radius);
        if (vrHand.ButtonDown)
        {
            foreach (var col in colliders)
            {
                if (col.tag == "Beat")
                {
                    Destroy(col.gameObject);
                }
            }
        }
	}
}
