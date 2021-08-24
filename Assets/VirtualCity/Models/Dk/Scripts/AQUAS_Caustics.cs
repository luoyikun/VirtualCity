using UnityEngine;
using System.Collections;

public class AQUAS_Caustics : MonoBehaviour {

    #region Variables
    public float fps;
	public Texture2D[] frames;
    public float maxCausticDepth;

	int frameIndex;
	Projector projector;
	#endregion

    //Initialize caustic image sequence
	void Start () {
		projector = GetComponent<Projector> ();
		NextFrame ();
		InvokeRepeating ("NextFrame", 1 / fps, 1 / fps);
		projector.material.SetFloat ("_WaterLevel", transform.parent.transform.position.y);
        projector.material.SetFloat("_DepthFade", transform.parent.transform.position.y-maxCausticDepth);
    }

    //<summary>
    //Adjusts the max caustic depth
    //</summary>
    void Update()
    {
        projector.material.SetFloat("_DepthFade", transform.parent.transform.position.y - maxCausticDepth);
    }

    //<summary>
    //Set current caustic texture based on fps
    //</summary>
	void NextFrame(){
		projector.material.SetTexture ("_Texture", frames[frameIndex]);
		frameIndex = (frameIndex+1) % frames.Length;
	}

}
