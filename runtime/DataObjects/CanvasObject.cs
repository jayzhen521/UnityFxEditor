namespace Packages.FxEditor
{
    public class CanvasObject : DataObjectBase
    {
        public FxCanvasObject canvas = null;
        public CanvasObject(FxCanvasObject obj)
        {
            canvas = obj;
        }
    }
}