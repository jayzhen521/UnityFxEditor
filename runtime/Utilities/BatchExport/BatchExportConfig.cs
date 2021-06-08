using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.FxEditor
{
    public class BatchExportConfig : MonoBehaviour
    {
        [Header("排除物体")]
        public List<GameObject> ExcludeGameObjects = new List<GameObject>();
        public List<ExportItem> ExportItems = new List<ExportItem>();
        


        [MenuItem("FxEditor/创建物体/批量导出 _s")]
        static public void OnMenuItem()
        {
            if (Object.FindObjectOfType<BatchExportConfig>() != null)
            {
                Debug.LogError("已经创建批量导出配置");
                return;
            }
            
            
            GameObject gameObject = new GameObject("BatchExportConfig");
            gameObject.AddComponent<BatchExportConfig>();
        }
    }
}