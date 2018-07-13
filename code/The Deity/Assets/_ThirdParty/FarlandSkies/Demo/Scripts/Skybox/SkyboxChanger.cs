using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkyboxChanger : MonoBehaviour
{
    public Material[] Skyboxes;
    //private Dropdown _dropdown;

    public void Awake()
    {
        //_dropdown = GetComponent<Dropdown>();
        //var options = Skyboxes.Select(skybox => skybox.name).ToList();
        //_dropdown.AddOptions(options);
    }

    public void ChangeSkybox(int idx)
    {
        RenderSettings.skybox = Skyboxes[idx];
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }
}