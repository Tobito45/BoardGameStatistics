using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class CharacterChangeInputUIStateController : UIStateControllerBase
    {
        private Label _totalGames, _totalWins, _games, _wins;
        public CharacterChangeInputUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {

            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetCharactersState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveCharacter(visualElement);
            _totalGames = visualElement.Q<Label>("GamesTotal");
            _totalWins = visualElement.Q<Label>("WinsTotal");
            _games = visualElement.Q<Label>("Games");
            _wins = visualElement.Q<Label>("Wins");
            visualElement.Q<Button>("PlusGames").clicked += () =>
            {
                ChangeStats(1, _totalGames, _games, _uIController.ActualCharater.Games);
            };
            visualElement.Q<Button>("MinusGames").clicked += () =>
                ChangeStats(-1, _totalGames, _games, _uIController.ActualCharater.Games);
            visualElement.Q<Button>("PlusWins").clicked += () =>
                ChangeStats(1, _totalWins, _wins, _uIController.ActualCharater.Wins);
            visualElement.Q<Button>("MinusWins").clicked += () =>
                ChangeStats(-1, _totalWins, _wins, _uIController.ActualCharater.Wins);
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            visualElement.Q<Label>("Name").text = _uIController.ActualCharater.Name;
            _games.text = _uIController.ActualCharater.Games.ToString();
            _wins.text = _uIController.ActualCharater.Wins.ToString();
            _totalGames.text = _totalWins.text = string.Empty;
        }

        private void ChangeStats(int koef, Label total, Label orig, int baseCate)
        {
            int count = int.Parse(orig.text) + koef;
            if (count < 0) return;

            orig.text = count.ToString();
            if (count == baseCate)
                total.text = string.Empty;
            else
            {
                int differance = count - baseCate;
                total.text = "Total: ";

                if (differance > 0)
                {
                    total.style.color = new StyleColor(Color.green);
                    total.text += "+" + differance;
                }
                else
                {
                    total.style.color = new StyleColor(Color.red);
                    total.text += +differance;
                }
            }
        }

        private void SaveCharacter(VisualElement visualElement)
        {
            _uIController.ActualCharater.ChangeStats(int.Parse(_games.text), int.Parse(_wins.text));
            StateMachine.SetCharactersState();
        }
    }
}