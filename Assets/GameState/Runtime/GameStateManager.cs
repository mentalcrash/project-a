using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
        
        private GameData _gameData;
        
        private readonly Dictionary<string/*State Name*/, GameStateData> _gameStateData = new();
        private readonly Dictionary<string/*State Name*/, IGameState> _gameStates = new();
        private readonly Dictionary<string/*State Name*/, Type> _cachedStateTypes = new();
        
        private readonly List<GameObject> _instantiateGameObjects = new();
        private readonly Stack<string> _history = new();
        
        
        private string _firstGameStatName;
        private bool _loadedFirstGameState;
        
        public bool Initialized { private set; get; }
        public string[] GameStateNames { private set; get; }

        public string CurrentGameStateName { private set; get; }    

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
            
            GameStateNames = _gameStateData.Keys.ToArray();
            
            if( _gameData.firstStat )
            {
                StartCoroutine( InitFirstState( _gameData.firstStat.stateName ) );
            }
        }

        public void SetState( string stateName )
        {
            StartCoroutine( LoadState( stateName ) );
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

        private IEnumerator InitFirstState( string stateName )
        {
            EnsureGameState( stateName );

            var gameStateData = _gameStateData[stateName];
            if( gameStateData.sceneAsset == null )
                throw new GameStateException( GameStateException.Code.NULL_SCENE_ASSET_IN_DATA, stateName );

            var sceneName = gameStateData.sceneAsset.name;

            yield return SceneManager.LoadSceneAsync( sceneName, new LoadSceneParameters( LoadSceneMode.Single ) );

            Enter( stateName );

            _firstGameStatName = stateName;
            CurrentGameStateName = stateName;
            _loadedFirstGameState = true;
            _history.Push( stateName );
            
            Initialized = true;
        }
        
        private IEnumerator LoadState( string stateName )
        {
            if( _loadedFirstGameState == false )
                throw new GameStateException( GameStateException.Code.NOT_LOADED_FIRST_GAME_STATE );
            
            EnsureGameState( stateName );
            var gameStateData = _gameStateData[stateName];
            if( gameStateData.sceneAsset == null )
                throw new GameStateException( GameStateException.Code.NULL_SCENE_ASSET_IN_DATA, stateName );

            var sceneName = gameStateData.sceneAsset.name;

            Leave( CurrentGameStateName );

            var currentGameStateData = _gameStateData[CurrentGameStateName];
            if( gameStateData.forceReload || currentGameStateData.sceneAsset != gameStateData.sceneAsset)
            {
                yield return SceneManager.LoadSceneAsync( sceneName, new LoadSceneParameters( LoadSceneMode.Single ) );
            }

            Enter( stateName );

            CurrentGameStateName = stateName;
            _history.Push( stateName );
        }

        private void Enter( string stateName )
        {
            var gameState = _gameStates[stateName];
            gameState.OnEnter();
            InstantiatePrefabs( stateName );
        }

        private void Leave( string stateName )
        {
            var gameState = _gameStates[stateName];
            gameState.OnLeave();
            DestroyPrefabs();
        }
        
        private void InstantiatePrefabs(string stateName)
        {
            var gameStateData = _gameStateData[stateName];
            var prefabs = gameStateData.prefabs;
            foreach( var prefab in prefabs )
            {
                if( prefab != null )
                    _instantiateGameObjects.Add( Instantiate( prefab ) );
            }
        }

        private void DestroyPrefabs()
        {
            _instantiateGameObjects.ForEach( Destroy);
            _instantiateGameObjects.Clear();
        }
        
        public void Release()
        {
            Debug.Log( "Release" );
        }
    }
}