using MC.Project.Core;
using UnityEngine;

namespace MC.Project.Character
{
    [CreateAssetMenu( menuName = MENU_FULL_PATH, fileName = FILE_NAME )]
    public class CharacterData : ScriptableObject
    {
        private const string MENU_NAME = "Character Data";
        private const string MENU_FULL_PATH = ProjectMenu.ROOT_PATH + "/" + CharacterMenu.MENU_PATH + "/" + MENU_NAME;
        private const string FILE_NAME = "Default Character Data";

        public string id;
    }
}