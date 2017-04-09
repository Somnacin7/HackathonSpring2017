using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour {

    public float aliveTime = .1f;

	void Awake () {
        Invoke("Destroy", aliveTime);
	}

    void Destroy()
    {
        Destroy(gameObject);
    }
}
