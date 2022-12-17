using UnityEngine;

namespace MC.Project.GameState
{
    [CreateAssetMenu( menuName = MENU_FULL_PATH, fileName = FILE_NAME )]
    public class GameData : ScriptableObject
    {
        private const string MENU_NAME = "Game Data";
        private const string MENU_FULL_PATH = GameStateMenu.ROOT_MENU + "/" + MENU_NAME;
        private const string FILE_NAME = "Default GameState Data";
        
        public GameStateData[] stats;
        public GameStateData firstStat;
    }
}