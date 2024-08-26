    using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class ReviewsUIStateController : UIStateControllerBase
    {
        VisualTreeAsset _prefabReviewElement, _prefabPlusElement;
        public ReviewsUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetActionsState();
            _prefabReviewElement = Resources.Load<VisualTreeAsset>("Elements/ReviewElement");
            _prefabPlusElement = Resources.Load<VisualTreeAsset>("Elements/PlusElement");
        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            //   if (visualElement.Q<Label>("Name").text != ActualData.Name 
            //      || ActualData.GetReviews.Count() != visualElement.Q<ScrollView>("List").childCount + 1)
            {
                visualElement.Q<ScrollView>("List").Clear();

                visualElement.Q<Label>("Name").text = ActualData.Name;
                ScrollView listView = visualElement.Q<ScrollView>("List");
                foreach (Review review in ActualData.GetReviews)
                {
                    VisualElement itemUi = _prefabReviewElement.Instantiate();
                    itemUi.Q<Label>("Name").text = review.Name;
                    itemUi.Q<Label>("Text").text = review.Text;
                    itemUi.Q<Label>("Mark").text = review.Mark.ToString("F1");
                    itemUi.Q<Button>("DeleteButton").clicked += () =>
                    {
                        ActualData.RemoveReview(review);
                        listView.Remove(itemUi);
                    };
                    itemUi.Q<Button>("EditButton").clicked += () =>
                    {
                        _uIController.ActualReview = review;
                        StateMachine.SetReviewsInputState();
                    };
                    listView.Add(itemUi);
                }
                VisualElement plus = _prefabPlusElement.Instantiate();
                listView.Add(plus);
                plus.Q<Button>("Add").clicked += () =>
                {
                    StateMachine.SetReviewsInputState();
                };
            }
        }
    }
}