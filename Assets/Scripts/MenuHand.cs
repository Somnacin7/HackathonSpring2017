using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
[RequireComponent(typeof(VRHand))]
public class MenuHand : MonoBehaviour
{

    public float radius = 0.1f;

    private VRHand vrHand;

    void Start()
    {
        vrHand = GetComponent<VRHand>();
    }

    // Update is called once per frame
    void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius);
        if (vrHand.ButtonDown)
        {
            foreach (var col in colliders)
            {
                if (col.tag == "OptionsBall")
                {
                    // Open options menu
                    // TODO
                }
                else if (col.tag == "PlayBall")
                {
                    SceneManager.LoadSceneAsync(1);
                }
                else if (col.tag == "QuitBall")
                {
                    // Quit game
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
                }
            }
        }
    }
}
