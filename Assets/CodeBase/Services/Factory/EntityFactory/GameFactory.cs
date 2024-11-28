using CodeBase.Configs.Interface;
using CodeBase.GamePlay.Player;
using CodeBase.Services.AssetManager;
using CodeBase.Services.Factory.EntityFactory.Interface;
using Lean.Pool;
using UnityEngine;

namespace CodeBase.Services.Factory.EntityFactory
{
    public class GameFactory : IGameFactory
    {
        public GameObject HeroObject { get; private set; }
        private readonly IAssetProvider _assetProvider;
        private readonly IConfigsProvider _configProvider;

        public GameFactory(IAssetProvider assetProvider, IConfigsProvider configProvider)
        {
            _assetProvider = assetProvider;
            _configProvider = configProvider;
        }
        
        public GameObject CreateHero()
        {
            var playerCharacterSettingConfig = _configProvider.GetPlayerConfig();
            
            HeroObject = LeanPool.Spawn(_assetProvider.GetPrefab<GameObject>(playerCharacterSettingConfig.PathPlayerPrefabe),playerCharacterSettingConfig.defaultSpawnPosition,Quaternion.identity);
            GameObject cameraObject = LeanPool.Spawn(_assetProvider.GetPrefab<GameObject>(playerCharacterSettingConfig.PathCameraPrefab));
            
            PlayerLogic playerLogic = HeroObject.GetComponent<PlayerLogic>();
            ThirdPersonCamera playerCamera = cameraObject.GetComponent<ThirdPersonCamera>();

            playerCamera.InitializeCamera(playerLogic.CameraFollowPaint,playerLogic.transform);
            playerLogic.InitializeHeroPlayer(playerCharacterSettingConfig,playerCamera);
            
            return HeroObject;
        }

        /*public VirtualJoystick CreateVirtualJoystick()
        {
            GameObject virtualJoystickPrefab = _assetProvider.GetPrefab<GameObject>(AsseetPath.VirtualJoystickPath);

            VirtualJoystick = _diContainer.InstantiateGameObject(virtualJoystickPrefab).GetComponent<VirtualJoystick>();

            return VirtualJoystick;
        }*/

        /*public FollowCamera CreateFollowCamera()
        {
            GameObject followCameraPrefab = _assetProvider.GetPrefab<GameObject>(AsseetPath.FollowCameraPath);

            FollowCamera = _diContainer.InstantiateGameObject(followCameraPrefab).GetComponent<FollowCamera>();

            return FollowCamera;
        }*/

        public GameObject CreateEnemy(Vector3 position, Quaternion rotation)
        {
            /*EnemyConfig config = _configProvider.GetEnemyConfig(id);
            GameObject enemyPrefab = config.Prefab;
            GameObject enemy = _diContainer.InstantiateGameObject(enemyPrefab);

            enemy.transform.position = position;
            enemy.transform.rotation = rotation;
            
            IEnemyConfigInstaller[] enemyConfigInstallers = enemy.GetComponentsInChildren<IEnemyConfigInstaller>();

            for (int i = 0; i < enemyConfigInstallers.Length; i++)
            {
                enemyConfigInstallers[i].InstallConfig(config);
            }

            return enemy;*/
            return null;
        }
    }
}