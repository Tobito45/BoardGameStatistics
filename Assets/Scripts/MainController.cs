using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainController : MonoBehaviour
{
    private VisualElement _root;
    private ScrollView _listView;


    private List<string> _items;
    [SerializeField]
    private VisualTreeAsset _prefabElement;

    void Start()
    {
        _items = new List<string>() { "1", "2", "3", "4" };
        _root = GetComponent<UIDocument>().rootVisualElement;
        _listView = _root.Q<ScrollView>("List");
        //_prefabElement = _root.Q<VisualElement>("TestElement");

        //_listView.Add(_prefabElement);
        //_prefabElement.style.display = DisplayStyle.Flex;

        // Set up a make item function for a list entry
        /*_listView.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = _prefabElement.Instantiate();

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        _listView.bindItem = (item, index) =>
        {
        };

        // Set a fixed item height matching the height of the item provided in makeItem. 
        // For dynamic height, see the virtualizationMethod property.
       // _listView.fixedItemHeight = 750;

        // Set the actual item's source list/array
        _listView.itemsSource = _items;
    */

        for (int i = 0; i < 5; i++)
        {
            var itemUi = _prefabElement.Instantiate();
            _listView.Add(itemUi);

        }
    }

   
}
