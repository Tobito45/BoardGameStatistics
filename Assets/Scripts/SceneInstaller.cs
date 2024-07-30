using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameDataFactory>().AsSingle();
    }
}
