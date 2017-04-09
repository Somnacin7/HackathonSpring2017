using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_Controller))]
[RequireComponent(typeof(VRHand))]
public class OsuHand : MonoBehaviour {

    public SphereCollider collider;
    private VRHand vrHand;

    void Start () {
        vrHand = GetComponent<VRHand>();
        collider = GetComponent<SphereCollider>();
	}

	// Update is called once per frame
	void Update () {
        var colliders = Physics.OverlapSphere(transform.position, collider.radius);
        if (vrHand.ButtonDown)
        {
            foreach (var col in colliders)
            {
                if (col.tag == "Beat")
                {
                }
            }
        }
	}
}
