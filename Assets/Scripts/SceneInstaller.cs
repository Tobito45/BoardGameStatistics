using States;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private UIDocument _main, _game, _actions, _reviews, _reviewsInput, _gamesInput, _characterNewInput, _characterChangeInput;

    public override void InstallBindings()
    {
        Container.Bind<GameDataFactory>().AsSingle();
        Container.Bind<UIController>().AsSingle().NonLazy();
        InstallStateMachine();
    }

    private void InstallStateMachine()
    {
       // VisualTreeAsset visualElement = Resources.Load<VisualTreeAsset>("Main");
        Container.Bind<MainState>().AsSingle().WithArguments(_main.rootVisualElement); //visualElement.Instantiate()
        Container.Bind<GameState>().AsSingle().WithArguments(_game.rootVisualElement);
        Container.Bind<ActionsState>().AsSingle().WithArguments(_actions.rootVisualElement);
        Container.Bind<ReviewsState>().AsSingle().WithArguments(_reviews.rootVisualElement);
        Container.Bind<ReviewInputState>().AsSingle().WithArguments(_reviewsInput.rootVisualElement);
        Container.Bind<GamesInfoState>().AsSingle().WithArguments(_reviews.rootVisualElement);
        Container.Bind<GamesInfoInputState>().AsSingle().WithArguments(_gamesInput.rootVisualElement);
        Container.Bind<CharactersState>().AsSingle().WithArguments(_reviews.rootVisualElement);
        Container.Bind<CharacterNewInputState>().AsSingle().WithArguments(_characterNewInput.rootVisualElement);
        Container.Bind<CharacterChangeInputState>().AsSingle().WithArguments(_characterChangeInput.rootVisualElement);
        Container.Bind<StateMachine>().AsSingle().NonLazy();
    }
}
