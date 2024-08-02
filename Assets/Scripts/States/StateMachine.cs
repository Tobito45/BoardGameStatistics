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
        
        private MainState _mainState;
        private GameState _gameState;
        private ActionsState _actionsState;
        private ReviewsState _reviewsState;
        private ReviewInputState _reviewsInputState;
        public StateMachine(MainState mainState, GameState gameState, ActionsState actionsState, ReviewsState reviewsState, ReviewInputState reviewsInputState)
        {
            _mainState = mainState;
            _gameState = gameState;
            _actionsState = actionsState;
            _reviewsState = reviewsState;
            _reviewsInputState = reviewsInputState;

            Intialize(_mainState);
        }

        private void Intialize(IState startState)
        {
            ActualState = startState;
            ActualState.Entry();
        }

        private void TransitionTo(IState newState)
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
    }
}
