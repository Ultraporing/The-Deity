using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour {
    //Lea Kohl
    //script that deactivates the game object after a certain amount of time (to avoid spamming the sound)
    public float timer;
	void Start () {
        timer = 5;
	}

	void Update () {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 5;
            this.gameObject.SetActive(false);
        }
	}
}
