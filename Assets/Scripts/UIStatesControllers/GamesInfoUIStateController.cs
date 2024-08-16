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
        VisualTreeAsset _prefabGamesElement, _prefabPlusElement;

        public GamesInfoUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetActionsState();
            _prefabGamesElement = Resources.Load<VisualTreeAsset>("Elements/GamesInfoElement");
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
                List<Game> games = ActualData.GetGames.ToList();
                for (int i = 0; i < games.Count(); i++)
                {
                    VisualElement itemUi = _prefabGamesElement.Instantiate();
                    itemUi.Q<Label>("Number").text = i.ToString();
                    itemUi.Q<Label>("Players").text = games[i].Players.ToString();
                    itemUi.Q<Label>("Time").text = games[i].Time.ToString("F1") + " min";
                    itemUi.Q<Label>("Text").text = games[i].Comment;
                    listView.Add(itemUi);
                }
                VisualElement plus = _prefabPlusElement.Instantiate();
                listView.Add(plus);
                plus.Q<Button>("Add").clicked += () => StateMachine.SetGamesInfoInputState();
            }
        }
    }
}