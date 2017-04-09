using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public int Score { get; set; }

    private List<OsuHand> hands = new List<OsuHand>();

    // Use this for initialization
    void Start () {
        Score = 0;

        var handGameObjects = GameObject.FindGameObjectsWithTag("Hand");
        foreach (var go in handGameObjects)
        {
            hands.Add(go.GetComponent<OsuHand>());
        }
	}

	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }
}
