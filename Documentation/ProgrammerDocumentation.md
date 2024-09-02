# Programmer Documentation

## Assests and Plugins
- Unity UI Toolkit
- [Zenject 9.2.0](https://github.com/modesttree/Zenject)
- [Unity Native File Picker v1.3.4](https://github.com/yasirkula/UnityNativeFilePicker)
- [Native Gallery for Android & iOS v1.8.1](https://github.com/yasirkula/UnityNativeGallery)
- [Native Share for Android & iOS v1.5.2](https://github.com/yasirkula/UnityNativeShare)
- [Newtonsoft Json 13.0.2](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html)

## Architecture

![UML](./diagram.png)

### Package Data

The package which is responsible for all data classes in the project responsible for the description of the board game.

Classes:
- GameData - describes the board game itself, stores information about all reviews, games played, characters and images of the game.
- Review - describes the review separately, stores the author's name, rating and comment.
- Game - describes a separately played game, stores the number of players, game duration time, comments, as well as winners and losers in this game.
- Character - describes the character in this board game, stores information about the name, the number of games played, the number of wins and the total number of points scored.
- GameDataFactory - is a class that stores all games, loading them from PlayerPrefs with the key(“PlayerData”) or creating new ones in the absence of the key(“PlayerData”).

### Package States

The package is a realization of the design pattern “State”. There is a StateMachine that allows switching between states and then calling the Entry and Exit key methods.

Classes:
- IState - interface for each state with Entry and Exit methods.
- BaseState - is an abstract class for states that stores reference to UIController object and VisualElement of this state, causes VisualElement to be turned off and on when it appears or disappears. Implements the IState interface.
- StateMachine - class that stores all the states created in the game as well as the current state. Realizes methods of transition between states.
- MainState - state of the main menu. Inherits BaseState.
- GameState - state of the game menu. Inherits BaseState.
- ActionsState - state of the actions menu. Inherits BaseState.
- _OtherStates - state of the others menu. Inherits BaseState._
- ...

### Package UIStateControllers

A package that implements the change and control side of the interface. Class instances are created in a UIController object and stored in a dictionary. These states have key methods Installization, Update (triggered each time the state is run), Clear (used to clear the memory).

Classes:
- IUIState - interface for each state controller with Installization, Update and Clear methods.
- UIStateControllerBase - is an abstract class which is a father class for all state controllers, it stores references to UIController. Implements the IUIState interface.
- MainUIStateController - is responsible for interface controls for MainState. Inherits UIStateControllerBase.
- GameUIStateController - is responsible for interface controls for GameState. Inherits UIStateControllerBase.
- ActionsStateController - is responsible for interface controls for ActionsState. Inherits UIStateControllerBase.
- _OtherStateControllers - is responsible for interface controls for OthersState. Inherits UIStateControllerBase._

### Other classes

- SceneInstaller - application entry point, bootstrap, creates all objects and defines dependencies. Inherits MonoInstaller, uses Zenject library.
- MigrationController - responsible for methods that refine GameData in case of changes due to new versions of the application. It is done step by step for each version. It is a static class.
- ShareController - implements the use of the Native Gallery for Android and Native Share for Android libraries for importing and exporting. It is a static class.
- UIController - is a linking class in the application. It performs various actions that can be repeated in the state control classes. It also stores information to be transferred between states. It has Dictionary<Type, IUIState>().

### Solving specific problems

#### Start menu problem

A start menu has been added to the application to solve one of the problems caused by the too fast creation of objects using Zenjects that work with VisualElement. Some objects such as the UIStateControllerBase class implementers search for objects using the Q<>() method and can't find anything because VisualElement hasn't had time to create all its children yet.
To solve this problem we implemented a timer via IEnumerator for 0.2 seconds and after that all objects are created and dependencies are described.

