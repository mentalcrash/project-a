using UnityEditor;
using UnityEngine;

namespace MC.Project.GameState
{
    [CreateAssetMenu( menuName = MENU_FULL_PATH, fileName = FILE_NAME )]
    public class GameStateData : ScriptableObject
    {
        private const string MENU_NAME = "GameState Data";
        private const string MENU_FULL_PATH = GameStateMenu.ROOT_MENU + "/" + MENU_NAME;
        private const string FILE_NAME = "Default GameState Data";

        public string stateName;
        public SceneAsset sceneAsset;
        public bool forceReload;
        public GameObject[] prefabs;
    }
}   