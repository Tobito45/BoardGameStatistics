using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIStateControllers
{
    public class ReviewsInputUIStateController : UIStateControllerBase
    {
        public ReviewsInputUIStateController(UIController uIController) : base(uIController) { }

        public override void Installization(VisualElement visualElement)
        {
            visualElement.Q<Button>("BackButton").clicked += () => StateMachine.SetReviewsState();
            visualElement.Q<Button>("AddButton").clicked += () => SaveNewReview(visualElement);

            FloatField markInput = visualElement.Q<FloatField>("MarkInput");
            visualElement.RegisterCallback<ChangeEvent<float>>(evt => ValidateFloatMark(markInput));

        }
        public override void Clear(VisualElement visualElement) { }

        public override void Update(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            TextField textFieldText = visualElement.Q<TextField>("TextInput");
            textFieldName.value = string.Empty;
            textFieldText.value = string.Empty;
            _uIController.SetInputFieldColor(textFieldText, Color.white, 0);
            _uIController.SetInputFieldColor(textFieldName, Color.white, 0);
            visualElement.Q<FloatField>("MarkInput").value = 0.0f;
        }

        private void ValidateFloatMark(FloatField floatField)
        {
            if (floatField.value > 10)
                floatField.value = 10;
            if (floatField.value < 0)
                floatField.value = 0;
        }

        private bool Validate(TextField textFieldName, TextField textFieldText)
        {
            bool result = true;
            if (textFieldName.value == string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldName, Color.red, 2);
                result = false;
            }
            if (textFieldText.value == string.Empty)
            {
                _uIController.SetInputFieldColor(textFieldText, Color.red, 2);
                result = false;
            }
            return result;
        }

        private void SaveNewReview(VisualElement visualElement)
        {
            TextField textFieldName = visualElement.Q<TextField>("NameInput");
            TextField textFieldText = visualElement.Q<TextField>("TextInput");
            float mark = visualElement.Q<FloatField>("MarkInput").value;

            if (!Validate(textFieldName, textFieldText))
            {
                _uIController.SetInputFieldColor(textFieldText, Color.red, 2);
                _uIController.SetInputFieldColor(textFieldName, Color.red, 2);
                return;
            }
            _uIController.GetActualData.AddReview(new Review(textFieldName.value, mark, textFieldText.value));
            StateMachine.SetReviewsState();
        }
    }
}