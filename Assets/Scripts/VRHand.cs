using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VRHand : MonoBehaviour {

    ulong grabInput;
    SteamVR_TrackedObject controller;

    public delegate void ButtonDown();
    public event ButtonDown OnButtonDown;

    public delegate void Button();
    public event Button OnButton;

    public delegate void ButtonUp();
    public event ButtonUp OnButtonUp;

    void Awake () {
        controller = GetComponent<SteamVR_TrackedObject>();
        grabInput = SteamVR_Controller.ButtonMask.Trigger;
	}
	
	void Update () {
        var device = SteamVR_Controller.Input((int)controller.index);
        if (device.GetTouchDown(grabInput)) {
            if (OnButtonDown != null) {
                OnButtonDown();
            }
            Debug.Log("BasdfasdD");

        }

        if (device.GetTouch(grabInput) && OnButton != null)
        {
            OnButton();
        }

        if (device.GetTouchUp(grabInput) && OnButtonUp != null)
        {
            OnButtonUp();
        }
    }

    private void OnEnable()
    {
        OnButtonDown += BD;
        OnButton += B;
        OnButtonUp += BU;
    }

    private void OnDisable()
    {
        OnButtonDown -= BD;
        OnButton -= B;
        OnButtonUp -= BU;
    }

    void BD()
    {
        Debug.Log("BD");
    }

    void B()
    {
        Debug.Log("B");
    }

    void BU()
    {
        Debug.Log("BU");
    }
}
