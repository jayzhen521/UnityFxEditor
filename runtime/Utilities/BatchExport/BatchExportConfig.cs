using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.FxEditor
{
    public class BatchExportConfig : MonoBehaviour
    {
        
        public string suffix = "videofx.src";
        
        [Header("排除物体")]
        public List<GameObject> ExcludeGameObjects = new List<GameObject>();
        
        
        
        public List<ExportItem> ExportItems = new List<ExportItem>();
        

        private void OnDrawGizmos()
        {
            foreach (var exportItem in ExportItems)
            {
                if (exportItem.customizedFileName||exportItem.exportRoot==null) continue;
                exportItem.filename = exportItem.exportRoot.name + "." + suffix;
            }
        }


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