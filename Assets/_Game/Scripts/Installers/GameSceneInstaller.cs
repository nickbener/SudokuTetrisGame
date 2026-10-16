using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GridModel>().AsSingle();

        Container.Bind<GridView>()
                .FromComponentInHierarchy()
                .AsSingle();

        Container.BindInterfacesAndSelfTo<GameLoopController>()
                .AsSingle();

        Container.Bind<ShapesSpawner>().AsSingle();

        Container.Bind<ScoreView>().FromComponentInHierarchy().AsSingle();

        Container.Bind<PauseView>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ISaveService>().To<FileSaveService>().AsSingle();
    }
}
