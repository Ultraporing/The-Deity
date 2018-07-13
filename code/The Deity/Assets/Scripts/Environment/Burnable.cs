using System;
using UnityEngine;

public class Burnable : MonoBehaviour
{ //Only game objects with the Burnable script can be burned
    public bool isOnFire;
    public void Burn(Fire fire)
    {
        fire.transform.position = transform.position;
        fire.transform.localScale = transform.localScale;
        fire.transform.parent = transform;
        isOnFire = true;
        
    }
}