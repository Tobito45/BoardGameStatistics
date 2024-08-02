using States;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private UIDocument _main, _game, _actions, _reviews, _reviewsInput;

    public override void InstallBindings()
    {
        Container.Bind<GameDataFactory>().AsSingle();
        Container.Bind<UIController>().AsSingle().NonLazy();
        InstallStateMachine();

    }

    private void InstallStateMachine()
    {
        Container.Bind<MainState>().AsSingle().WithArguments(_main.rootVisualElement);
        Container.Bind<GameState>().AsSingle().WithArguments(_game.rootVisualElement);
        Container.Bind<ActionsState>().AsSingle().WithArguments(_actions.rootVisualElement);
        Container.Bind<ReviewsState>().AsSingle().WithArguments(_reviews.rootVisualElement);
        Container.Bind<ReviewInputState>().AsSingle().WithArguments(_reviewsInput.rootVisualElement);
        Container.Bind<StateMachine>().AsSingle().NonLazy();
    }
}
