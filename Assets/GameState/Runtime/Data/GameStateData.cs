using MC.Project.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace MC.Project.GameState
{
    [CreateAssetMenu( menuName = MENU_FULL_PATH, fileName = FILE_NAME )]
    public class GameStateData : ScriptableObject
    {
        private const string MENU_NAME = "Main GameState Data";
        private const string MENU_FULL_PATH = ProjectMenu.ROOT_PATH + "/" + GameStateMenu.MENU_PATH + "/" + MENU_NAME;
        private const string FILE_NAME = "Default GameState Data";


        public string stateName;
        public SceneAsset sceneAsset;
        public GameObject prefab;
        public SubGameStateData[] subGameState;
    }
}   