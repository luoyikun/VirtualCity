using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("AQUAS/Render Queue Controller")]
public class AQUAS_RenderQueueEditor : MonoBehaviour
{
    public int renderQueueIndex = -1;
    void Update()
    {
        gameObject.GetComponent<Renderer>().sharedMaterial.renderQueue = renderQueueIndex;
    }
}

