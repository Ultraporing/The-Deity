using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructFire : MonoBehaviour
{
    //Lea Kohl

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Basic Ground")
        {
            Destroy(this.gameObject);
        }
    }
}