#if UNITY_EDITOR
using System.IO;

using UnityEngine;
using UnityEditor;

using VacuumShaders.TerrainToMesh;


public class Runtime_OBJExporter : MonoBehaviour
{
    public Terrain terrain;

    public int vertexCountHorizontal = 10;
    public int vertexCountVertical = 10;


    // Use this for initialization
    void Start()
    {
        float time = Time.realtimeSinceStartup;
        
        //Cointins OBJ data
        string strOBJ = TerrainToMeshConverter.TerrainToOBJ(terrain, vertexCountHorizontal, vertexCountVertical);

        Debug.Log("OBJ convertion in: " + (Time.realtimeSinceStartup- time) + " sec");




        //Create directory 
        if (Directory.Exists(GetGenerateDirectory()) == false)
            Directory.CreateDirectory(GetGenerateDirectory());


        //File path
        string objFilePath = GetGenerateDirectory() + "/" + terrain.name + ".obj";


        //Save OBJ file
        StreamWriter sw = new StreamWriter(objFilePath);
        try
        {
            sw.WriteLine(strOBJ);
        }
        catch  (System.Exception err)
        {
            Debug.LogError("Houston, we have a problem: " + err.Message);
        }
        sw.Close();




        //Refresh asset database
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);


        //Load OBJ mesh
        gameObject.AddComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(objFilePath, typeof(Mesh));
        gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Diffuse")); 
    }


    string GetGenerateDirectory()
    {
        return "Assets/Terrain Obj files/" + terrain.name;
    }
}

#endif