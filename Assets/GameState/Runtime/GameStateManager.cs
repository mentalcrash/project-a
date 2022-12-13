using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MC.Project.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MC.Project.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance => _instance ??= CreateInstance();

        private static GameStateManager _instance;

        private static GameStateManager CreateInstance()
        {
            var go = new GameObject( nameof( GameStateManager ) );
            return go.AddComponent<GameStateManager>();
        }

        private bool _initialized;
        private GameData _gameData;
        
        private readonly Dictionary<string/*State Name*/, GameStateData> _gameStateData = new();
        private readonly Dictionary<string/*State Name*/, IGameState> _gameStates = new();
        private readonly Dictionary<string/*State Name*/, Type> _cachedStateTypes = new();

        private void Awake()
        {
            Debug.Log( $"[MC](GameState) Awake {nameof( GameStateManager )}" );

            DontDestroyOnLoad( this.gameObject );
            _instance = this;
            CacheGameStateTypes();
        }

        private void CacheGameStateTypes()
        {
            var gameStateTypes = Utils.FindTypes( typeof(GameStateAttribute), typeof(IGameState) ).ToArray();
            foreach( var gameStateType in gameStateTypes )
            {
                var customAttributes = gameStateType.GetCustomAttributes( false );
                foreach( var customAttribute in customAttributes )
                {
                    if( customAttribute is GameStateAttribute gameStateAttribute )
                    {
                        _cachedStateTypes.Add( gameStateAttribute.name, gameStateType );
                    }
                }
            }
        }

        public void Initialize( GameData data )
        {
            Debug.Log( $"[MC](GameState) Initialized {nameof( GameStateManager )}" );
            _gameData = data;

            foreach( var gameStateData in _gameData.stats )
            {
                var stateName = gameStateData.stateName;
                if( string.IsNullOrEmpty( stateName ) )
                    throw new GameStateException( GameStateException.Code.EMPTY_STATE_NAME_IN_DATA, gameStateData.name );
                _gameStateData.Add( stateName, gameStateData );
            }
            
            if( _gameData.firstStat )
            {
                StartCoroutine( LoadFirstScene( _gameData.firstStat.stateName ) );
            }
        }

        private void EnsureGameState( string stateName )
        {
            if( _gameStates.ContainsKey( stateName ) )
                return;

            if( _cachedStateTypes.ContainsKey( stateName ) == false )
                throw new GameStateException( GameStateException.Code.NOT_FOUND_GAME_STATE_TYPE, stateName );

            var type = _cachedStateTypes[stateName];
            var gameState = Activator.CreateInstance( type ) as IGameState;

            _gameStates.Add( stateName, gameState );
        }

        private IEnumerator LoadFirstScene( string stateName )
        {
            EnsureGameState( stateName );

            var gameStateData = _gameStateData[stateName];
            if( gameStateData.sceneAsset == null )
                throw new GameStateException( GameStateException.Code.NULL_SCENE_ASSET_IN_DATA, stateName );

            var sceneName = gameStateData.sceneAsset.name;

            yield return SceneManager.LoadSceneAsync( sceneName, new LoadSceneParameters( LoadSceneMode.Single ) );

            var gameState = _gameStates[stateName];
            gameState.OnEnter();
        }

        private void LoadScene( string stateName )
        {
        }

        public void Release()
        {
            Debug.Log( "Release" );
        }
    }
}