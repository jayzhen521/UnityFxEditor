using System.IO;

namespace Packages.FxEditor
{
    public class TimeInformation : DataObjectBase
    {
        private float duration =0.0f ;
        private float frameRate = 120;
        public TimeInformation(SceneConfig obj)
        {
            ObjectType = ObjectTypeTimeInformation;
            //-------------------
            
            duration = obj.duration;
            frameRate = obj.frameRate;
        }
        public override void Write(Stream stream)
        {
            Write(stream,duration);
            Write(stream,frameRate);
        }
    }
}