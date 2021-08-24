using UnityEngine;

namespace AQUAS
{
    /// <summary>
    /// Simple character move and look script for the demos, provided for convenience to save having to import standard assets.
    /// </summary>
    public class AQUAS_Walk : MonoBehaviour
    {
        public float m_moveSpeed = 10f;
        public CharacterController m_controller;

        void Start()
        {
            if (m_controller == null)
            {
                m_controller = GetComponent<CharacterController>();
            }
        }

        void Update()
        {
            if (m_controller != null && m_controller.enabled)
            {
                Vector3 forward = Input.GetAxis("Vertical") * transform.TransformDirection(Vector3.forward) * m_moveSpeed;
                m_controller.Move(forward * Time.deltaTime);
                Vector3 side = Input.GetAxis("Horizontal") * transform.TransformDirection(Vector3.right) * m_moveSpeed;
                m_controller.Move(side * Time.deltaTime);
                m_controller.SimpleMove(Physics.gravity);
            }
        }
    }
}