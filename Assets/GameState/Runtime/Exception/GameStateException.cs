using System;

namespace MC.Project.GameState
{
    
    public class GameStateException : Exception
    {
        public enum Code
        {
            NULL_SCENE_ASSET_IN_DATA,
            NOT_FOUND_GAME_STATE_TYPE,
            EMPTY_STATE_NAME_IN_DATA,
        }

        public GameStateException( Code code ) : base( code.ToString() )
        {
            
        }
        
        public GameStateException( Code code, string subMessage ) : base( code.ToString() + ", " + subMessage )
        {
            
        }
    }
}