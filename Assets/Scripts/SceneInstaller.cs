using States;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private UIDocument _main, _game, _actions;

    [SerializeField]
    private UIController _controller;
    public override void InstallBindings()
    {
        InstallStateMachine();

        Container.Bind<GameDataFactory>().AsSingle();
        //Container.Bind<GameDataController>().AsSingle();
        //Container.Bind<UIController>().AsSingle()
       //     .WithArguments(_main, _game, _actions).NonLazy();
    }

    private void InstallStateMachine()
    {
        Container.Bind<MainState>().AsSingle().WithArguments(_main.rootVisualElement, _controller);
        Container.Bind<GameState>().AsSingle().WithArguments(_game.rootVisualElement, _controller);
        Container.Bind<ActionsState>().AsSingle().WithArguments(_actions.rootVisualElement, _controller);
        Container.Bind<StateMachine>().AsSingle().NonLazy();
    }
}
