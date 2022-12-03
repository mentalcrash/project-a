using MC.Project.Core.Editor;
using UnityEditor;

namespace MC.Project.Character.Editor
{
    public class CreateCharacterDataMenu
    {
        private const string SUB_MENU_NAME = "Character";
        private const string MENU_NAME = "Create Data";
        
        private const string FULL_MENU_NAME = MCProjectMenuPath.GAME_OBJECT_MENU + "/" + SUB_MENU_NAME + "/" + MENU_NAME;

        [MenuItem(FULL_MENU_NAME, false, 10)]
        private static void Active(MenuCommand menuCommand)
        {
            
        }
    }
}