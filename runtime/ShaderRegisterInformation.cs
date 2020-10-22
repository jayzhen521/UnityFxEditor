using System;
using UnityEngine;

namespace Packages.FxEditor
{
    public class ShaderRegisterInformation
    {
        
        public Int64 ShaderUUID = 0;
        // public string GLES3Source = "";
        // public string GLCoreSource = "";

        public string[] sources = new string[ShaderObject.MaxSources];
        public int[] states =new int[ShaderObject.MaxStates];
        public ShaderRegisterInformation(Int64 _uuid)
        {
            ShaderUUID = _uuid;
            for (var i = 0; i < ShaderObject.MaxStates; i++)
            {
                states[i] = -1;
            }
            for (var i = 0; i < ShaderObject.MaxSources; i++)
            {
                sources[i] = "none";
            }
        }
    }
}