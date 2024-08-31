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
                _uIController.ActualGame = (null, false);
            };
            visualElement.Q<Button>("AddButton").clicked += () =>
            {
                SaveNewGame(visualElement);
                _uIController.ActualGame = (null, false);
            };
            visualElement.Q<Button>("AddButtonWinners").clicked += () =>
            {
                SaveData(visualElement);
                StateMachine.SetGamesCharacterInputState();
                _uIController.ActualGame.game.isLoser = false;
            };
            visualElement.Q<Button>("AddButtonLossers").clicked += () =>
            {
                SaveData(visualElement);
                StateMachine.SetGamesCharacterInputState();
                _uIController.ActualGame.game.isLoser = true;
            };
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            if (_uIController.ActualGame.game == null)
            {
                _uIController.ActualGame = (new Game(), false);
                textFieldPlayers.value = textFieldTime.value = 0;
                visualElement.Q<TextField>("TextInput").value = string.Empty;
                visualElement.Q<Label>("HeadText").text = "Add game";
            }
            else
            {
                textFieldPlayers.value = (uint)_uIController.ActualGame.game.Players;
                textFieldTime.value = (uint)_uIController.ActualGame.game.Time;
                visualElement.Q<TextField>("TextInput").value = _uIController.ActualGame.game.Comment;
                if(_uIController.ActualGame.isEdit)
                    visualElement.Q<Label>("HeadText").text = "Edit game";
            }

            _uIController.SetInputFieldColor(textFieldPlayers, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldTime, Color.white, 0);
            ScrollView winners = visualElement.Q<ScrollView>("WinnersList");
            ScrollView lossers = visualElement.Q<ScrollView>("LosersList");
            winners.Clear();
            lossers.Clear();
            CreateList(winners, _uIController.ActualGame.game.Winners);
            CreateList(lossers, _uIController.ActualGame.game.Losers);
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

        private void SaveData(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            _uIController.ActualGame.game.Time = (int)textFieldTime.value;
            _uIController.ActualGame.game.Players = (int)textFieldPlayers.value;
            _uIController.ActualGame.game.Comment = visualElement.Q<TextField>("TextInput").value;
        }

        private void SaveNewGame(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            
            if (!Validate(textFieldPlayers, textFieldTime))
                return;

            SaveData(visualElement);
            if(!_uIController.ActualGame.isEdit)
                _uIController.GetActualData.AddGame(_uIController.ActualGame.game);
            StateMachine.SetGamesInfoState();
        }
    }
}