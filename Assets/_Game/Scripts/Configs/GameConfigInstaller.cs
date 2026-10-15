using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
{
    [SerializeField] private GameConfigSO _gameConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(_gameConfig).AsSingle();
    }
}