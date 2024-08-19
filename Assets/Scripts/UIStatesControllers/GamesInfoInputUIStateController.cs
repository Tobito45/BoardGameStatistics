using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class GamesInfoInputUIStateController : UIStateControllerBase
    {
        VisualTreeAsset _prefabCharacter;
        public GamesInfoInputUIStateController(UIController uIController) : base(uIController) { }
        public override void Installization(VisualElement visualElement)
        {
            _prefabCharacter = Resources.Load<VisualTreeAsset>("Elements/GameInfoInputElement");
            visualElement.Q<Button>("BackButton").clicked += () =>
            {
                StateMachine.SetGamesInfoState();
                _uIController.ActualGame = null;
            };
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewGame(visualElement);
            visualElement.Q<Button>("AddButtonWinners").clicked += () =>
            {
                StateMachine.SetGamesCharacterInputState();
                _uIController.ActualGame.isLoser = false;
            };
            visualElement.Q<Button>("AddButtonLossers").clicked += () =>
            {
                StateMachine.SetGamesCharacterInputState();
                _uIController.ActualGame.isLoser = true;
            };
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            if (_uIController.ActualGame == null)
            {
                _uIController.ActualGame = new Game();
                textFieldPlayers.value = textFieldTime.value = 0;
                visualElement.Q<TextField>("TextInput").value = string.Empty;
            } 

            _uIController.SetInputFieldColor(textFieldPlayers, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldTime, Color.white, 0);
            ScrollView winners = visualElement.Q<ScrollView>("WinnersList");
            ScrollView lossers = visualElement.Q<ScrollView>("LosersList");
            winners.Clear();
            lossers.Clear();
            CreateList(winners, _uIController.ActualGame.Winners);
            CreateList(lossers, _uIController.ActualGame.Losers);
        }

        private void CreateList(ScrollView scrollView, List<(Character, int)> list)
        {
            foreach ((Character character, int points) in list)
            {
                VisualElement item = _prefabCharacter.Instantiate();
                scrollView.Add(item);
                item.Q<Label>("Name").text = character.Name;
                item.Q<Label>("Points").text = points.ToString();
                item.Q<Button>("DeleteButton").clicked += () =>
                {
                    list.Remove((character, points));
                    scrollView.Remove(item);
                };
            }
        }

        private bool Validate(UnsignedIntegerField textFieldPlayers, UnsignedIntegerField textFieldTime)
        {
            bool result = true;
            if (textFieldPlayers.value == 0)
            {
                _uIController.SetInputFieldColor(textFieldPlayers, Color.red, 2);
                result = false;
            }
            if (textFieldTime.value == 0)
            {
                _uIController.SetInputFieldColor(textFieldTime, Color.red, 2);
                result = false;
            }
            return result;
        }

        private void SaveNewGame(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            
            if (!Validate(textFieldPlayers, textFieldTime))
                return;

            _uIController.ActualGame.Time = (int)textFieldTime.value;
            _uIController.ActualGame.Players = (int)textFieldPlayers.value;
            _uIController.ActualGame.Comment = visualElement.Q<TextField>("TextInput").value;
            _uIController.GetActualData.AddGame(_uIController.ActualGame);
            _uIController.ActualGame = null;
            StateMachine.SetGamesInfoState();
        }
    }
}