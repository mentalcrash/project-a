using UnityEditor;
using UnityEngine;

namespace MC.Project.GameState.Editor
{
    public class GameStateEditorWindow : EditorWindow
    {
        private const string MENU_NAME = "Editor Window";

        [MenuItem( GameStateMenu.EDITOR_WINDOW_ROOT_PATH + "/" + MENU_NAME )]
        private static void Init()
        {
            var window = GetWindow<GameStateEditorWindow>( false, "Game State Window" );
            window.Show();
        }

        private void OnGUI()
        {
            if( Application.isPlaying == false )
            {
                EditorGUILayout.LabelField( "Use In Playing Mode" );
                return;
            }

            if( GameStateManager.Instance.Initialized == false )
            {
                EditorGUILayout.LabelField( "Initialized First" );
                return;
            }

            EditorGUILayout.LabelField( "State List", EditorStyles.boldLabel );
            
            var statNames = GameStateManager.Instance.GameStateNames;
            var currentStateName = GameStateManager.Instance.CurrentGameStateName;
            
            foreach( var statName in statNames )
            {
                if( statName == currentStateName )
                {
                    EditorGUILayout.LabelField( $">> {statName}" );
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField( $"{statName}" );
                    if( GUILayout.Button( "Enter", GUILayout.Width( 100 ) ) )
                    {
                        GameStateManager.Instance.SetState( statName );
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }    
}

