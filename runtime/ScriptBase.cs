using System.Collections.Generic;
using UnityEngine;

namespace Packages.FxEditor
{
    
    public class ScriptBase : MonoBehaviour
    {
        //public static List<ScriptBase> gScripts=new List<ScriptBase>();

        // public static ScriptBase[] GetAllScript()
        // {
        //     List<ScriptBase> scripts = new List<ScriptBase>();
        //     foreach (var scriptBase in gScripts)
        //     {
        //         if (scriptBase == null) continue;
        //         if (scriptBase.gameObject == null) continue;
        //         if(scripts.IndexOf(scriptBase)>=0)continue;
        //         scripts.Add(scriptBase);
        //     }
        //
        //     return scripts.ToArray();
        // }
        public ScriptBase()
        {
            
            //gScripts.Add(this);
        }

        public virtual void BeginExport()
        {
            
        }
        public virtual void UpdateAnimation()
        {
            
        }

        public virtual void EndExport()
        {
            
        }
    }
}