using UnityEngine;
using System.Collections;

[AddComponentMenu("AQUAS/Buoyancy")]
[RequireComponent(typeof(Rigidbody))]
public class AQUAS_Buoyancy : MonoBehaviour {

    #region variables
    //<summary>
    //public variables
    //</summary>
    public float waterLevel;
    public float waterDensity;
    [Space(5)]
    public bool useBalanceFactor;
    public Vector3 balanceFactor;
    [Space(20)]
    [Range(0, 1)]
    public float dynamicSurface=0.3f;
    [Range(1, 10)]
    public float bounceFrequency=3;

    public enum debugModes { none, showAffectedFaces, showForceRepresentation, showReferenceVolume };
    [Space(5)]
    [Header("Debugging can be ver performance heavy!")]
    public debugModes debug;

    //<summary>
    //variables to store required object info in
    //</summary>
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;
    Rigidbody rb;

    //<summary>
    //Additional variables for surface dynamics simulation
    //</summary>
    float effWaterDensity;
    float regWaterDensity;
    float maxWaterDensity;
    #endregion

    //<summary>
    //cache object info at start and set max water density for surface dynamics simulation
    //</summary>
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        rb = GetComponent<Rigidbody>();

        regWaterDensity = waterDensity;
        maxWaterDensity = regWaterDensity + regWaterDensity * 0.5f * dynamicSurface;
    }
	
	//<summary>
    //Balance factor must not be zero, because it's used as a divisor
    //Check if balance factor is zero and run AddForce Method on fixed time frame
    //</summary>
	void FixedUpdate () {

            if (balanceFactor.x < 0.001f){balanceFactor.x = 0.001f;}
            if (balanceFactor.y < 0.001f){balanceFactor.y = 0.001f;}
            if (balanceFactor.z < 0.001f){balanceFactor.z = 0.001f;}

        AddForce();

	}

    //<summary>
    //The Update method dynamically alters the effective water density according to parameters given
    //to simulate surface dynamics and prevent floating objects from standing still
    //</summary>
    void Update()
    {

        regWaterDensity = waterDensity;
        maxWaterDensity = regWaterDensity + regWaterDensity * 0.5f * dynamicSurface;
        effWaterDensity = ((maxWaterDensity - regWaterDensity) / 2) + regWaterDensity + Mathf.Sin(Time.time * bounceFrequency) * (maxWaterDensity - regWaterDensity) / 2;
    }

    //<summary>
    //Iterate through the triangles of the mesh
    //Approximate volume ousted by each triangle
    //Calculate updrift according to Archimedes' principle
    //More dense and inhomogeneous meshes will get an overkill torque from forces applied (chattering). The balance factor is used to cancel the effect out
    //Apply calculated forces at the center points of the mesh triangles
    //Debug if required
    //</summary>
    void AddForce() {

        //Iterate through the triangles
        for (int i = 0; i < triangles.Length; i += 3)
        {
            //Get the points of the current triangle
            Vector3 p1 = vertices[triangles[i]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            //Calculate the triangle center's distance to the water surface
            float distanceToWater = waterLevel - Center(p1, p2, p3).y;

            //Only add force if the normal of the triangle is facing down
            if (distanceToWater > 0 && Center(p1,p2,p3).y> (Center(p1, p2, p3)+Normal(p1,p2,p3)).y)
            {
                //Calculate updrift
                float updrift = effWaterDensity * Physics.gravity.y * distanceToWater * Area(p1, p2, p3) *Normal(p1,p2,p3).normalized.y;

                //Add force
                if (useBalanceFactor)
                {
                    rb.AddForceAtPosition(new Vector3(0, updrift, 0), transform.TransformPoint(new Vector3(transform.InverseTransformPoint(Center(p1, p2, p3)).x / (balanceFactor.x * transform.localScale.x * 1000), transform.InverseTransformPoint(Center(p1, p2, p3)).y / (balanceFactor.y * transform.localScale.x * 1000), transform.InverseTransformPoint(Center(p1, p2, p3)).z / (balanceFactor.z * transform.localScale.x * 1000))));

                }
                else
                {
                    rb.AddForceAtPosition(new Vector3(0, updrift, 0), transform.TransformPoint(new Vector3(transform.InverseTransformPoint(Center(p1, p2, p3)).x, transform.InverseTransformPoint(Center(p1, p2, p3)).y, transform.InverseTransformPoint(Center(p1, p2, p3)).z)));
                }

                //Show which triangles are currently affected
                if (debug == debugModes.showAffectedFaces)
                {
                    Debug.DrawLine(Center(p1, p2, p3), Center(p1, p2, p3) + Normal(p1, p2, p3), Color.white);
                }

                //Show a representation of the updrift force
                if (debug == debugModes.showForceRepresentation)
                {
                    Debug.DrawRay(Center(p1, p2, p3), new Vector3(0, updrift, 0), Color.red);
                }

                ///<summary>
                ///Show a representation of the approximated volume ousted
                ///This is NOT physically significant: The ousted volume will increase continuously
                ///when the mesh goes deeper, even if it's already fully underwater, which is
                ///actually wrong, you wouldn't be able to use this for submarines, but for
                ///floating objects such as boats and ships it's sufficient
                ///</summary>
                if (debug == debugModes.showReferenceVolume)
                {
                    Debug.DrawLine(new Vector3(transform.TransformPoint(p1).x, Center(p1, p2, p3).y, transform.TransformPoint(p1).z), new Vector3(transform.TransformPoint(p2).x, Center(p1, p2, p3).y, transform.TransformPoint(p2).z), Color.green);
                    Debug.DrawLine(new Vector3(transform.TransformPoint(p2).x, Center(p1, p2, p3).y, transform.TransformPoint(p2).z), new Vector3(transform.TransformPoint(p3).x, Center(p1, p2, p3).y, transform.TransformPoint(p3).z), Color.green);
                    Debug.DrawLine(new Vector3(transform.TransformPoint(p3).x, Center(p1, p2, p3).y, transform.TransformPoint(p3).z), new Vector3(transform.TransformPoint(p1).x, Center(p1, p2, p3).y, transform.TransformPoint(p1).z), Color.green);

                    Debug.DrawLine(new Vector3(transform.TransformPoint(p1).x, waterLevel, transform.TransformPoint(p1).z), new Vector3(transform.TransformPoint(p2).x, waterLevel, transform.TransformPoint(p2).z), Color.green);
                    Debug.DrawLine(new Vector3(transform.TransformPoint(p2).x, waterLevel, transform.TransformPoint(p2).z), new Vector3(transform.TransformPoint(p3).x, waterLevel, transform.TransformPoint(p3).z), Color.green);
                    Debug.DrawLine(new Vector3(transform.TransformPoint(p3).x, waterLevel, transform.TransformPoint(p3).z), new Vector3(transform.TransformPoint(p1).x, waterLevel, transform.TransformPoint(p1).z), Color.green);

                    Debug.DrawLine(new Vector3(transform.TransformPoint(p1).x, waterLevel, transform.TransformPoint(p1).z), new Vector3(transform.TransformPoint(p1).x, Center(p1, p2, p3).y, transform.TransformPoint(p1).z), Color.green);
                    Debug.DrawLine(new Vector3(transform.TransformPoint(p2).x, waterLevel, transform.TransformPoint(p2).z), new Vector3(transform.TransformPoint(p2).x, Center(p1, p2, p3).y, transform.TransformPoint(p2).z), Color.green);
                    Debug.DrawLine(new Vector3(transform.TransformPoint(p3).x, waterLevel, transform.TransformPoint(p3).z), new Vector3(transform.TransformPoint(p3).x, Center(p1, p2, p3).y, transform.TransformPoint(p3).z), Color.green);
                }
            }
        }
    }

    //<summary>
    //Get the center point of a triangle in world space
    //</summary>
    Vector3 Center(Vector3 p1, Vector3 p2, Vector3 p3) {
        Vector3 center = (p1 + p2 + p3) / 3;
        return transform.TransformPoint(center);
    }

    //<summary>
    //Get the normal of a triangle in world space
    //</summary>
    Vector3 Normal(Vector3 p1, Vector3 p2, Vector3 p3) { 
        Vector3 normal = Vector3.Cross(transform.TransformPoint(p2) - transform.TransformPoint(p1), transform.TransformPoint(p3) - transform.TransformPoint(p1)).normalized;
        return normal;
    }

    //<summary>
    //Get the area of a triangle
    //</summary>
    float Area(Vector3 p1, Vector3 p2, Vector3 p3) {

        float a = Vector3.Distance(new Vector3(transform.TransformPoint(p1).x, Center(p1, p2, p3).y, transform.TransformPoint(p1).z), new Vector3(transform.TransformPoint(p2).x, Center(p1, p2, p3).y, transform.TransformPoint(p2).z));
        float c = Vector3.Distance(new Vector3(transform.TransformPoint(p3).x, Center(p1, p2, p3).y, transform.TransformPoint(p3).z), new Vector3(transform.TransformPoint(p1).x, Center(p1, p2, p3).y, transform.TransformPoint(p1).z));
        return (a * c * Mathf.Sin(Vector3.Angle(new Vector3(transform.TransformPoint(p2).x, Center(p1, p2, p3).y, transform.TransformPoint(p2).z) - new Vector3(transform.TransformPoint(p1).x, Center(p1, p2, p3).y, transform.TransformPoint(p1).z), new Vector3(transform.TransformPoint(p3).x, Center(p1, p2, p3).y, transform.TransformPoint(p3).z) - new Vector3(transform.TransformPoint(p1).x, Center(p1, p2, p3).y, transform.TransformPoint(p1).z)) * Mathf.Deg2Rad)) / 2;
    }
}