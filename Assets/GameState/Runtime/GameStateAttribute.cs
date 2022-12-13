using System;

namespace MC.Project.GameState
{
    public class GameStateAttribute : Attribute
    {
        public string name;

        public GameStateAttribute( string name )
        {
            this.name = name;
        }
    }
}