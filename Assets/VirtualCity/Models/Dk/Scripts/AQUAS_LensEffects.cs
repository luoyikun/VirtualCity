using UnityEngine;
using System.Reflection;
#if UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
using UnityEngine.PostProcessing;
#endif
#if UNITY_POST_PROCESSING_STACK_V2 && AQUAS_PRESENT
using UnityEngine.Rendering.PostProcessing;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AQUAS_LensEffects : MonoBehaviour {

#region instantiate objects of parameter classes (pseudo structs)
    //<summary>
    //Parameters are stored in external classes
    //for convenience reasons
    //</summary>
    public AQUAS_Parameters.UnderWaterParameters underWaterParameters = new AQUAS_Parameters.UnderWaterParameters();
    public AQUAS_Parameters.GameObjects gameObjects = new AQUAS_Parameters.GameObjects();
    public AQUAS_Parameters.BubbleSpawnCriteria bubbleSpawnCriteria = new AQUAS_Parameters.BubbleSpawnCriteria();
    public AQUAS_Parameters.WetLens wetLens = new AQUAS_Parameters.WetLens();
    public AQUAS_Parameters.CausticSettings causticSettings = new AQUAS_Parameters.CausticSettings();
    public AQUAS_Parameters.Audio soundEffects = new AQUAS_Parameters.Audio();
#endregion

#region additional (mostly) private variables

    int sprayFrameIndex;

    GameObject tenkokuObj;

	//Material waterLensMaterial;
	Material airLensMaterial;
	Material waterPlaneMaterial;

    [HideInInspector]
    public float t;
	float t2;
	float bubbleSpawnTimer;

    //Default values (while afloat)
    float defaultFogDensity;
    Color defaultFogColor;

    float defaultFoamContrast;
	float defaultBloomIntensity;
	float defaultSpecularity;
	float defaultRefraction;

	bool defaultFog;
	bool defaultSunShaftsEnabled;
	bool defaultBloomEnabled;
	bool defaultBlurEnabled;
	bool defaultVignetteEnabled;
	bool defaultNoiseEnabled;

	public bool underWater { get; private set; }

	[HideInInspector]
	public bool setAfloatFog = true;
    [HideInInspector]
    public bool rundown;

    //audio
	bool playSurfaceSplash;
    bool playDiveSplash;
    bool playUnderwater;

	//bubble parameters
	int bubbleCount;
	int maxBubbleCount;
	int activePlane;
    int lastActivePlane=100;

    FieldInfo fi;

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
    Bloom bloom;
    GlobalFog globalFog;
    BlurOptimized blur;
    VignetteAndChromaticAberration vignette;
    NoiseAndGrain noiseAndGrain;
    SunShafts sunShafts;
#endif

#if UNITY_5_6_OR_NEWER && UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
    PostProcessingBehaviour postProcessing;
#endif

#if UNITY_5_6_OR_NEWER && UNITY_POST_PROCESSING_STACK_V2 && AQUAS_PRESENT
    PostProcessLayer postProcessing;
    PostProcessVolume postProcessingVolume;
#endif

    AudioSource waterLensAudio;
    AudioSource airLensAudio;
    AudioSource audioComp;
    AudioSource cameraAudio;

    Projector primaryCausticsProjector;
    Projector secondaryCausticsProjector;
    AQUAS_Caustics primaryAquasCaustics;
    AQUAS_Caustics secondaryAquasCaustics;

    AQUAS_BubbleBehaviour bubbleBehaviour;
#endregion

    //<summary>
    //Initializes the state at start
    //Grabs the default values and stores them into the appropriate variables
    //</summary>
    void Start () {

    // Disables Caustics in Unity 2018 to avoid rendering issues - will be removed once it is certain, that there will be no more rendering issues whatsoever.
/*#if UNITY_2018 && UNITY_EDITOR

        if (gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector").gameObject.activeSelf || gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector").gameObject.activeSelf)
        {
            if (EditorUtility.DisplayDialog("Caustics Warning", "You are using Unity 2018. AQUAS is currently experiencing some issues with caustic effects in Unity 2018. It is recommended to keep the caustic projectors disabled until a bugfix is ready. Do you want to disable the caustic projectors?", "Disable Caustics", "Keep Caustics Enabled"))
            {
                gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector").gameObject.SetActive(false);
                gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector").gameObject.SetActive(false);
            }
        }
#endif*/

        //Set up the post processing on the camera
#if UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
        if (gameObjects.mainCamera.GetComponent<PostProcessingBehaviour>() == null)
        {
            gameObjects.mainCamera.AddComponent<PostProcessingBehaviour>();
        }

        postProcessing = gameObjects.mainCamera.GetComponent<PostProcessingBehaviour>();
#endif

#if UNITY_POST_PROCESSING_STACK_V2 && UNITY_EDITOR && AQUAS_PRESENT
        if (gameObjects.mainCamera.GetComponent<PostProcessLayer>() == null)
        {
            EditorUtility.DisplayDialog("No Post Process Layer detected", "The camera object is missing a Post Process Layer and a Post Process Volume. In the Editor AQUAS will try to add them when entering play mode. However it is recommended that you add them manually before entering playmode, or else they will be missing in the build.", "Got It!");

            gameObjects.mainCamera.AddComponent<PostProcessLayer>();

            postProcessing = gameObjects.mainCamera.GetComponent<PostProcessLayer>();
            PostProcessResources resources;

            if ((PostProcessResources)AssetDatabase.LoadAssetAtPath("Assets/PostProcessing-2/PostProcessing/PostProcessResources.asset", typeof(PostProcessResources)) != null)
            {
                resources = (PostProcessResources)AssetDatabase.LoadAssetAtPath("Assets/PostProcessing-2/PostProcessing/PostProcessResources.asset", typeof(PostProcessResources));
            }
            else if ((PostProcessResources)AssetDatabase.LoadAssetAtPath("Assets/PostProcessing/PostProcessResources.asset", typeof(PostProcessResources)) != null)
            {
                resources = (PostProcessResources)AssetDatabase.LoadAssetAtPath("Assets/PostProcessing/PostProcessResources.asset", typeof(PostProcessResources));
            }
            else if ((PostProcessResources)AssetDatabase.LoadAssetAtPath("Assets/PostProcessing-2/PostProcessResources.asset", typeof(PostProcessResources)) != null)
            {
                resources = (PostProcessResources)AssetDatabase.LoadAssetAtPath("Assets/PostProcessing-2/PostProcessResources.asset", typeof(PostProcessResources));
            }
            else
            {
                EditorUtility.DisplayDialog("Could not locate Post Process Resource file.", "Please make sure your post processing folder is at the top level of the assets folder and named either 'PostProcessing' or 'PostProcessing-2'","Got It!");
                resources = null;
            }


            postProcessing.Init(resources);

            postProcessing.volumeTrigger = gameObjects.mainCamera.transform;
            postProcessing.volumeLayer = LayerMask.NameToLayer("Everything");
        }

        if (gameObjects.mainCamera.GetComponent<PostProcessVolume>() == null)
        {
            gameObjects.mainCamera.AddComponent<PostProcessVolume>();
            gameObjects.mainCamera.GetComponent<PostProcessVolume>().isGlobal = true;
        }
        
        postProcessingVolume = gameObjects.mainCamera.GetComponent<PostProcessVolume>();
#endif


#if UNITY_EDITOR && AQUAS_PRESENT
#if UNITY_POST_PROCESSING_STACK_V1 || UNITY_POST_PROCESSING_STACK_V2
        if (underWaterParameters.defaultProfile == null)
        {
            EditorUtility.DisplayDialog("WARNING! - Post default post processing profile missing!", "The post processing profiles in the inspector of the underwater camera effects are missing a default profile! It's not recommended to leave the default profile empty. If you don't want to use post processing while afloat, you can use a profile with all image effects disabled", "Got it!");
        }
#endif
#endif

        waterLensAudio = gameObjects.waterLens.GetComponent<AudioSource>();
        airLensAudio = gameObjects.airLens.GetComponent<AudioSource>();
        audioComp = GetComponent<AudioSource>();
        cameraAudio = gameObjects.mainCamera.GetComponent<AudioSource>();

        bubbleBehaviour = gameObjects.bubble.GetComponent<AQUAS_BubbleBehaviour>();

        //Set initially active lenses
        gameObjects.airLens.SetActive (true);
        gameObjects.waterLens.SetActive (false);

		//Assign materials
		//waterLensMaterial = gameObjects.waterLens.GetComponent<Renderer> ().material;
		airLensMaterial = gameObjects.airLens.GetComponent<Renderer> ().material;

		waterPlaneMaterial = gameObjects.waterPlanes[0].GetComponent<Renderer> ().material;

		t = wetLens.wetTime+wetLens.dryingTime;
		t2 = 0;
		bubbleSpawnTimer = 0;

		//Initialize default values for ---
        //--- global fog
		defaultFog = RenderSettings.fog;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultFogColor = RenderSettings.fogColor;

        /*if (globalFog != null)
        {
            globalFog.enabled = defaultFog;
        }*/

        //--- Some water parameters
		defaultFoamContrast = waterPlaneMaterial.GetFloat ("_FoamContrast");
		defaultSpecularity = waterPlaneMaterial.GetFloat ("_Specular");

        if(waterPlaneMaterial.HasProperty("_Refraction"))
        {
            defaultRefraction = waterPlaneMaterial.GetFloat("_Refraction");
        }

#if UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
        postProcessing.profile = underWaterParameters.defaultProfile;
#endif

#if UNITY_POST_PROCESSING_STACK_V2 && AQUAS_PRESENT
        postProcessingVolume.profile = underWaterParameters.defaultProfile;
#endif

        audioComp.clip = soundEffects.sounds[0];
        audioComp.loop = true;
        audioComp.Stop();
        airLensAudio.clip = soundEffects.sounds[1];
        airLensAudio.loop = false;
        airLensAudio.Stop();
        waterLensAudio.clip = soundEffects.sounds[2];
        waterLensAudio.loop = false;
        waterLensAudio.Stop();

        //Check if Tenkoku is in the scene
        if (GameObject.Find("Tenkoku DynamicSky") != null)
        {
            tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
        }
    }

    //<summary>
    //Update is called once per frame
    //Continuous effects are based on two timers, one for underwater & one for afloat
    //Being underwater or afloat resets the other timer respectively
    //</summary>
    void Update () {

        CheckIfStillUnderWater();

        ///<summary>
        ///define behaviour under water & reset timer
        ///</summary>
        if (underWater) {
#region Underwater Behaviour
        
            t=0;
            t2 += Time.deltaTime;   

            //Switches air lens for water lens
            gameObjects.airLens.SetActive(false);
            gameObjects.waterLens.SetActive (true);

            //Resets the image sequence for the spray animation on the lens
            sprayFrameIndex = 0;
			rundown=true;

            BubbleSpawner();

            //Controls underwater sound
#region Underwater Audio
            if (playUnderwater)
            {
                audioComp.Play();
                playUnderwater = false;
            }

            if (playDiveSplash)
            {
                waterLensAudio.Play();
                playDiveSplash = false;
            }

            playSurfaceSplash = true;

            airLensAudio.Stop();
            cameraAudio.enabled = false;

            airLensAudio.volume = soundEffects.surfacingVolume;
            audioComp.volume = soundEffects.diveVolume;
            waterLensAudio.volume = soundEffects.underwaterVolume;
            //Add custom code for audio
#endregion

            //Controls caustic behaviour and size while underwater
#region Caustics Control Underwater
            if (primaryCausticsProjector!=null)
            {
                 primaryCausticsProjector.material.SetTextureScale ("_Texture", new Vector2(causticSettings.causticTiling.y, causticSettings.causticTiling.y));
                 primaryCausticsProjector.material.SetFloat ("_Intensity", causticSettings.causticIntensity.y);
                 primaryAquasCaustics.maxCausticDepth=causticSettings.maxCausticDepth;
            }

			if(secondaryCausticsProjector!=null)
            {
                secondaryCausticsProjector.material.SetTextureScale ("_Texture", new Vector2(causticSettings.causticTiling.y, causticSettings.causticTiling.y));
                secondaryCausticsProjector.material.SetFloat ("_Intensity", causticSettings.causticIntensity.y);
                secondaryAquasCaustics.maxCausticDepth = causticSettings.maxCausticDepth;
            }
#endregion

            //Enables underwater mode in the water material
#region Set Underwater Parameters
            waterPlaneMaterial.SetFloat("_UnderwaterMode",1);
			waterPlaneMaterial.SetFloat ("_FoamContrast", 0);
			waterPlaneMaterial.SetFloat ("_Specular", defaultSpecularity * 5);
			waterPlaneMaterial.SetFloat ("_Refraction", 0.7f);
            #endregion

            //Enables Camera Effects and sets bloom value for underwater mode
            #region Enable Image Effects
#if UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
            if(postProcessing.profile != underWaterParameters.underwaterProfile)
            {
                postProcessing.profile = underWaterParameters.underwaterProfile;
            }
#endif
#if UNITY_POST_PROCESSING_STACK_V2 && AQUAS_PRESENT
            if (postProcessingVolume.profile != underWaterParameters.underwaterProfile)
            {
                postProcessingVolume.profile = underWaterParameters.underwaterProfile;
                postProcessingVolume.sharedProfile = underWaterParameters.underwaterProfile;
            }
#endif
            #endregion

            //Enables underwater fog and sets fog parameters for underwater mode
            #region Enable Underwater Fog    

            if (tenkokuObj!=null)
            {
                var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
                FieldInfo enableTenkokuFog = tenkokuModule.GetType().GetField("enableFog", BindingFlags.Public | BindingFlags.Instance);
                if (enableTenkokuFog != null) enableTenkokuFog.SetValue(tenkokuModule, false);
            }

            RenderSettings.fog = true;
#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            if (globalFog != null)
            {
                globalFog.enabled = true;
            }
#endif
            //FieldInfo enableGlobalFog = gameObjects.mainCamera.GetComponent("GlobalFog").GetType().GetField("enabled", BindingFlags.Public | BindingFlags.Instance);
            //if (enableGlobalFog != null) { enableGlobalFog.SetValue(gameObjects.mainCamera.GetComponent("GlobalFog"), true); }

            RenderSettings.fogDensity = underWaterParameters.fogDensity;
			RenderSettings.fogColor = underWaterParameters.fogColor;
#endregion
#endregion
        }

        ///<summary>
        ///define behaviour afloat & reset timer
        ///</summary>
        else
        {
#region Afloat Behaviour

            t2 = 0;
            t += Time.deltaTime;
            
            //Switches water lens for air lens
            gameObjects.airLens.SetActive(true);
            gameObjects.waterLens.SetActive (false);
            
            //Initiates wet lens animation
			if(rundown)
            {
				sprayFrameIndex=0;
				NextFrame ();
				InvokeRepeating("NextFrame",1/wetLens.rundownSpeed,1/wetLens.rundownSpeed);
				rundown=false;
			}
            
            //Resets bubble parameters and randomizes new maxBubbleCount based on parameters set
			bubbleCount = 0;
			maxBubbleCount = (int)Random.Range (bubbleSpawnCriteria.minBubbleCount, bubbleSpawnCriteria.maxBubbleCount);
			bubbleSpawnTimer = 0;

            //Controls afloat sound
#region Afloat Audio
            if (playSurfaceSplash)
            {
                airLensAudio.Play();
                playSurfaceSplash = false;
            }

            playUnderwater = true;
            playDiveSplash = true;

            audioComp.Stop();
            waterLensAudio.Stop();
            cameraAudio.enabled = true;
            //Add custom code for audio
#endregion

            //Controls caustic behaviour and size while afloat
#region Caustics Control Afloat
            if (primaryCausticsProjector!=null)
            {
                 primaryCausticsProjector.material.SetTextureScale ("_Texture", new Vector2(causticSettings.causticTiling.x, causticSettings.causticTiling.x));
                 primaryCausticsProjector.material.SetFloat("_Intensity", causticSettings.causticIntensity.x);
			}
			
			if(secondaryCausticsProjector!=null)
            {
                secondaryCausticsProjector.material.SetTextureScale ("_Texture", new Vector2(causticSettings.causticTiling.x, causticSettings.causticTiling.x));
                secondaryCausticsProjector.material.SetFloat("_Intensity", causticSettings.causticIntensity.x);
			}
#endregion

            //Sets the air lens parameters during wet and drying time after diving up
#region Wet Lens Effect
            if (t<=wetLens.wetTime){

                //vignette.blur=0.75f;

				airLensMaterial.SetFloat ("_Refraction", 1);
				airLensMaterial.SetFloat("_Transparency", 0.01f);
			} 

            else 
            {

                //vignette.blur=Mathf.Lerp(0.75f,0,(t-wetLens.wetTime)/wetLens.dryingTime);

				airLensMaterial.SetFloat ("_Refraction", Mathf.Lerp(1,0,(t-wetLens.wetTime)/wetLens.dryingTime));
				airLensMaterial.SetFloat ("_Transparency", Mathf.Lerp(0.01f,0,(t-wetLens.wetTime)/wetLens.dryingTime));
			}
#endregion

            //Disables underwater mode in the water material
#region Set Afloat Parameters
            waterPlaneMaterial.SetFloat ("_FoamContrast", defaultFoamContrast);
			waterPlaneMaterial.SetFloat("_UnderwaterMode",0);
			waterPlaneMaterial.SetFloat ("_Specular", defaultSpecularity);
			waterPlaneMaterial.SetFloat ("_Refraction", defaultRefraction);
            #endregion

            //Disables Camera Effects for and sets bloom value for afloat mode
            #region Disable Image Effects
#if UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
            if(postProcessing.profile != underWaterParameters.defaultProfile)
            {
                postProcessing.profile = underWaterParameters.defaultProfile;
            }
#endif

#if UNITY_POST_PROCESSING_STACK_V2 && AQUAS_PRESENT
            if (postProcessingVolume.profile != underWaterParameters.defaultProfile)
            {
                postProcessingVolume.profile = underWaterParameters.defaultProfile;
                postProcessingVolume.sharedProfile = underWaterParameters.defaultProfile;
            }
#endif
            #endregion

            //Disables underwater fog and sets fog parameters back to default
            #region Disable Underwater Fog

            if (tenkokuObj != null)
            {
                var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
                FieldInfo enableTenkokuFog = tenkokuModule.GetType().GetField("enableFog", BindingFlags.Public | BindingFlags.Instance);
                if (enableTenkokuFog != null) enableTenkokuFog.SetValue(tenkokuModule, true);
            }

            RenderSettings.fog = defaultFog;
#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
            if (globalFog != null && setAfloatFog)
            {
                globalFog.enabled = defaultFog;
            }
#endif
            //FieldInfo enableGlobalFog = gameObjects.mainCamera.GetComponent("GlobalFog").GetType().GetField("enabled", BindingFlags.Public | BindingFlags.Instance);
            //if (enableGlobalFog != null) { enableGlobalFog.SetValue(gameObjects.mainCamera.GetComponent("GlobalFog"), defaultFog); }

			if(setAfloatFog)
			{
				RenderSettings.fogColor = defaultFogColor;
				RenderSettings.fogDensity = defaultFogDensity;
			}
            
#endregion
#endregion
        }
    }

    //<summary>
    //Checks if the camera moves into the cylinder or quad mounted by the corresponding waterplane
    //Works with circular and squared planes
    //<returns>underWater</returns>
    //</summary>
	bool CheckIfUnderWater(int waterPlanesCount){
		
		if (!gameObjects.useSquaredPlanes) {

			for (int i=0; i < waterPlanesCount; i++) {

				if (Mathf.Pow ((transform.position.x - gameObjects.waterPlanes[i].transform.position.x), 2) + Mathf.Pow ((transform.position.z - gameObjects.waterPlanes[i].transform.position.z), 2) < Mathf.Pow (gameObjects.waterPlanes[i].GetComponent<Renderer> ().bounds.extents.x, 2)) {

                    if (activePlane != lastActivePlane)
                    {
                        if (gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector") != null)
                        {
                            primaryCausticsProjector = gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<Projector>();
                            primaryAquasCaustics = gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<AQUAS_Caustics>();
                        }

                        if (gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector") != null)
                        {
                            secondaryCausticsProjector = gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<Projector>();
                            secondaryAquasCaustics = gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<AQUAS_Caustics>();
                        }

                        lastActivePlane = activePlane;
                    }

                    activePlane = i;

                    if (transform.position.y < gameObjects.waterPlanes[i].transform.position.y) {

						waterPlaneMaterial = gameObjects.waterPlanes[i].GetComponent<Renderer> ().material;
						activePlane = i;
						return true;
						//break;
					}
				}
			}
		} else {

			for (int i=0; i < waterPlanesCount; i++) {

				if (Mathf.Abs(transform.position.x - gameObjects.waterPlanes[i].transform.position.x) < gameObjects.waterPlanes[i].GetComponent<Renderer>().bounds.extents.x && Mathf.Abs(transform.position.z - gameObjects.waterPlanes[i].transform.position.z)  < gameObjects.waterPlanes[i].GetComponent<Renderer> ().bounds.extents.z) {

                    if (activePlane != lastActivePlane)
                    {
                        if (gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector") != null)
                        {
                            primaryCausticsProjector = gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<Projector>();
                            primaryAquasCaustics = gameObjects.waterPlanes[activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<AQUAS_Caustics>();
                        }

                        if (gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector") != null)
                        {
                            secondaryCausticsProjector = gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<Projector>();
                            secondaryAquasCaustics = gameObjects.waterPlanes[activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<AQUAS_Caustics>();
                        }

                        lastActivePlane = activePlane;
                    }

                    activePlane = i;
                    
                    if (transform.position.y < gameObjects.waterPlanes[i].transform.position.y) {

						waterPlaneMaterial = gameObjects.waterPlanes[0].GetComponent<Renderer> ().material;
						activePlane = i;
						return true;
						//break;
					}
				}
			}
		}
		return false;
	}

    //<summary>
    //Once underwater, checks if still underwater
    //</summary>
    void CheckIfStillUnderWater() {

        if (!gameObjects.useSquaredPlanes)
        {

            if (underWater && Mathf.Pow((transform.position.x - gameObjects.waterPlanes[activePlane].transform.position.x), 2) + Mathf.Pow((transform.position.z - gameObjects.waterPlanes[activePlane].transform.position.z), 2) > Mathf.Pow(gameObjects.waterPlanes[activePlane].GetComponent<Renderer>().bounds.extents.x, 2))
            {
                underWater = false;
            }

            else if (underWater && transform.position.y > gameObjects.waterPlanes[activePlane].transform.position.y)
            {
                underWater = false;
            }

            else if (!underWater)
            {
                underWater = CheckIfUnderWater(gameObjects.waterPlanes.Count);
            }
        }
        else {

            if (underWater && Mathf.Abs(transform.position.x - gameObjects.waterPlanes[activePlane].transform.position.x) > gameObjects.waterPlanes[activePlane].GetComponent<Renderer>().bounds.extents.x || underWater && Mathf.Abs(transform.position.z - gameObjects.waterPlanes[activePlane].transform.position.z) > gameObjects.waterPlanes[activePlane].GetComponent<Renderer>().bounds.extents.z)
            {
                underWater = false;
            }

            else if (underWater && transform.position.y > gameObjects.waterPlanes[activePlane].transform.position.y)
            {
                underWater = false;
            }

            else if (!underWater)
            {
                underWater = CheckIfUnderWater(gameObjects.waterPlanes.Count);
            }
        }
    }

    //<summary>
    //Handles the image sequence for the wet lens effect
    //</summary>
	void NextFrame(){
		if (sprayFrameIndex >= wetLens.sprayFrames.Length - 1) {
			sprayFrameIndex=0;
			CancelInvoke ("NextFrame");
		}
		airLensMaterial.SetTexture ("_CutoutReferenceTexture", wetLens.sprayFramesCutout [sprayFrameIndex]);
		airLensMaterial.SetTexture ("_Normal", wetLens.sprayFrames [sprayFrameIndex]);
		sprayFrameIndex = (sprayFrameIndex + 1);
	}

    //<summary>
    //Spawns bubbles according to the parameters set
    //Small bubbles being spawned directly by the bubbles
    //Small bubbles parameters & randomization are based on bubble parameters but are not directly controllable
    //</summary>
    void BubbleSpawner() {

        //Applies spawning rules for initial dive
#region Spawn for initial dive
        if (t2 > bubbleSpawnTimer && maxBubbleCount > bubbleCount)
        {

            float bubbleScaleFactor = Random.Range(0, bubbleSpawnCriteria.avgScaleSummand * 2);

            bubbleBehaviour.mainCamera = gameObjects.mainCamera;
            bubbleBehaviour.waterLevel = gameObjects.waterPlanes[activePlane].transform.position.y;
            bubbleBehaviour.averageUpdrift = bubbleSpawnCriteria.averageUpdrift + Random.Range(-bubbleSpawnCriteria.averageUpdrift * 0.75f, bubbleSpawnCriteria.averageUpdrift * 0.75f);
            
            gameObjects.bubble.transform.localScale += new Vector3(bubbleScaleFactor, bubbleScaleFactor, bubbleScaleFactor);
            
            Instantiate(gameObjects.bubble, new Vector3(transform.position.x + Random.Range(-bubbleSpawnCriteria.maxSpawnDistance, bubbleSpawnCriteria.maxSpawnDistance), transform.position.y - 0.4f, transform.position.z + Random.Range(-bubbleSpawnCriteria.maxSpawnDistance, bubbleSpawnCriteria.maxSpawnDistance)), Quaternion.identity);
            
            bubbleSpawnTimer += Random.Range(bubbleSpawnCriteria.minSpawnTimer, bubbleSpawnCriteria.maxSpawnTimer);
            
            bubbleCount += 1;
            
            gameObjects.bubble.transform.localScale = new Vector3(bubbleSpawnCriteria.baseScale, bubbleSpawnCriteria.baseScale, bubbleSpawnCriteria.baseScale);
        }
#endregion

        //Applies spawning rules for long dive
        //Definition for long dive: bubbleCount == maxBubbleCount
#region Spawn for long dive
        else if (t2 > bubbleSpawnTimer && maxBubbleCount == bubbleCount)
        {
            float bubbleScaleFactor = Random.Range(0, bubbleSpawnCriteria.avgScaleSummand * 2);

            bubbleBehaviour.mainCamera = gameObjects.mainCamera;
            bubbleBehaviour.waterLevel = gameObjects.waterPlanes[activePlane].transform.position.y;
            bubbleBehaviour.averageUpdrift = bubbleSpawnCriteria.averageUpdrift + Random.Range(-bubbleSpawnCriteria.averageUpdrift * 0.75f, bubbleSpawnCriteria.averageUpdrift * 0.75f);
            
            gameObjects.bubble.transform.localScale += new Vector3(bubbleScaleFactor, bubbleScaleFactor, bubbleScaleFactor);
            
            Instantiate(gameObjects.bubble, new Vector3(transform.position.x + Random.Range(-bubbleSpawnCriteria.maxSpawnDistance, bubbleSpawnCriteria.maxSpawnDistance), transform.position.y - 0.4f, transform.position.z + Random.Range(-bubbleSpawnCriteria.maxSpawnDistance, bubbleSpawnCriteria.maxSpawnDistance)), Quaternion.identity);
            
            bubbleSpawnTimer += Random.Range(bubbleSpawnCriteria.minSpawnTimerL, bubbleSpawnCriteria.maxSpawnTimerL);
            
            gameObjects.bubble.transform.localScale = new Vector3(bubbleSpawnCriteria.baseScale, bubbleSpawnCriteria.baseScale, bubbleSpawnCriteria.baseScale);
        }
#endregion
    }
}
