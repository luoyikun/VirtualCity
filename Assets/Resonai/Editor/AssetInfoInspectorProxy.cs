using UnityEditor;

namespace Ywz
{
  [CustomEditor(typeof(AssetInfo))]
  public class AssetInfoInspectorProxy : Editor
  {
    AssetInfoInspector inspector;

    public AssetInfoInspectorProxy()
    {
      inspector = new AssetInfoInspector(this);
    }

    private void OnEnable()
    {
      inspector.SetTarget(target);
    }

    private void OnDisable()
    {
      inspector.OnDisable();
    }

    public override void OnInspectorGUI()
    {
      inspector.OnInspectorGUI();
    }
  }
}