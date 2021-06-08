using System;
using UnityEngine;

namespace Packages.FxEditor
{
    
    [Serializable]
    public class ExportItem
    {
        public string filename = "";
        public GameObject exportRoot = null;
        public Camera camera = null;

    }
}