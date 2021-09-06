using UnityEditor;

namespace TouchControlsKit.Inspector
{
    [CustomEditor( typeof( TCKTouchpad ) )]
    public class TCKTouchpadEditor : AxesBasedControllerEditor
    {
        // OnEnable
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        
        // ShowParameters
        protected override void ShowParameters()
        {
            DrawUpdateMode();
            DrawEnable( false );
            DrawIdentProp();
            DrawTouchZone();
            DrawSensitivityProp();
        }
    };
}