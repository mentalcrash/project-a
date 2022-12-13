using MC.Project.Core;
using UnityEditor;
using UnityEngine;

namespace MC.Project.GameState
{
    [CreateAssetMenu( menuName = MENU_FULL_PATH, fileName = FILE_NAME )]
    public class SubGameStateData : ScriptableObject
    {
        private const string MENU_NAME = "Sub GameState Data";
        private const string MENU_FULL_PATH = ProjectMenu.ROOT_PATH + "/" + GameStateMenu.MENU_PATH + "/" + MENU_NAME;
        private const string FILE_NAME = "Default Sub GameState Data";
        
        public SceneAsset scene;
    }
}