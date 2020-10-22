namespace Packages.FxEditor
{
    public class EndCanvasCommand:CommandObjectBase
    {
        public EndCanvasCommand(FxCanvasObject obj)
        {
            ObjectType = CommandTypeEndCanvas;
            //-----------------------------------
        }
        
    }
}