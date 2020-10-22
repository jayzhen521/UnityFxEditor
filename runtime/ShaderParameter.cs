using UnityEngine;
using UnityEngine.Rendering;

namespace Packages.FxEditor
{
    public class ShaderParameter
    {
        public const int ParameterTypeFloat = 0;
        public const int ParameterTypeFloat2 = 1;
        public const int ParameterTypeFloat3 = 2;
        public const int ParameterTypeFloat4 = 3;
        
        

        public const int ParameterTypeTexture2D = 4;
        public const int ParameterTypeMatrix4x4 = 5;
        public const int ParameterTypeColor = 6;

        //--------------------------------------
        public Color colorValue = Color.white;
        public float floatValue = 0.0f;
        public ulong ID = 0;
        public int intValue = 0;
        public string name = "";
        public TextureObject textureValue = null;
        public int type = 0;

        public Vector4 vectorValue = Vector4.zero;

        public ShaderParameter(string _name, int t)
        {
            name = _name;
            type = t;
        }
        public static int GetTypeFromPropertyType(ShaderPropertyType t)
        {
            switch (t)
            {
                case ShaderPropertyType.Color:
                    return ParameterTypeColor;
                case ShaderPropertyType.Float:
                    return ParameterTypeFloat;
                case ShaderPropertyType.Texture:
                    return ParameterTypeTexture2D;
                case ShaderPropertyType.Vector:
                    return ParameterTypeFloat4;
            }
            return 0;
        }
    }
}