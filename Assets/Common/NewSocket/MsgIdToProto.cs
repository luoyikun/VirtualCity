using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MsgIdToProto : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Type t = typeof(MsgIdDefine);
        PropertyInfo[] fields = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        int i = fields.Length;

        foreach (FieldInfo field in t.GetFields())
        {
            Console.WriteLine("Field: {0}, Value:{1}", field.Name, field.GetValue(t));
        }

        Debug.Log(i);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
