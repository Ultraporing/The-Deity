using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    //Lea kohl
    //used to load tutorial scene
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
