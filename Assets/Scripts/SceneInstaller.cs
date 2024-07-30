using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private UIDocument _main, _game;
    public override void InstallBindings()
    {
        Container.Bind<GameDataFactory>().AsSingle();
        Container.Bind<GameDataController>().AsSingle();
        Container.Bind<UIController>().AsSingle()
            .WithArguments(_main, _game).NonLazy();
    }
}
