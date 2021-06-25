using System.Collections.Generic;
using UnityEngine;

namespace Packages.FxEditor.JsonTypes
{
    public class FxJsonFilter : FxJsonDataBase
    {
        public enum FilterTypes
        {
            
            [InspectorName("可调节")]Modifiable=0,
            [InspectorName("不可调节")]UnModifiable=1
            
        }
        
        public enum ScreenAspect
        {
            [InspectorName("16:9")]Screen_16x9=1,
            [InspectorName("4:3")]Screen_4x3,
            [InspectorName("1:1")]Screen_1x1,
            [InspectorName("3:4")]Screen_3x4,
            [InspectorName("9:16")]Screen_9x16
        }
        
        public bool EngineType = true;
        public FilterTypes type =FilterTypes.Modifiable;
        public List<ScreenAspect> supportSizes = new List<ScreenAspect>();
        public int totalDuration = 0;
        
        private void OnDrawGizmos()
        {
            
        }
        
    }
}