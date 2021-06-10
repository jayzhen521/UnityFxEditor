using System;
using UnityEngine;

namespace Packages.FxEditor
{
    
    [Serializable]
    public class ExportItem
    {
        public bool customizedFileName = false;
        public string filename = "";
        public GameObject exportRoot = null;
        public Camera camera = null;

    }
}