using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Vc
{
    public class RandomWalk : MonoBehaviour
    {
        [SerializeField] private bool Move = true;

        private float Rate = 0;
        Coroutine m_cor;
        void Awake()
        {
            
        }

        void Update()
        {
            //if (!Move)
            //{
            //    Anim.SetBool("IsRun", false);
            //    return;
            //}

            if (Rate > Time.time)
            {
                if (!Agent.hasPath)
                {
                    return;
                }

            }

            if (!Agent.hasPath)
            {
                if (m_cor != null)
                {
                    StopCoroutine(m_cor);
                }


                m_cor = StartCoroutine(YieldRandom());
            }
        }

        IEnumerator YieldRandom()
        {
            
            RandomBot();
            yield return null;
        }
        void RandomBot()
        {
            Vector3 randomDirection = Random.insideUnitSphere * 50;
            randomDirection += transform.position;
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 50, 1);
            Vector3 finalPosition = hit.position;
            Agent.SetDestination(finalPosition);

          
            Rate = Time.time + 5;
        }

        private UnityEngine.AI.NavMeshAgent m_Agent;
        private UnityEngine.AI.NavMeshAgent Agent
        {
            get
            {
                if (m_Agent == null)
                {
                    m_Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                }
                return m_Agent;
            }
        }
    }
}
