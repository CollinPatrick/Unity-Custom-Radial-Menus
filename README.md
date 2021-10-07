# Unity-Custom-Radial-Menus
Create customizable dynamic radial menus in Unity.

## RadialMenu
**RadialMenu** is a monobehavior class that wraps the classes **RadialMenuGroup** and **RadialMenuSection**.

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
