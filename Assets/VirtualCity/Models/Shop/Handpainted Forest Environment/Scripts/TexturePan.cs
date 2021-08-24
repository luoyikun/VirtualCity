using UnityEngine;
using System.Collections;

public class TexturePan : MonoBehaviour 
{
	public float scrollSpeed = 1.0f;
	Renderer rend;
	
	void Start () 
	{
		rend=GetComponent<Renderer>();
	}

	void Update () 
	{
		float offset = Time.time * scrollSpeed;
		rend.material.SetTextureOffset ("_MainTex",new Vector2(0,offset));
	}
}
