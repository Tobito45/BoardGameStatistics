using Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class CharactersUIStateController : UIStateControllerBase
    {
        VisualTreeAsset _prefabCharactersElement, _prefabPlusElement;
        public CharactersUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetActionsState();
            _prefabCharactersElement = Resources.Load<VisualTreeAsset>("Elements/CharacterElement");
            _prefabPlusElement = Resources.Load<VisualTreeAsset>("Elements/PlusElement");
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            //   if (int.Parse(visualElement.Q<Label>("Players").text) != ActualData.Players
            //       || ActualData.GetGames.Count() != visualElement.Q<ScrollView>("List").childCount + 1)
            {
                visualElement.Q<ScrollView>("List").Clear();

                visualElement.Q<Label>("Name").text = ActualData.Name;
                ScrollView listView = visualElement.Q<ScrollView>("List");
                foreach (Character character in ActualData.GetCharacters)
                {
                    VisualElement itemUi = _prefabCharactersElement.Instantiate();
                    character.CalculateData(ActualData.GetGames.Select(n => n.Winners),
                               ActualData.GetGames.Select(n => n.Losers).ToList());

                    itemUi.Q<Label>("Name").text = character.Name;
                    itemUi.Q<Label>("Games").text = character.Games.ToString();
                    itemUi.Q<Label>("Wins").text = character.Wins.ToString();
                    itemUi.Q<Label>("Percents").text = character.Percent.ToString("F1");
                    itemUi.Q<Label>("Points").text = character.AveragePoints.ToString("F1");

                    itemUi.Q<Button>("AddButton").clicked += () =>
                    {
                        _uIController.ActualCharater = character;
                        StateMachine.SetCharacterChangeInputState();
                    };
                    listView.Add(itemUi);
                }
                VisualElement plus = _prefabPlusElement.Instantiate();
                listView.Add(plus);
                plus.Q<Button>("Add").clicked += () => StateMachine.SetCharacterNewInputState();
            }
        }
    }
}