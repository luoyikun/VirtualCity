using System.Collections.Generic;
using UnityEngine;

namespace AQUAS
{
    /// <summary>
    /// A simple mouse look script for the demos, provided for convenience to avoid having to import standard assets.
    /// Copied with thanks from here in unity forums : https://forum.unity3d.com/threads/a-free-simple-smooth-mouselook.73117/#post-3101292
    /// </summary>
    public class AQUAS_Look : MonoBehaviour
    {
        [Header("Info")]
        private List<float> _rotArrayX = new List<float>();
        private List<float> _rotArrayY = new List<float>();
        private float rotAverageX;
        private float rotAverageY;
        private float mouseDeltaX;
        private float mouseDeltaY;

        [Header("Settings")]
        public bool _isLocked;
        public float _sensitivityX = 1.5f;
        public float _sensitivityY = 1.5f;
        [Tooltip("The more steps, the smoother it will be.")]
        public int _averageFromThisManySteps = 3;

        [Header("References")]
        [Tooltip("Object to be rotated when mouse moves left/right.")]
        public Transform _playerRootT;
        [Tooltip("Object to be rotated when mouse moves up/down.")]
        public Transform _cameraT;

        //============================================
        // FUNCTIONS (UNITY)
        //============================================

        void Update()
        {
            MouseLookAveraged();
        }

        //============================================
        // FUNCTIONS (CUSTOM)
        //============================================

        void MouseLookAveraged()
        {
            rotAverageX = 0f;
            rotAverageY = 0f;
            mouseDeltaX = 0f;
            mouseDeltaY = 0f;

            mouseDeltaX += Input.GetAxis("Mouse X") * _sensitivityX;
            mouseDeltaY += Input.GetAxis("Mouse Y") * _sensitivityY;

            // Add current rot to list, at end
            _rotArrayX.Add(mouseDeltaX);
            _rotArrayY.Add(mouseDeltaY);

            // Reached max number of steps? Remove oldest from list
            if (_rotArrayX.Count >= _averageFromThisManySteps)
                _rotArrayX.RemoveAt(0);

            if (_rotArrayY.Count >= _averageFromThisManySteps)
                _rotArrayY.RemoveAt(0);

            // Add all of these rotations together
            for (int i_counterX = 0; i_counterX < _rotArrayX.Count; i_counterX++)
                rotAverageX += _rotArrayX[i_counterX];

            for (int i_counterY = 0; i_counterY < _rotArrayY.Count; i_counterY++)
                rotAverageY += _rotArrayY[i_counterY];

            // Get average
            rotAverageX /= _rotArrayX.Count;
            rotAverageY /= _rotArrayY.Count;

            // Apply
            _playerRootT.Rotate(0f, rotAverageX, 0f, Space.World);
            _cameraT.Rotate(-rotAverageY, 0f, 0f, Space.Self);
        }
    }
}