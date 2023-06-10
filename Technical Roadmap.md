# Handiman — Technical Intro

## How to use this document

**Author & Tech Lead:** exitshadow

Please DM me if you encounter any difficulty during development! I am here to troubleshoot and debug with you! If something doesn’t work architecture-wise, we also can discuss what is needed to change.

## Overview

*Handiman* is a casual and short adventure game with minigames. This documentation aims to provide a roadmap for programmers to write C# classes / scripts designed to work as components for the GameObjects. It also determines where are the points of interaction and how the components should communicate together when needed.

## Design principles

The project is based on Object Oriented Programming coupled with Unity’s `Components` and `GameObjects` system.

### Encapsulation

Even if the project scope is very small, I’d recommend to encapsulate as much as possible with `private` accessors to avoid spaghetti and side-effects.

:arrow_right: Getters and setters **aren’t** needed at this point, as they would only complicate the code for no gain.

Public variables should be used only in the case the variable *has* to be accessed by another script. In that case, it is likely that we do not want to expose that variable in the inspector and mess with the values. In that case you should use the `[HideInInspector]` attribute.

### Reference injection

We’re opting to work through direct reference injections in the class member variables rather that `GameObject.Find()` in order to keep things clean.

In the context of Unity, a reference injection is when you expose the variable in the inspector either with a `public` accessor or a `[SerializeField]` attribute with a private member. **The use of `[SerializeField]` is to be preferred.**

### Game state

The game for now is designed to have a very short run with no saves. In order to keep track of all game variables, we’re opting for a Scriptable Object asset that will remain accessible through all scenes and referenced in the classes that need it by injection.

## Style Guidelines

### Class names

For the sake of clarity, class names should effectively express the *entity* that they represent. Since this project makes use of Scriptable Objects on the top of Components (see further below for more information on those), we want to distinguish between the two types of classes.

**Component / MonoBehaviour** classes can have any name that you find fit, as long as they effectively meet the expression requirement. There is no need to overthink it but let’s try to make it as expressive as possible. What’s nice is that they can be changed later on.

**Scriptable Object** classes always have to end by Asset to signify that it is a persistent asset in the same way a 3D model or a texture is an asset in the game.

### Variable names

In general, try to name your variables in a way that *explicitly states what they do*. Never use "a" or "b" because it makes the code hard to read and understand. Also, prefer stuff like `initialTimerSecondsValue` over `maxTime`.

End the name of the variable with a singular noun for single variables, and with a plural noun for arrays, lists or any other type of collection.

Do not use notations to state the type in the name of the variable such as `i_myIntVariable` or `arr_i_myArrayInt` or anything like that, as modern IDEs will show you the typing and check their compatibility with Omnisharp (the C# checker extension for most IDEs).

#### Casing

Contrarily to the Microsoft guidelines, we do not underscore private variable names. All member variables simply are in camelCase.

This is for the sake of not losing reference injections in the inspector by changing the name of the variable if we decide to move from private to public accessors and vice versa.

#### Booleans

Booleans should be named in a way that they non-ambiguously answer to a question by yes or no, and the effect of that yes or no should be clear. (This is also valid for naming methods that return a boolean)

Examples:

:white_check_mark: `isPlayerDead` 

:large_orange_diamond:`hasPlayerDied` 

:x: `dead`

Note the potential nuance between the two: `isPlayerDead`would refer to a current state, whereas `hasPlayerDied`would refer to a variable that changes only one, from the first time they died. An even better, less ambiguous phrasing would even be `hasPlayerAlreadyDiedOnce`. 

Always prefer a perfectly non-ambiguous expression.

### Methods names

Methods names always are written in PascalCase. They also should always represent an action, therefore be a *verb*, and express what they return on the base of which arguments. There is no need to indicate the return type or the arguments type as this is already provided by Omnisharp/IntelliSense.

Examples:

:white_check_mark: `CalculateTangentFromArc();`

:x: `ArcTangent();`

This method returns a representation of a tangent, that it calculates from a representation of an arc. `arcTangent` would more adequately be the variable name that captures the return of that method than the method name.

### Clean code

#### Split your methods as much as you can

If you find yourself writing a lot of code, split it in different methods instead of commenting steps. Ideally, a method should do *one and only one thing*. Exceptions to that are methods and functions that process a lot of stuff, but this isn’t likely to be the case in this project.

#### Make use of summary tag

Instead of writing comments on the top of a method, please use the C# `<summary>` tag in the comments above it. This will tell the IDE to show the comment about how a function is supposed to work when someone hovers over the method’s name.

Ideally, all methods should have a summary when they do something that cannot be explained in their names.

Examples:

``` csharp
/// <summary>
/// Adds two integers and returns the result.
/// </summary>
/// <param name="a">The first integer.</param>
/// <param name="b">The second integer.</param>
/// <returns>The sum of a and b.</returns>
public int Add(int a, int b)
{
    return a + b;
}
```

You can also summarize classes or pretty much anything, including classes.

```csharp
/// <summary>
/// Represents a book with a title, author, and publication date.
/// </summary>
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublicationDate { get; set; }
}
```

More on this here : https://marketsplash.com/tutorials/c-sharp/csharp-how-to-create-summary/#:~:text=A%20summary%20in%20C%23%20is,and%20what%20parameters%20it%20expects.



# Priority features

## General comments on architecture

It has been resolved that given the extremely small scope of the project, we’ll opt for a small script per entity or NPC, that inherit from an abstract class for common implementations. The inventory system is just about checking the values assigned to given variables. In the case the game succeeds and goes to further development, it should be replaced by a proper inventory and quest system.

## Commercial Package Dependencies

We use the TextAnimator commercial package to manage the animation of the text. I do own a license.

## Unity Built-In Dependencies

These are the main engine-specific things we’re going to use in the project.

I recommend consulting the corresponding Unity Manual entry for those at least for a quick overview before starting coding. Don’t hesitate to DM me for questions.

- Animator
- Canvas / RectTransform / Button / Toggle ... (Unity UI system elements)

- Unity Events

- Trigger Colliders
- NavMeshAgent (basic features)
- Action Input (new system)
- (will be updated in case)

## Scriptable Objects

**Scriptable Objects** are classes that are meant to be instanced as an asset in the Project folder of the game. Since they are stored as assets and not tied as a `Component` to a `GameObject` that gets destroyed every time a scene is closed, they can therefore be effectively be used to track data between scenes. Their data is persistent only during execution, and is reset every time the program is executed.

They can be scaled to a persistent data save system when used with serialization files, but this won’t be needed for this project during the time of the jam.

### `GameStateAsset.cs`

#### Member variables

All variables are public.

- boolean that checks if player has spoken to the butterfly neighbour
- boolean that checks if player has encountered the cat
- boolean that checks if player has found the tuna cans (alternatively, this can be verified by checking if there is tuna in the inventory)
- boolean that checks if player has fed the cat
- boolean that checks if the player has found the tree branch (alternatively, this can be verified by checking whether the butterfly tree branch is in the inventory)
- boolean that checks if the player has captured the butterflies
- boolean that checks if the player has beaten Dad’s Computer in the final battle

#### Methods

There are in principles no methods needed in this class.

### `InventoryAsset.cs`

#### General

The inventory could have been directly kept track of in the game state, but for the sake of readability and scalability, let’s make it another class. It will also be inherited from ScriptableObject and instanced as an asset in the Project folder.

#### Member variables

All variables are public.

- int that tracks amount of tuna cans
- int that tracks amount of butterfly tree branches

#### Methods

There are no methods needed in this class.

### `NPCDialogueAsset.cs`

This asset contains the lines that a NPC can say. Mostly based on @CaptainV’s previous dialogue system asset class:

https://github.com/CaptainVanessa/TLSC_v1/blob/dev/Assets/%23Project/Scripts/Dialogues/Dialogue.cs

## MonoBehaviours (Component scripts)

Those are classes that are used as `Components` in a `GameObject`. Contrarily to Scriptable Objects, their data is persistent only during the time of existence of the Game Object they are attached to.

### Managers

Those are scripts that are ubiquitous to the scene and have only one instance.

#### `GameManager.cs`

This is the handler for a `GameStateAsset` asset. Its job is to handle the data contained in the asset so other scripts don’t directly touch it. It will be present on a Game Object called "Game State" present in each scene, and components that need to access it will reference the Game State component and not the asset itself.

##### Member variables

- `private GameStateAsset stateAsset`

##### Methods

It will contain methods that assign new values to the variables of the referenced stateAsset, as well as eventual checks.

#### `InventoryManager.cs`

Also a handler, but for an `InventoryAsset`.

##### Member variables

- `private InventoryAsset inventoryAsset`

##### Methods

Methods that assign new values to the variables of the referenced inventoryAsset, as well as eventual checks.

#### `UIManager.cs`

The UI Display class is in charge of displaying all the texts in their respective places, as well as triggering specific animations when applicable. UI behaviour specific to a Game Object should be defined on a Component of that element.

##### Member variables

- every RectTransform that represents a UI group that we want to control (private)
- every TextAnimator / TextMeshProUIGUI fields that have to refresh their text (private)

##### Methods

Most of the methods of an UI manager are to be accessed through Unity Events, they therefore have to be public. Some can be accessed by other scripts. 

- Methods that open and close windows and trigger animations
- Methods that replace the text inside of the box. Flow management of the text animation should be taken in charge by the TextAnimator attached to the targeted text.

:arrow_right: There should be as many methods of opening / closing groups that there are groups referenced as member variables

:arrow_right: There should be as many methods of refreshing text as there are target TextAnimators & TMP elements

### NPCs

The NPCs have an abstract class NPC of which they inherit common behaviour such as the lines of the dialogue and the management of their animations

#### `NPC.cs`

##### Member variables

Accessor: protected (children of the class can access it but not external scripts)

Attribute `[SerializeField]` (otherwise it’s not going to show up in the inspector just as with private variables).

###### Global references

- a reference to a NPCDialogueAsset
- a reference to the GameManager
- a reference to the UIManager
- subscription to a delegate that listens to player interacting actions ?

###### GameObject references

This means the script requires components of the following types so it can find them and set the reference:

- a reference to the NPC Animator
- a reference to its Trigger Collider
- a reference to its NavMeshAgent

##### Public methods

- All methods needed to fetch the dialogues from the NPCDialogueAsset
- methods to inject the changing lines in the UIManager

##### Virtual methods

- Dialogue inializer. Most of it should be definid in the abstract class, specifics are defined by the inherited class (eg, a specific element to be checked etc.)

#### `Dad.cs`

Inherits from the `NPC` abstract class.

- any supplementary method seen fit

#### `ButterflyNeighbour.cs`

Inherits from the `NPC` abstract class.

- any supplementary method seen fit

#### Etc.

For every new character, the same architecture is used.

### Player Controls & Logic

#### `PlayerController.cs`

This is the class that manages the logic of the inputs to make the player execute actions

##### References

- Animator
- Collider
- Trigger Collider
- InputMap

##### Members (exposed)

- base speed
- run speed
- steering speed

##### Methods

- methods to manage inputs & turn them into parameters for moving & steering
- methods that use the parameters and logic to trigger or change animations in the Animator state machine

# Special Features

Those are given by order of priority.

## Annoying Phone Text Messages

### Scriptable Objects

#### `AnnoyingTextMessageAsset.cs`

##### Members

- image with the texture of the sender
- name of the sender
- content of the text

#### `AnnoyingPhone.cs`

It’s a component that is attached to the player Game Object.

##### Required features

- regularly sends off messages to the screen with annoying questions from friends
  - makes an annoying noise
  - shows that there is an unread notification and a button to tell the player to open it
  - regularly animates on the screen
  - waits for the player to push a button to read their waiting message
  - displays message
  - close message on closing button
  - only sends off a new message if last message is read

##### References

- UI Display, since it has to change stuff
- list of `AnnoyingTextMessage` assets
- Player Controller

## Get The Tuna

## Feed The Cat

## Capture The Butterflies

## Clean Dad’s Computer



