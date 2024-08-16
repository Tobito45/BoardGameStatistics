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
        public GamesInfoInputUIStateController(UIController uIController) : base(uIController) { }
        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetGamesInfoState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewGame(visualElement);
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            UnsignedIntegerField textFieldPlayers = visualElement.Q<UnsignedIntegerField>("PlayersInput");
            UnsignedIntegerField textFieldTime = visualElement.Q<UnsignedIntegerField>("TimeInput");
            textFieldPlayers.value = textFieldTime.value = 0;
            _uIController.SetInputFieldColor(textFieldPlayers, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldTime, Color.white, 0);
            visualElement.Q<TextField>("TextInput").value = string.Empty;
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
            string comment = visualElement.Q<TextField>("TextInput").value;

            if (!Validate(textFieldPlayers, textFieldTime))
                return;

            _uIController.GetActualData.AddGame(new Game((int)textFieldPlayers.value, (int)textFieldTime.value, comment));
            StateMachine.SetGamesInfoState();
        }
    }
}