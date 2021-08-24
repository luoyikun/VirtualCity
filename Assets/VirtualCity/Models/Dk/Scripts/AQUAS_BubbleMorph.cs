using UnityEngine;
using System.Collections;

public class AQUAS_BubbleMorph : MonoBehaviour {

    #region Variables
    float t =0;
	float t2=0;
    [Space(5)]
    [Header("Duration of a full morphing cycle")]
	public float tTarget;

    SkinnedMeshRenderer skinnedMeshRenderer;
    #endregion

    void Start() {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    //<summary>
    //Morphs the bubble through shape keys based on 2 timers
    //Morphin is cyclic and repeats once in tTarget
    //</summary>
	void Update () {
	
		t += Time.deltaTime;
		t2 += Time.deltaTime;

		if (t < tTarget / 2) 
        {
			skinnedMeshRenderer.SetBlendShapeWeight (0, Mathf.Lerp (0, 50, t / (tTarget / 2)));
			skinnedMeshRenderer.SetBlendShapeWeight (1, Mathf.Lerp (50, 0, t / (tTarget / 2)));
		} 
        
        else if (t >= tTarget / 2 && t < tTarget) 
        {
			skinnedMeshRenderer.SetBlendShapeWeight (0, Mathf.Lerp (50, 100, t / tTarget));
			skinnedMeshRenderer.SetBlendShapeWeight (1, Mathf.Lerp (0, 50, t / tTarget));
		} 
        
        else if (t >= tTarget && t < (tTarget * 1.5f)) 
        {
			skinnedMeshRenderer.SetBlendShapeWeight (0, Mathf.Lerp (100, 50, t / (tTarget * 1.5f)));
			skinnedMeshRenderer.SetBlendShapeWeight (1, Mathf.Lerp (50, 100, t / (tTarget * 1.5f)));
		} 
        
        else if (t >= tTarget * 1.5f && t < (tTarget * 2)) 
        {
			skinnedMeshRenderer.SetBlendShapeWeight (0, Mathf.Lerp (50, 0, t / (tTarget * 2)));
			skinnedMeshRenderer.SetBlendShapeWeight (1, Mathf.Lerp (100, 50, t / (tTarget * 2)));
		} 
        
        else 
        {
            t = 0;
        }
	}
}
