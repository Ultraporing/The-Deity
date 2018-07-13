/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Animates the Texture based on the provided rate and direction.
/// Moves the texture in a linear motion.
/// </summary>
public class AnimatedUVs : MonoBehaviour
{
    public int m_MaterialIndex = 0;
    public Vector2 m_UvAnimationRate = new Vector2(1.0f, 0.0f);
    public string m_TextureName = "_MainTex";
    private Renderer m_Renderer;

    Vector2 uvOffset = Vector2.zero;

    /// <summary>
    /// Get the renderer
    /// </summary>
    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Update the Texture UV by applying the direction and time
    /// </summary>
    void LateUpdate()
    {
        uvOffset += (m_UvAnimationRate * Time.deltaTime);
        if (m_Renderer.enabled)
        {
            m_Renderer.materials[m_MaterialIndex].SetTextureOffset(m_TextureName, uvOffset);
        }
    }
}