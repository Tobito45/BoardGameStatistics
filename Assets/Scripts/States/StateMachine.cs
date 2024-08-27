using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace States
{
    public class StateMachine
    {
        public IState ActualState { get; private set; }
        
        private readonly MainState _mainState;
        private readonly GameState _gameState;
        private readonly ActionsState _actionsState;
        private readonly ReviewsState _reviewsState;
        private readonly ReviewInputState _reviewsInputState;
        private readonly GamesInfoState _gamesInfoState;
        private readonly GamesInfoInputState _gamesInfoInputState;
        private readonly CharactersState _charactersState;
        private readonly CharacterNewInputState _charactersNewInputState;
        private readonly CharacterChangeInputState _charactersChangeInputState;
        private readonly GameNewInputState _gameNewInputState;
        private readonly StartScreenState _startScreenState;
        private readonly UrlInputState _urlInput;
        private readonly GamesCharacterInputState _gamesCharacterInputState;
        private readonly ImportInputState _importInputState;
        private readonly ErrorScreenState _errorScreenState;
        private readonly CorrectScreenState _correctScreenState;
        public StateMachine(MainState mainState, GameState gameState, ActionsState actionsState, ReviewsState reviewsState, ReviewInputState reviewsInputState, GamesInfoState gamesInfoState, GamesInfoInputState gamesInfoInputState, CharactersState charactersState, CharacterNewInputState charactersNewInputState, CharacterChangeInputState charactersChangeInputState, GameNewInputState gameNewInputState, StartScreenState startScreenState, UrlInputState urlInput, GamesCharacterInputState gamesCharacterInputState, ImportInputState importInputState, ErrorScreenState errorScreenState, CorrectScreenState correctScreenState)
        {
            _mainState = mainState;
            _gameState = gameState;
            _actionsState = actionsState;
            _reviewsState = reviewsState;
            _reviewsInputState = reviewsInputState;
            _gamesInfoState = gamesInfoState;
            _gamesInfoInputState = gamesInfoInputState;
            _charactersState = charactersState;
            _charactersNewInputState = charactersNewInputState;
            _charactersChangeInputState = charactersChangeInputState;
            _gameNewInputState = gameNewInputState;
            _startScreenState = startScreenState;
            _urlInput = urlInput;
            _gamesCharacterInputState = gamesCharacterInputState;
            _importInputState = importInputState;
            _errorScreenState = errorScreenState;
            _correctScreenState = correctScreenState;

            Intialize(_mainState);
        }
        private void Intialize(IState startState)
        {
            ActualState = startState;
            ActualState.Entry();
        }

        public void TransitionTo(IState newState)
        {
            ActualState.Exit();
            ActualState = newState;
            ActualState.Entry();
        }

        public void SetMainState() => TransitionTo(_mainState);
        public void SetGameState() => TransitionTo(_gameState);
        public void SetActionsState() => TransitionTo(_actionsState);
        public void SetReviewsState() => TransitionTo(_reviewsState);
        public void SetReviewsInputState() => TransitionTo(_reviewsInputState);
        public void SetGamesInfoState() => TransitionTo(_gamesInfoState);
        public void SetGamesInfoInputState() => TransitionTo(_gamesInfoInputState);
        public void SetCharactersState() => TransitionTo(_charactersState);
        public void SetCharacterNewInputState() => TransitionTo(_charactersNewInputState);
        public void SetCharacterChangeInputState() => TransitionTo(_charactersChangeInputState);
        public void SetGameNewInputState() => TransitionTo(_gameNewInputState);
        public void SetStartScreenState() => TransitionTo(_startScreenState);
        public void SetUrlInputState() => TransitionTo(_urlInput);
        public void SetGamesCharacterInputState() => TransitionTo(_gamesCharacterInputState);
        public void SetImportInputState() => TransitionTo(_importInputState);
        public void SetErrorState() => TransitionTo(_errorScreenState);
        public void SetCorrectState() => TransitionTo(_correctScreenState);
    }
}
