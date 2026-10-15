using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
{
    [SerializeField] private GameConfigSO _gameConfig;
    [SerializeField] private ShapesDatabaseSO _shapesDatabase;

    public override void InstallBindings()
    {
        Container.BindInstance(_gameConfig).AsSingle();
        Container.BindInstance(_shapesDatabase).AsSingle();
    }
}