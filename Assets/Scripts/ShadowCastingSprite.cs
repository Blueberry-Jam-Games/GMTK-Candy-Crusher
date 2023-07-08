using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class ShadowCastingSprite : MonoBehaviour
{
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        spriteRenderer.receiveShadows = true;
    }

    #if UNITY_EDITOR
    private SpriteRenderer targetRenderer;

    private void Update()
    {
        if (!Application.isPlaying)
        {
            if(targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
                targetRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                targetRenderer.receiveShadows = true;

                EditorUtility.SetDirty(this.gameObject);
            }
        }
    }
    #endif
}
