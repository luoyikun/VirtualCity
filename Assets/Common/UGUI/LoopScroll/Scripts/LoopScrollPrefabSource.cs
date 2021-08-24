using UnityEngine;
using System.Collections;

public enum EnPoolFrom
{
    Tmp,
    Resource
}
namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource 
    {
        public EnPoolFrom m_poolFrom;
        public GameObject m_objTmp;//实例化的
        public string prefabName;
        public int poolSize = 5;

        private bool inited = false;
        public virtual GameObject GetObject()
        {
            if(!inited)
            {
                if (m_poolFrom == EnPoolFrom.Resource)
                {
                    SG.ResourceManager.Instance.InitPool(prefabName, poolSize);
                }
                else if (m_poolFrom == EnPoolFrom.Tmp ){
                    SG.ResourceManager.Instance.InitPoolByTmp(m_objTmp, poolSize);
                }
                inited = true;
            }
            return SG.ResourceManager.Instance.GetObjectFromPool(prefabName);
        }

        public virtual void ReturnObject(Transform go)
        {
            go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            SG.ResourceManager.Instance.ReturnObjectToPool(go.gameObject);
        }
    }
}
