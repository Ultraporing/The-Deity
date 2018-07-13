using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Environment.Planet
{
    public class PlanetDatalayerUpdater : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            PlanetDatalayer.Instance.Update();
        }
    }
}