using UnityEngine;
using UnityEditor;

namespace TouchControlsKit.Inspector
{
    [CustomEditor( typeof( TCKButton ) )]
    public class TCKButtonEditor : ControllerBaseEditor
    {
        SerializedProperty swipeOutProp;
        SerializedProperty normalSpriteProp, pressedSpriteProp;
        SerializedProperty pressedColorProp;


        // OnEnable
        protected override void OnEnable()
        {
            base.OnEnable();

            swipeOutProp = serializedObject.FindProperty( "swipeOut" );

            normalSpriteProp = serializedObject.FindProperty( "normalSprite" );
            pressedSpriteProp = serializedObject.FindProperty( "pressedSprite" );

            pressedColorProp = serializedObject.FindProperty( "pressedColor" );            
        }


        // ShowParameters
        protected override void ShowParameters()
        {
            base.ShowParameters();

            DrawIdentProp();

            GUILayout.Space( 5f );
            TCKEditorHelper.DrawPropertyField( swipeOutProp );

            GUILayout.Space( 5f );

            using( var ecc = new TCKEditorChangeCheck() )
            {
                GUI.enabled = activeProp.boolValue && visibleProp.boolValue;
                TCKEditorHelper.DrawSpriteAndColor( baseImageObj, normalSpriteProp.GetLabel() );
                GUI.enabled = true;

                ecc.OnChangeCheck = () => 
                {
                    normalSpriteProp.objectReferenceValue = baseImageObj.FindProperty( "m_Sprite" ).objectReferenceValue;
                    baseImageColorProp.colorValue = baseImageObj.FindProperty( "m_Color" ).colorValue;
                };
            }
            
            TCKEditorHelper.DrawSpriteAndColor( pressedSpriteProp, pressedColorProp, pressedSpriteProp.GetLabel() );
        }
    };
}