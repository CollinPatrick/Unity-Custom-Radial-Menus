# Unity-Custom-Radial-Menus
Create customizable dynamic radial menus in Unity.

## RadialMenu
**RadialMenu** is a monobehavior class that wraps the classes **RadialMenuGroup** and **RadialMenuSection**, and handles the generation and selections of the menu.

<details>
  <summary>Editor Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  private GameObject | root | The root object of the menu. The menu will be generated inside this object.
  private GameObject | sectionPrefab | The default prefab to use when building radial menu sections.
  private GameObject | groupDividerPrefab | A prefab that is placed between each group.
  private GameObject | sectionDividerPrefab | A prefab that is placed between each section.
  private bool | buildOnStart | If enabled, the radial menu will build when start runs.
  private List<RadialMenuGroup> | groups | The groups that make up the radial menu.
  public bool | selectOnHover | If enabled, selections will be made automatically when a section is hovered. This is useful when using a gamepad or making quick selections.<br>**Per group selection does not support this.**
  public bool | perGroupSelection | Allows groups to have multiple selections per group.
   </details>
  
  <details>
    <summary>Public Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  public Vector2 | selectDirection | The direction of the selection. Set this value using input such as mouse delta or gamepad stick axis.
   </details>
  
  <details>
  <summary>Readonly Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  public GameObject | root | The root object of the menu. The menu will be generated inside this object.
  public RadialMenuGroup | hoveredGroup | The group currently being hovered.
  public RadialMenuSection | hoveredSection | The section currently being hovered.
   </details>

<details>
  <summary>Public Methods</summary>
  
  Return Type | Method | Summary
  ----------- | ------ | -------
  void | BuildMenu() | Builds/rebuilds the radial menu using current settings.
  void | AddSection(string aGroupKey, RadialMenuSection aSection) | Adds a section to a group with the specified group key.
  void | AddSection(string aGroupKey, RadialMenuSection aSection, int aIndex) | Adds a section to a group with the specified group key at the specified index.
  void | AddGroup(RadialMenuGroup aSection) | Adds a group to the radial menu.
  void | AddGroup(RadialMenuGroup aSection, int aIndex) | Adds a group to the radial menu at the specified index.
  RadialMenuSection | GetSection(string aGroupKey, string aSectionKey) | Finds and returns a RadialMenuSection via a group key and section key.
  RadialMenuSection | GetSection(Vector2 aDirection, Vector2 aDeadZone) | Returns a RadialMenuSection using a direction and deadzone.
  RadialMenuGroup | GetGroup(string aGroupKey) | Finds and returns a RadialMenuGroup by its key.
  RadialMenuGroup | GetGroup(Vector2 aDirection, Vector2 aDeadZone) | Returns a RadialMenuGroup using a direction and dedzone.
  void | SelectHoveredSection() | Selects the hovered section.
  void | SelectSection(string aGroupKey, string aSectionKey) | Selects a section via a group and section key.
  Dictionary<string, List<string>> | GetSelections() | Returns the selections of all groups in the menu.
  List<string> | GetSelection() | Returns the selections of the first avalible group. Useful if the menu only has one group.
  List<string> | GetSelection( string aGroupKey ) | Retruns the selections of a specified group by key.
 </details>
  
  ## RadialMenu.RadialMenuGroup
**RadialMenuGroup** is a serialized class that defines the functionality of a radial menu group.

<details>
  <summary>Constructors</summary>
  
  ```c#
  new RadialMenuGroup( string aKey, int aDegrees, int aMaxSelections = 1 );
  ```
  ```c#
  new RadialMenuGroup( string aKey, int aDegrees, int aAdditionalFillClip, int aMaxSelections = 1 );
  ```
  ```c#
  new RadialMenuGroup( string aKey, int aDegrees, int aAdditionalFillClip, GameObject aSectionPrefabOverride, int aMaxSelections = 1 );
  ```
  </details>
  
<details>
  <summary>Editor Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  private string | groupKey | A unique key to identify this selection group.
  private int | degrees | How many degress this group will use. Make sure all groups add up to 360 degrees.
  private int | additionalFillClip | Additional clipping to add to section fills. This is useful if you are using dividers between sections since dividers do not take up space.
  public int | maxSelections | The max number of selections that can be made in this group. This number is ignored if per group selection is disabled.
  public GameObject | sectionPrefabOverride | Use this if you want to use a prefab instead of sprite fills. Section size is ignored.
  public List<RadialMenuSection> | sections | The sections inside this group.
   </details>
  
  <details>

  <summary>Readonly Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  public string | groupKey | A unique key to identify this selection group.
  public int | additionalFillClip | Additional clipping to add to section fills. This is useful if you are using dividers between sections since dividers do not take up space.
  public float | startDegrees | The degree this section group starts.
  public float | endDegrees | The degree this section group ends.
  public IReadOnlyList<RadialMenuSection> | selectedSections | A readonly list containing the ids of all selected sections inside the group.
  public RadialMenuSection | selectedSection | The first selection inside the group. Useful if a group does not support more than one selection.
   </details>

<details>
  <summary>Public Methods</summary>
  
  Return Type | Method | Summary
  ----------- | ------ | -------
  void | AddSection(RadialMenuSection aSection) | Adds a section to the end of the group.
  void | AddSection(RadialMenuSection aSection, int aIndex) | Adds a section to the group at a specified index.
 </details>
    
  ## RadialMenu.RadialMenuSection
**RadialMenuGroup** is a serialized class that defines the functionality of a radial menu section.

<details>
  <summary>Constructors</summary>

  ```c#
  new RadialMenuSection( string aKey );
  ```
  ```c#
  new RadialMenuSection( string aKey, Sprite aSprite, bool aRepositionSprite = true );
  ```
  ```c#
  new RadialMenuSection( string aKey, Sprite aSprite, Vector2 aSpriteSize, bool aRepositionSprite = true );
  ```
  ```c#
  new RadialMenuSection( string aKey, Sprite aSprite, Vector2 aSpriteSize, Color aSpriteColor, bool aRepositionSprite = true );
  ```
  </details>
  
<details>
  <summary>Editor Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  private string | sectionKey | A unique key to identify this selection inside its group.
  private Sprite | displaySprite | The sprite to display for this section.
  private Vector2 | spriteSize | The size of the display sprite.
  private Color | spriteColor | The color of the display sprite.
  public bool | repositionSprite | Should the sprite be repositioned to the center of the section along the circumference of the menu.
  public UnityEvent<string, string> | OnSelect | An event that fires when the section is selected with the group and section key.
  public UnityEvent<string, string> | OnDeselect | An event that fires when the section is deselected with the group and section key.
  </details>
  
  <details>

  <summary>Readonly Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  public string | sectionKey | A unique key to identify this selection inside its group.
  public bool | selected | Is this section currently selected?
  public bool | hovered | Is this section currently being hovered?
  public Sprite | displaySprite | The sprite to display for this section.
  public Vector2 | spriteSize | The size of the display sprite.
  public Color | spriteColor | The color of the display sprite.
  public float | startDegree | The degree this section starts.
  public float | endDegree | The degree this section ends.
  </details>

<details>
  <summary>Public Methods</summary>
  
  Return Type | Method | Summary
  ----------- | ------ | -------
  void | UpdateSprite | Updates the sprite, sprite size, and sprite color to the current settings.
  void | Select(string aGroupKey) | Select this section.
  void | Deselect(string aGroupKey) | Deselects this section.
  void | SetSprite(Sprite aSprite, Vector2 aSize) | Sets the section's sprite with the supplied size.
  void | SetSprite(Sprite aSprite, Vector2 aSize, Color aColor) | Sets the section's sprite with the supplied size and color.
 </details>
  
## RadialMenuSectionObject
**RadialMenuSectionObject** is a Monobehavior class that is attched to the section prefab. This class stores references to UI dependencies and handles hover and selection effects.

  <details>
  <summary>Editor Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  private Image | backgroundImage | The background image of this section. The sprite radial fill will be applied to this image.
  private Image | displayImage | The icon inside this section. The sprite will automatically be set and centered inside the section.
  private GameObject | selectedOverlay | An overlay object that is enabled when the section is selected.
  private Color | selectedColor | The color of the selected overlay.
  private GameObject | hoverOverlay | An overlay object that is enabled when the section is hovered.
  private Color | hoverColor | The color of the hover overlay.
  </details>
  
  <details>

  <summary>Readonly Variables</summary>
  
  Type | Name | Summary
  ---- | ---- | -------
  public Image | backgroundImage | The background image of this section. The sprite radial fill will be applied to this image.
  public Image | displayImage | The icon inside this section. The sprite will automatically be set and centered inside the section.
  </details>

## Example
99% of the radial menu can be built inside the RadialMenu component without any extra code, but the radial menu does support adding and removing sections programmically as well.
  
To determine the direction of the menu selection, all you have to do is set the ``selectDirection`` variable inside the RadialMenu object. Everything else is handled automatically. The following code will handle the direction calculation for the mouse and controllers. For controllers, just input the direction axis from a stick.
```c#
public void SetInputDirection( Vector2 aDirection, bool aControllerInput ) {
    if ( aControllerInput ) {
        radialMenu.selectDirection = aDirection;
    }
    else {
        radialMenu.selectDirection = ( Input.mousePosition - new Vector3( Screen.width / 2, Screen.height / 2 ) - Vector3.zero ).normalized;
    }
}
```
To make a selection inside the radial menu, just call ``SelectHoveredSection()`` inside the RadialMenu object. 
  
To create the radial menu programmatically, all you have to do is create each group and their sections then add them to the menu.
```c#
public void GenerateRadialMenu() {
    //Create and populate group 1
    RadialMenu.RadialMenuGroup lGroup1 = new RadialMenu.RadialMenuGroup( "Group1", 180 );
    for ( int i = 1; i <= 3; i++ ) {
        RadialMenu.RadialMenuSection lSection = new RadialMenu.RadialMenuSection( "1-Section" + i, iconSprite, Vector2.zero, Color.white );
        lGroup1.AddSection( lSection );
    }

    //Create and populate group 2
    RadialMenu.RadialMenuGroup lGroup2 = new RadialMenu.RadialMenuGroup( "Group2", 180, 2 );
    for ( int i = 1; i <= 5; i++ ) {
        RadialMenu.RadialMenuSection lSection = new RadialMenu.RadialMenuSection( "2-Section" + i, iconSprite, Vector2.zero, Color.white );
        lGroup2.AddSection( lSection );
    }

    //Add the groups to the radial menu
    radialMenu.AddGroup( lGroup1 );
    radialMenu.AddGroup( lGroup2 );

    //Build the menu
    radialMenu.BuildMenu();

    //Automatically select the first section.
    radialMenu.SelectSection( "Group1", "Section1" );
}
```
To get the current selections, just call ``GetSelection()`` or ``GetSelections()``
```c#
List<string> selection;
selection = radialMenu.GetSelection(); //1-Section1
radialMenu.SelectSection( "Group1", "1-Section2" );
selection = radialMenu.GetSelection(); //1-Section2
radialMenu.SelectSection( "Group2", "2-Section2" );
selection = radialMenu.GetSelection(); //1-Section2, 2-Section2
radialMenu.SelectSection( "Group2", "2-Section5" );
selection = radialMenu.GetSelection(); //1-Section2, 2-Section2, 2-Section5

Dictionary<string, List<string>> selections = radialMenu.GetSelections(); // Group1{1-Section2}, Group2{2-Section2, 2-Section5}

radialMenu.SelectSection( "Group2", "2-Selection2" ); //The selection is toggled when multiple selections are allowed.
List<string> selection = radialMenu.GetSelection(); //1-Section2, 2-Section5
```
Realistically, if you were constructing this menu for something like an inventory, the section key would be the name or id of the object in the players inventory. When you get the selection string, you would use that to figure out what was actually selected. 
  
## Tips
* All prefabs are instatntiated at the 0,0,0 coordinate of the root game object.
  * If you are using the fill clip method, make sure the pivot is at the center of the prefab.
  * If you are using the override prefab method, make sure the pivot is at 0,0,0 and the UI elements are moved to the disired radius distance of the menu. 
