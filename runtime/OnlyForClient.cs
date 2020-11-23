using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Packages.FxEditor
{
    public class OnlyForClient
    {
        [MenuItem("FxEditor/更新")]
        public static void OnUpdate()
        {
            if (SystemInfo.deviceName == "Henry’s MacBook Pro") return;
            Debug.Log("Updating....");
            Client.Add("https://github.com/Helin777/UnityFxEditor.git");
            Debug.Log("Update finish!");
        }
    }
}