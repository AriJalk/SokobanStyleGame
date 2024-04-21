# SimplePuzzleGame

Developed within a 3-week constraint, this simple turn-based Sokoban-style game in Unity (C#) uses entanglement-like behavior and interactions between objects of different types/colors. The game supports built-in levels as well as custom levels created in the in-game level editor, which features intuitive 'painting' tools for level design. Additionally, players can fix mistakes with the undo feature that allows them to revert moves all the way back to the start of the level, along with an instant restart button for quick puzzle resets.

## Controls

* **WASD/Arrows/Numpad**: Move Player/s
* **Z**: Undo last move
* **R**: Restart level

## Implemented Mechanics

* Place correct colored cubes on all goal tiles requiring them in the level to win.
* Pushing objects can cascade to neighbor objects depending on their properties. Blue cubes are linked to red cubes in such a way that a push of a single blue cube will push all red cubes in the level in the same direction. Red does the same but blue cubes will be pushed in the opposite direction. These kinds of interactions are easily customizable and expandable in code.
* Multiple player actors can exist in the scene at the same time, and they all respond to the same input simultaneously.
* Borders prevent movement between tiles (1-way borders were a potential mechanic that would be easy to implement, but I had to focus on more important aspects).
* Level-Editor supports the previously mentioned elements and saves the level in the JSON format used to load and play the levels. Adding and deleting elements can be done by clicking or dragging the mouse to 'paint' the tiles.

## Pictures

### Menu

![MainMenu](ReadmeImages/SPG_MainMenu.png)

### Level Select

![LevelSelect](ReadmeImages/SPG_CustomLevelSelect.png)

### Editor

![Editor](ReadmeImages/SPG_Editor.png)

### Saved Level

![SavedLevel](ReadmeImages/SPG_EditorExample.png)

### Saved Level Solved

![SavedLevelSolved](ReadmeImages/SPG_EditorExampleSolved.png)

### Level Example

![LevelExample](ReadmeImages/SPG_gif.gif)

## Main Goals for Development

* A fully functional game with modular push mechanics and a level editor.
* Make things relatively lightweight, makes objects behavior and instancing shared as much as possible (used flyweight/type object pattern for this) so large levels should remain lightweight.
* The game must have undo and instant restart feature (solved with the Command pattern).

## Challenges

* Making the push mechanic consistent with all edge cases and easily controlled in a chain of pushes, solved by defining each type.
* The level editor took the bulk of the work since it required a lot of UI code work so the level is displayed correctly for editing and bound correctly to the level coordinates.
* JSON serialization of levels required extra code work to serialize/deserialize a list of custom data types like barriers.
* Ensuring consistency between the level editor UI, Game Grid Coordinates, and the Game World itself in the scene was a bit of a trial and error.

## What I Would've Done Differently

* More levels: Most of the 2 weeks went to making sure everything works correctly, not leaving enough time to build enough levels to really explore the possibilities of the mechanics.
* Aesthetic improvements: Would've liked to make it more aesthetic. Using primitive shapes was the fastest way to make it work in the limited time frame without using existing assets.
* Command pattern in the level editor: Would've added the command pattern to the level editor given more time, making it easy to fix mistakes instead of reapplying the correct element.
