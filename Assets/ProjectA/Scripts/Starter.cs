using MC.Project.GameState;
using UnityEngine;

namespace MC.Project.A
{
    public class Starter : MonoBehaviour
    {
        [SerializeField]
        private GameData gameData;

        private void Start()
        {
            GameStateManager.Instance.Initialize( gameData );
        }
    }    
}

