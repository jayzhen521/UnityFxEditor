using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class FxJsonDataBase : MonoBehaviour
    {
        public void SaveTo(string filepath)
        {
            var text = JsonUtility.ToJson(this);
            File.WriteAllText(filepath, text);
        }
    }
}