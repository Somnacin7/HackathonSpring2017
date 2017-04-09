using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour {

    public float aliveTime = .1f;

    public float animationSpeed = 1f;   
    private Animator animator;

    void Awake () {
        Invoke("Destroy", aliveTime);

        animator = GetComponentInChildren<Animator>();
	}

    private void Update()
    {
        animator.speed = animationSpeed;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
