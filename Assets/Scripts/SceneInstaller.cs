using States;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private UIDocument _main, _game, _actions, _reviews;

    [SerializeField]
    private UIController _controller;
    public override void InstallBindings()
    {
        InstallStateMachine();

        Container.Bind<GameDataFactory>().AsSingle();
    }

    private void InstallStateMachine()
    {
        Container.Bind<MainState>().AsSingle().WithArguments(_main.rootVisualElement, _controller);
        Container.Bind<GameState>().AsSingle().WithArguments(_game.rootVisualElement, _controller);
        Container.Bind<ActionsState>().AsSingle().WithArguments(_actions.rootVisualElement, _controller);
        Container.Bind<ReviewsState>().AsSingle().WithArguments(_reviews.rootVisualElement, _controller);
        Container.Bind<StateMachine>().AsSingle().NonLazy();
    }
}
