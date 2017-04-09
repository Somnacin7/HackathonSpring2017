using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VRHand : MonoBehaviour {

    ulong grabInput;
    SteamVR_TrackedObject controller;

    public bool ButtonDown { get; set; }

    public bool Button { get; set; }

    public bool ButtonUp { get; set; }

    void Awake () {
        controller = GetComponent<SteamVR_TrackedObject>();
        grabInput = SteamVR_Controller.ButtonMask.Trigger;
	}
	
	void Update () {
        var device = SteamVR_Controller.Input((int)controller.index);
        ButtonDown = device.GetTouchDown(grabInput);

        Button = device.GetTouch(grabInput);

        ButtonUp = device.GetTouchUp(grabInput);
    }

    public void HapticPulse(ushort duration)
    {
        var device = SteamVR_Controller.Input((int)controller.index);
        device.TriggerHapticPulse(duration);
    }
}
