Needs to be able to add buttons

Point & Click Function

Button List

When click on GameObject added into the button list
	Default cursor changes to Clickable cursor when hover over interactable
	Clickable cursor changes to a Clicked cursor
	Button  

Create a list that allows easy creation of interactable buttons

Buttons to press

Functions when button is pressed

[[Cat Position Controls.canvas|Cat Position Controls]]

Scrollable Icons
	Allow players to scroll icons and re-arrange the sequence of cat icons. This will affect the positions of the cats in-game.
		**!!Gameplay!!**
		Giving players the ability to strategize and play with positionings
	


### ✅ Key Sync Logic Principles

1. **Manager is authoritative**
    
    - `SlidingIcons` holds the `icons` list, defines positions, and reorders icons.
        
    - Each `DraggableIcon` just knows its `iconIndex` and calls manager when dropped.
        
2. **Do not overwrite indices every frame**
    
    - Only update indices **when the order actually changes** (initial assignment + after snapping).
        
3. **Avoid `GameObject.Find` for dynamic lists**
    
    - Instead, use the hierarchy (children of a known parent) or inspector assignment.
        
    - This ensures `Start()` always finds all icons and adds them to the manager properly.
        
4. **Drag state separation**
    
    - `isDragging` prevents your smooth Lerp from fighting the mouse movement.