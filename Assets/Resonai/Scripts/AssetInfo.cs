using UnityEngine;

namespace Ywz
{
  #if UNITY_EDITOR
  public class AssetInfo : AssetInfoInternal
  {
  }
  #else
  public class AssetInfo : MonoBehaviour
  {
  }
  #endif
}