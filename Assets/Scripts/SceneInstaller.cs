using States;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private UIDocument _main, _game, _actions, _reviews, _reviewsInput, _gamesInput, _characterNewInput, _characterChangeInput,
        _gameNewInput, _startState, _urlInput, _gamesCharacterInput, _importInput;

    [SerializeField]
    private SceneContext _sceneContext;
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
        Container.Bind<GamesInfoState>().AsSingle().WithArguments(_reviews.rootVisualElement);
        Container.Bind<GamesInfoInputState>().AsSingle().WithArguments(_gamesInput.rootVisualElement);
        Container.Bind<CharactersState>().AsSingle().WithArguments(_reviews.rootVisualElement);
        Container.Bind<CharacterNewInputState>().AsSingle().WithArguments(_characterNewInput.rootVisualElement);
        Container.Bind<CharacterChangeInputState>().AsSingle().WithArguments(_characterChangeInput.rootVisualElement);
        Container.Bind<GameNewInputState>().AsSingle().WithArguments(_gameNewInput.rootVisualElement);
        Container.Bind<StartScreenState>().AsSingle().WithArguments(_startState.rootVisualElement);
        Container.Bind<UrlInputState>().AsSingle().WithArguments(_urlInput.rootVisualElement);
        Container.Bind<GamesCharacterInputState>().AsSingle().WithArguments(_gamesCharacterInput.rootVisualElement);
        Container.Bind<ImportInputState>().AsSingle().WithArguments(_importInput.rootVisualElement);
        Container.Bind<StateMachine>().AsSingle().NonLazy();
    }

    private void Start()
    {
        StartCoroutine(RunInstaller());
    }

    private IEnumerator RunInstaller()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
        yield return new WaitForSeconds(0.2f);
        _sceneContext.Run();
    }

    private void OnDestroy()
    {
       Container.Resolve<GameDataFactory>().SaveData();
    }

    private void OnApplicationQuit()
    {
        Container.Resolve<GameDataFactory>().SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        Container?.Resolve<GameDataFactory>()?.SaveData();
    }
}
