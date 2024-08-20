using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class GamesInfoUIStateController : UIStateControllerBase
    {
        private VisualTreeAsset _prefabGamesElement, _prefabPlusElement;
        private VisualTreeAsset _prefabCharacter;

        public GamesInfoUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetActionsState();
            _prefabGamesElement = Resources.Load<VisualTreeAsset>("Elements/GamesInfoElement");
            _prefabPlusElement = Resources.Load<VisualTreeAsset>("Elements/PlusElement");
            _prefabCharacter = Resources.Load<VisualTreeAsset>("Elements/GameInfoInputElement");

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
                List<Game> games = ActualData.GetGames.ToList();
                for (int i = 0; i < games.Count(); i++)
                {
                    VisualElement itemUi = _prefabGamesElement.Instantiate();
                    itemUi.Q<Label>("Number").text = i.ToString();
                    itemUi.Q<Label>("Players").text = games[i].Players.ToString();
                    itemUi.Q<Label>("Time").text = games[i].Time.ToString("F1") + " min";
                    itemUi.Q<Label>("Text").text = games[i].Comment;
                    int save = i;
                    itemUi.Q<Button>("DeleteButton").clicked += () =>
                    {
                        ActualData.RemoveGame(games[save]);
                        listView.Remove(itemUi);
                    };
                    if (games[i].Winners.Count == 0 && games[i].Losers.Count == 0) 
                        itemUi.Q<VisualElement>("Characters").style.display = DisplayStyle.None;
                    else
                    {
                        CreateList(games[i].Winners, itemUi.Q<ScrollView>("Winners"));
                        CreateList(games[i].Losers, itemUi.Q<ScrollView>("Losers"));
                    }
                    listView.Add(itemUi);
                }
                VisualElement plus = _prefabPlusElement.Instantiate();
                listView.Add(plus);
                plus.Q<Button>("Add").clicked += () => StateMachine.SetGamesInfoInputState();
            }
        }

        private void CreateList(List<(Character character, int point)> list, ScrollView scrollView)
        {
            foreach ((Character character, int point) in list)
            {
                VisualElement element = _prefabCharacter.Instantiate();
                element.Q<Button>("DeleteButton").text = string.Empty;
                element.Q<Label>("Name").text = character.Name;
                element.Q<Label>("Points").text = point.ToString();
                scrollView.Add(element);
            }
        }
    }
}