using System.Collections.Generic;
using UnityEditor;

namespace TouchControlsKit.Inspector
{
    public static class TCKInitialize
    {
        const string ASSET_DEFINE = "INPUT_FOR_ANDROID";


        readonly static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
             {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS
            };


        // CheckDefines
        private static bool CheckDefines( string defineName )
        {
            foreach( var group in buildTargetGroups )
            {
                if( GetDefinesList( group ).Contains( defineName ) )
                {
                    return true;
                }
            }

            return false;
        }


        // SetEnabled
        private static void SetDefineEnabled( string defineName, bool enable )
        {
            foreach( var group in buildTargetGroups )
            {
                var defines = GetDefinesList( group );

                if( enable )
                {
                    if( defines.Contains( defineName ) )
                    {
                        return;
                    }

                    defines.Add( defineName );
                }
                else
                {
                    if( defines.Contains( defineName ) == false )
                    {
                        return;
                    }

                    while( defines.Contains( defineName ) )
                    {
                        defines.Remove( defineName );
                    }
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup( group, string.Join( ";", defines.ToArray() ) );
            }
        }

        // Get DefinesList
        private static List<string> GetDefinesList( BuildTargetGroup group )
        {
            return new List<string>( PlayerSettings.GetScriptingDefineSymbolsForGroup( group ).Split( ';' ) );
        }
    };
}
