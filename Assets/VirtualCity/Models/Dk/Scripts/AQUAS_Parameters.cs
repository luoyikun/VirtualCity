using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
using UnityEngine.PostProcessing;
#endif
#if UNITY_POST_PROCESSING_STACK_V2 && AQUAS_PRESENT
using UnityEngine.Rendering.PostProcessing;
#endif

[System.Serializable]
public class AQUAS_Parameters{

    [System.Serializable]
    public class UnderWaterParameters {
        [Header("The following parameters apply for underwater only!")]
        [Space(5)]
        public float fogDensity = 0.1f;
        public Color fogColor;
#if UNITY_POST_PROCESSING_STACK_V1 && AQUAS_PRESENT
        [Space(5)]
        [Header("Post Processing Profiles (Must NOT be empty!)")]
        [Space(5)]
        public PostProcessingProfile underwaterProfile;
        public PostProcessingProfile defaultProfile;
#endif

#if UNITY_POST_PROCESSING_STACK_V2 && AQUAS_PRESENT
        [Space(5)]
        [Header("Post Processing Profiles (Must NOT be empty!)")]
        [Space(5)]
        public PostProcessProfile underwaterProfile;
        public PostProcessProfile defaultProfile;
#endif
    }

    [System.Serializable]
    public class GameObjects {
        [Header("Set the game objects required for underwater mode.")]
        [Space(5)]
        public GameObject mainCamera;
        public GameObject waterLens;
        public GameObject airLens;
        public GameObject bubble;
        [Space(5)]
        [Header("Set waterplanes array size = number of waterplanes")]
        public List<GameObject> waterPlanes = new List<GameObject>();
        public bool useSquaredPlanes;
    }

    [System.Serializable]
    public class WetLens {
        [Header("Set how long the lens stays wet after diving up.")]
        public float wetTime = 1;
        [Space(5)]
        [Header("Set how long the lens needs to dry.")]
        public float dryingTime = 1.5f;
        [Space(5)]
        public Texture2D[] sprayFrames;
        public Texture2D[] sprayFramesCutout;
        public float rundownSpeed = 72;
    }

    [System.Serializable]
    public class CausticSettings {
        [Header("The following values are 'Afloat'/'Underwater'")]
        public Vector2 causticIntensity = new Vector2(0.6f, 0.2f);
        public Vector2 causticTiling = new Vector2(300, 100);
        public float maxCausticDepth;
    }

    [System.Serializable]
    public class Audio {
        public AudioClip[] sounds;
        [Range(0,1)]
        public float underwaterVolume;
        [Range(0,1)]
        public float surfacingVolume;
        [Range(0, 1)]
        public float diveVolume;
    }

    [System.Serializable]
    public class BubbleSpawnCriteria {
        [Header("Spawn Criteria for big bubbles")]
        public int minBubbleCount = 20;
        public int maxBubbleCount = 40;
        [Space(5)]
        public float maxSpawnDistance=1;
        public float averageUpdrift = 3;
        [Space(5)]
        public float baseScale = 0.06f;
        public float avgScaleSummand = 0.15f;
        [Space(5)]
        [Header("Spawn Timer for initial dive")]
        public float minSpawnTimer = 0.005f;
        public float maxSpawnTimer = 0.03f;
        [Space(5)]
        [Header("Spawn Timer for long dive")]
        public float minSpawnTimerL = 0.1f;
        public float maxSpawnTimerL = 0.5f;
    }

}
