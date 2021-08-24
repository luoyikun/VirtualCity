using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ywz
{
  [ExecuteInEditMode]
  public class HideGameObject : MonoBehaviour
  {
    void Awake()
    {
      gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
    }

    void OnEnable()
    {
      gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
    }
  }
}