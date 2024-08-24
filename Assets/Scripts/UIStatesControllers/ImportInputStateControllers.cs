using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class ImportInputStateControllers : UIStateControllerBase
    {
        private readonly GameDataFactory _gameDataFactory;
        private VisualTreeAsset _prefabImportExport;

        public ImportInputStateControllers(UIController uIController, GameDataFactory gameDataFactory) : base(uIController)
        {
            _gameDataFactory = gameDataFactory;
        }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetMainState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewReview(visualElement.Q<ScrollView>("List"));
            _prefabImportExport = Resources.Load<VisualTreeAsset>("Elements/ImportElement");
        }

       

        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            ScrollView listView = visualElement.Q<ScrollView>("List");
            listView.Clear();
            foreach (GameData data in _uIController.ImportedData)
            {
                VisualElement itemUi = _prefabImportExport.Instantiate();
                itemUi.Q<Label>("Name").text = data.Name;
                _uIController.LoadImageAsync(itemUi.Q<VisualElement>("Image"), data.Url);
                listView.Add(itemUi);
            }
        }

        private void SaveNewReview(ScrollView list)
        {
            foreach (VisualElement item in list.Query<VisualElement>("ImportElement").ToList())
            {
                string name = item.Q<Label>("Name").text;
                bool onlyGame = item.Q<Toggle>("AddOnlyToggle").value;
                bool reviews = item.Q<Toggle>("ReviewsToggle").value;
                bool games = item.Q<Toggle>("GamesToggle").value;
                bool charaters = item.Q<Toggle>("CharacterToggle").value;
                bool urls = item.Q<Toggle>("UrlToggle").value;
               
                GameData importData = _uIController.ImportedData.First(x => x.Name.Equals(name));
                GameData actualData = _gameDataFactory.GetData().FirstOrDefault(x => x.Name.Equals(name));
                if (!onlyGame && !reviews && !games && !charaters && !urls) continue;

                if (actualData == null) {
                    actualData = new GameData(name, importData.Url, importData.Description);
                    _gameDataFactory.AddBoardGame(actualData);
                    actualData.AddUrl(GameDataFactory.URL_LOADING);
                }
                if (onlyGame) continue;

                if(reviews)
                    foreach (Review review in importData.GetReviews)
                        actualData.AddReview(review);

                if (games)
                    foreach (Game game in importData.GetGames)
                        actualData.AddGame(game);

                if (charaters)
                    foreach (Character character in importData.GetCharacters)
                        if(!actualData.GetCharacters.Any(n => n.Name.Equals(character.Name)))
                          actualData.AddCharacter(character);
               
                if (urls)
                    foreach (string url in importData.GetUrls)
                        if(url != GameDataFactory.URL_LOADING && _uIController.IsLink(url))
                            actualData.AddUrlNotLast(url);
            }
            StateMachine.SetMainState();
        }
    }
}
