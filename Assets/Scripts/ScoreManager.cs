using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public GameObject leftHand;
    public GameObject rightHand;

    public string scorePrefix = "SCORE: ";
    public Text scoreText;


    public int score = 0;
    private OsuHand leftOsuHand;
    private OsuHand rightOsuHand;

    private List<OsuHand> hands = new List<OsuHand>();

    // Use this for initialization
    void Awake () {
        leftOsuHand = leftHand.GetComponent<OsuHand>();
        rightOsuHand = rightHand.GetComponent<OsuHand>();

        var scoreNumber = GameObject.FindGameObjectWithTag("ScoreNumber");
        if (scoreNumber != null)
        {
            var scoreNumberText = scoreNumber.GetComponent<TextMesh>();
            scoreNumberText.text = "" + score;

        }
    }

	// Update is called once per frame
	void Update () {
        scoreText.text = scorePrefix + "\n" + score;

        var scoreNumber = GameObject.FindGameObjectWithTag("ScoreNumber");
        if (scoreNumber != null)
        {
            var scoreNumberText = scoreNumber.GetComponent<TextMesh>();
            scoreNumberText.text = "" + score;

        }
    }

    private void OnEnable()
    {
        leftOsuHand.OnDestroyBeat += addScore;
        rightOsuHand.OnDestroyBeat += addScore;
    }

    private void OnDisable()
    {
        leftOsuHand.OnDestroyBeat -= addScore;
        rightOsuHand.OnDestroyBeat -= addScore;
    }

    private void addScore()
    {
        score += 10;
    }
}
