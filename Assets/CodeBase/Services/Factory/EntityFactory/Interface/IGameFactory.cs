using CodeBase.Configs.Player;
using UnityEngine;

namespace CodeBase.Services.Factory.EntityFactory.Interface
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero();
        
       // GameObject CreateEnemy(EnemyId id, Vector3 position, Quaternion rotation);

        GameObject HeroObject { get; }
        /*VirtualJoystick VirtualJoystick { get; }
        FollowCamera FollowCamera { get; }
        HeroHealth HeroHeals { get;  }*/
    }
}

