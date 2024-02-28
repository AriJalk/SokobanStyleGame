# SimplePuzzleGame

Developed within a 2-week constraint, this simple turn-based Sokoban-style game in Unity (C#) uses entanglement-like behavior and interactions between objects of different types/colors. The game supports built-in levels as well as custom levels created in the in-game level editor, which features intuitive 'painting' tools for level design. Additionally, players can fix mistakes with the undo feature that allows them to revert moves all the way back to the start of the level, along with an instant restart button for quick puzzle resets.

## Controls:

* WASD/Arrows/Numpad : Move Player/s
* Z: Undo last move
* R: Restart level

## Implemented Mechanics:
* Place correct colored cubes on all goal tiles requiring them in the level to win.
* Pushing objects can cascade to neighbor objects depending on their properties, also blue cubes are linked to red cubes in such a way that a push of a single blue cube will push all red cubes in the level in the same direction. Red does the same but blue cubes will be pushed in the opposite direction. These kinds of interactions are easily customizable and expandable in code.
* Multiple player actors can exist in the scene at the same time, and they all respond to the same input simultaniously.
* Borders prevent movement between tiles (1-way borders were a potential mechanic that would be easy to implement, but I had to focus on more important aspects).
* Level-Editor supports the previously mentioned elements and saves the level in the json format used to load and play the levels, adding and deleting elements can be done by clicking or dragging the mouse to 'paint' (I wanted to move the editor architecture to command based to support undo like the game itself, but it wouldn't fit under the time constraint, so the only way to undo is to delete or replace the spot).

## Pictures

Menu

![MainMenu](ReadmeImages/SPG_MainMenu.png)</br>

Level Select

<img src = 'ReadmeImages/SPG_CustomLevelSelect.png' width = '800'/>

Editor

![SPG_Editor](ReadmeImages/SPG_Editor.png)</br>

Saved Level

![SPG_EditorExample](ReadmeImages/SPG_EditorExample.png)</br>

Saved Level Solved
![SPG_EditorExampleSolved](ReadmeImages/SPG_EditorExampleSolved.png)</br>

Level Example

![SPG_gif](ReadmeImages/SPG_gif.gif)</br>
