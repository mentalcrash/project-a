using MC.Project.GameState;
using UnityEngine;

namespace MC.Project.A
{
    [GameState("InGame")]
    public class InGameState : IGameState
    {
        public void OnEnter()
        {
            Debug.Log( "Entered InGame State" );
        }

        public void OnLeave()
        {
            Debug.Log( "Leave InGame State" );
        }
    }   
}
