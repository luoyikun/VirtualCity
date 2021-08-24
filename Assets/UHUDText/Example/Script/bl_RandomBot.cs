using UnityEngine;


namespace HUDText
{
    public class bl_RandomBot : MonoBehaviour
    {

        [SerializeField] private bool Move = true;

        private Animator Anim;
        private float Rate;

        void Awake()
        {
            Anim = GetComponent<Animator>();
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
                Debug.Log("5s时间到了");
                if (!Agent.hasPath)
                {
                    if (Anim)
                    {
                        Anim.SetBool("IsRun", false);
                    }
                    return;
                }
                
            }

            if (!Agent.hasPath)
            {
                RandomBot();
            }
        }

        void RandomBot()
        {
            Vector3 randomDirection = Random.insideUnitSphere * 50;
            randomDirection += transform.position;
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 50, 1);
            Vector3 finalPosition = hit.position;
            Agent.SetDestination(finalPosition);

            if (Anim && Anim.GetBool("IsRun") == false)
            {
                Debug.Log("随机行走");
                Anim.SetBool("IsRun", true);
            }
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