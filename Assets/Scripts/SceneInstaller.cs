using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private UIDocument document;
    public override void InstallBindings()
    {
        Container.Bind<VisualElement>().FromInstance(document.rootVisualElement).AsSingle();
        Container.Bind<GameDataFactory>().AsSingle();
        Container.Bind<MainController>().AsSingle().NonLazy();
    }
}
