using System;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.FxEditor.JsonTypes
{
    public class FxJsonTranslator : FxJsonDataBase
    {
        public enum TranslatorTypes
        {
            
            [InspectorName("转场")]Translator=1,
            [InspectorName("滤镜")]Filter
            
        }
        
        public enum ScreenAspect
        {
            [InspectorName("16:9")]Screen_16x9=1,
            [InspectorName("4:3")]Screen_4x3,
            [InspectorName("1:1")]Screen_1x1,
            [InspectorName("3:4")]Screen_3x4,
            [InspectorName("9:16")]Screen_9x16
        }
        public int duraiton = 0;
        public bool EngineType = true;
        public TranslatorTypes transType = TranslatorTypes.Translator;
        public List<ScreenAspect> supportSizes = new List<ScreenAspect>();
        
        private void OnDrawGizmos()
        {
            
        }
    }
}