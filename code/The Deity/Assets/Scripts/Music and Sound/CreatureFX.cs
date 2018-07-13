using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreatureFX : MonoBehaviour {
    [System.Serializable]
    public struct FXContainer
    {
        public string Name;
        public AudioClip AudioClip;
    }

    public List<FXContainer> m_FXList = new List<FXContainer>();
    
    public AudioClip GetClip(string name)
    {
        List<FXContainer> clip = m_FXList.Where(x => x.Name == name).ToList();
        return clip.Count > 0 ? clip[0].AudioClip : null;
    }
}
