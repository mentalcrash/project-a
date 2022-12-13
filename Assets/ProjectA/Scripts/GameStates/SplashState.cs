using MC.Project.GameState;
using UnityEngine;

namespace MC.Project.A.GameStates
{
    [GameState("Splash")]
    public class SplashState : IGameState
    {
        public void OnEnter()
        {
            Debug.Log( "Entered Splash State" );
        }

        public void OnLeave()
        {
            
        }
    }
}