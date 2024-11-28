using CodeBase.Configs.Interface;
using CodeBase.Services.Factory.EntityFactory.Interface;
using CodeBase.Services.StateMachine.Common.Interface;
using CodeBase.Services.StateMachine.LevelStateMachine.Interface;
using CodeBase.Services.WindowsProvider;
using CodeBase.UI.MainUI;
using UnityEngine;

namespace CodeBase.Services.StateMachine.LevelStateMachine.LevelState
{
    public class LevelBootstrapMainSceneState : IEnterableState, IService
    {
        private readonly IGameFactory _gameFactory;
        private readonly ILevelStateSwitcher _levelStateSwitcher;
        private readonly IConfigsProvider _configsProvider;
        private readonly IWindowsProvider _windowsProvider;

        public LevelBootstrapMainSceneState(ILevelStateSwitcher levelStateSwitcher, IConfigsProvider configsProvider, IWindowsProvider windowsProvider,IGameFactory gameFactory)
        {
            Debug.Log("LEVEL: InitMainScene");
            _gameFactory = gameFactory;
            _levelStateSwitcher = levelStateSwitcher;
            _configsProvider = configsProvider;
            _windowsProvider = windowsProvider;
        }

        public void Enter()
        {
            _gameFactory.CreateHero();
            _windowsProvider.OpenWindow(WindowMainUIId.InventoryWindow);
            
            /*string sceneName = SceneManager.GetActiveScene().name;
            LevelConfig levelConfig = _configsProvider.GetLevelConfig(sceneName);*/

            //EnemySpawner[] enemiesSpawners =GameObject.FindObjectsOfType<EnemySpawner>();

            /*for (int i = 0; i < enemiesSpawners.Length; i++)
            {
                enemiesSpawners[i].SpawnEnemy();
            }*/

            //_gameFactory.CreateHero(levelConfig.HeroSpawnPoint, Quaternion.identity);
            /*_gameFactory.CreateFollowCamera().SetTarget(_gameFactory.HeroObject.transform);
            _gameFactory.CreateVirtualJoystick();*/
            
            
            //_levelStateSwitcher.EnterState<LevelResearcherState>();
        }
    }
}