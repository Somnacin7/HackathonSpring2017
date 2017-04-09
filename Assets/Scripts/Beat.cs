using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour {

	void Awake () {
        Invoke("Destroy", 0.1f);
	}

    void Destroy()
    {
        Destroy(gameObject);
    }
}
