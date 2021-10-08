using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    [System.Serializable]
    public class RadialMenuSection {
        private bool _selected = false;
        private bool _hovered = false;

        [SerializeField] private string _sectionKey = "New Section";
        [SerializeField] private Sprite _displaySprite = null;
        [SerializeField] private Vector2 _spriteSize = Vector2.zero;
        [SerializeField] private Color _spriteColor = Color.white;
        public bool repositionSprite = true;

        [ReadOnly] public float startDegree = 0;
        [ReadOnly] public float endDegree = 0;
        [ReadOnly] public RadialMenuSectionObject obj = null;

        public UnityEvent<string, string> OnSelect = new UnityEvent<string, string>();
        public UnityEvent<string, string> OnDeselect = new UnityEvent<string, string>();

        /// <summary>
        /// Is this section currently selected?
        /// </summary>
        public bool selected => _selected;
        
        /// <summary>
        /// Is this section currently being hovered?
        /// </summary>
        public bool hovered => _hovered;

        /// <summary>
        /// A unique key to identify this selection inside its group.
        /// </summary>
        public string sectionKey => _sectionKey;

        /// <summary>
        /// The sprite to display for this section.
        /// </summary>
        public Sprite displaySprite => _displaySprite;

        /// <summary>
        /// The size of the display sprite.
        /// </summary>
        public Vector2 spriteSize => _spriteSize;

        /// <summary>
        /// The color of the display sprite.
        /// </summary>
        public Color spriteColor => _spriteColor;

        public RadialMenuSection( string aKey, Sprite aSprite, Vector2 aSpriteSize, Color aSpriteColor, bool aRepositionSprite = true ) : this(aKey, aSprite, aSpriteSize, aRepositionSprite ){
            _spriteColor = aSpriteColor;
        }

        public RadialMenuSection( string aKey, Sprite aSprite, Vector2 aSpriteSize, bool aRepositionSprite = true ) : this(aKey, aSprite, aRepositionSprite) {
            _spriteSize = aSpriteSize;
        }

        public RadialMenuSection( string aKey, Sprite aSprite, bool aRepositionSprite = true ) : this( aKey ) {
            _displaySprite = aSprite;
            repositionSprite = aRepositionSprite;
        }
        public RadialMenuSection( string aKey ) {
            if( string.IsNullOrEmpty(aKey) ) {
                _sectionKey += Guid.NewGuid().ToString();
            }
            else {
                _sectionKey = aKey;
            }
        }

        /// <summary>
        /// Updates the sprite, sprite size, and sprite color to the current settings.
        /// </summary>
        public void UpdateSprite() {
            if ( obj != null ) {
                obj.displayImage.sprite = _displaySprite;
                obj.displayImage.color = _spriteColor;
                if ( _spriteSize != Vector2.zero ) {
                    obj.displayImage.gameObject.GetComponent<RectTransform>().sizeDelta = _spriteSize;
                }
            }
        }

        public void HoverEnter() {
            _hovered = true;
            obj.OnHoverEnter();
        }

        public void HoverExit() {
            _hovered = false;
            obj.OnHoverExit();
        }

        /// <summary>
        /// Select this section.
        /// </summary>
        public void Select( string aGroupKey ) {
            _selected = true;
            OnSelect?.Invoke( aGroupKey, sectionKey );
            obj.OnSelect();
        }

        /// <summary>
        /// Deselect this section. 
        /// </summary>
        public void Deselect( string aGroupKey ) {
            _selected = false;
            OnDeselect?.Invoke( aGroupKey, sectionKey );
            obj.OnDeselect();
        }

        /// <summary>
        /// Sets the section's sprite with the supplied size.
        /// </summary>
        public void SetSprite( Sprite aSprite, Vector2 aSize ) {
            _displaySprite = aSprite;
            _spriteSize = aSize;

            if( obj != null ) {
                obj.transform.GetChild( 0 ).GetComponent<Image>().sprite = _displaySprite;
                if( aSize != Vector2.zero ) {
                    obj.transform.GetChild( 0 ).GetComponent<RectTransform>().sizeDelta = _spriteSize;
                }
            }
        }

        /// <summary>
        /// Sets the section's sprite with the supplied size and color.
        /// </summary>
        public void SetSprite( Sprite aSprite, Vector2 aSize, Color aColor ) {
            _displaySprite = aSprite;
            _spriteSize = aSize;
            _spriteColor = aColor;

            UpdateSprite();
        }
    }

    [System.Serializable]
    public class RadialMenuGroup {
        /// <summary>
        /// A unique key to identify this selection group.
        /// </summary>
        [SerializeField] private string _groupKey = "New Group";
        
        /// <summary>
        /// How many degress this group will use. Make sure all groups add up to 360 degrees.
        /// </summary>
        [Range(1, 360)] public int degrees = 360;
        
        /// <summary>
        /// The max number of selections that can be made in this group. This number is ignored if per group selection is disabled.
        /// </summary>
        public int maxSelections = 1;
        
        [SerializeField] private int _additionalFillClip = 0;
        
        /// <summary>
        /// Use this if you want to use a prefab instead of sprite fills. Section size is ignored.
        /// </summary>
        public GameObject sectionPrefabOverride;

        /// <summary>
        /// The sections inside this group.
        /// </summary>
        public List<RadialMenuSection> sections = new List<RadialMenuSection>();

        /// <summary>
        /// The degree this section group starts.
        /// </summary>
        [ReadOnly] public float startDegree = 0;
        
        /// <summary>
        /// The degree this section group ends.
        /// </summary>
        [ReadOnly] public float endDegree = 0;

        /// <summary>
        /// A unique key to identify this selection group.
        /// </summary>
        public string groupKey => _groupKey;

        /// <summary>
        /// Additional clipping to add to section fills. This is useful if you are using dividers between sections since dividers do not take up space.
        /// </summary>
        public int additionalFillClip => _additionalFillClip;

        [SerializeField, HideInInspector] private List<RadialMenuSection> _selectedSections = new List<RadialMenuSection>();

        /// <summary>
        /// A readonly list containing the ids of all selected sections inside the group.
        /// </summary>
        public IReadOnlyList<RadialMenuSection> selectedSections => _selectedSections;

        /// <summary>
        /// The first selection inside the group. Useful if a group does not support more than one selection. 
        /// </summary>
        public RadialMenuSection selectedSection => selectedSections[0];

        #region Constructors
        public RadialMenuGroup( string aKey, int aDegrees, int aAdditionalFillClip, GameObject aSectionPrefabOverride, int aMaxSelections = 1 ) : this( aKey, aDegrees, aAdditionalFillClip, aMaxSelections ) {
            sectionPrefabOverride = aSectionPrefabOverride;
        }

        public RadialMenuGroup( string aKey, int aDegrees, int aAdditionalFillClip, int aMaxSelections = 1 ) : this( aKey, aDegrees, aMaxSelections ) {
            _additionalFillClip = aAdditionalFillClip;
        }

        public RadialMenuGroup( string aKey, int aDegrees, int aMaxSelections = 1 ) {
            degrees = Mathf.Clamp( aDegrees, 1, 360 );
            maxSelections = aMaxSelections;
            if ( string.IsNullOrEmpty( aKey ) ) {
                _groupKey += Guid.NewGuid().ToString();
            }
            else {
                _groupKey = aKey;
            }

            sections = new List<RadialMenuSection>();
            _selectedSections = new List<RadialMenuSection>();
        }
        #endregion

        /// <summary>
        /// Adds a section to the end of the group.
        /// </summary>
        public void AddSection( RadialMenuSection aSection ) {
            foreach ( RadialMenuSection lSection in sections ) {
                if ( lSection.sectionKey == aSection.sectionKey ) {
                    Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to add new section \"{aSection.sectionKey}\" to group \"{groupKey}\". Group already contains section with given key." );
                    return;
                }
            }
            sections.Add( aSection );
        }

        /// <summary>
        /// Adds a section to the group at a specified index.
        /// </summary>
        public void AddSection( RadialMenuSection aSection, int aIndex ) {
            foreach ( RadialMenuSection lSection in sections ) {
                if ( lSection.sectionKey == aSection.sectionKey ) {
                    Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to add new section \"{aSection.sectionKey}\" to group \"{groupKey}\". Group already contains section with given key." );
                    return;
                }
            }
            sections.Insert( aIndex, aSection );
        }

        public void OnSelectSection( string aGroupKey, string aSectionKey, bool aPerGroupSelection ) {
            //Each group can have it's own selection. Nothing in this group was selected, so ignore the new selection.
            if ( aPerGroupSelection && aGroupKey != groupKey ) {
                return;
            }
            //Disable everything in this group
            else if (!aPerGroupSelection && aGroupKey != groupKey ) {
                foreach( RadialMenuSection sect in _selectedSections ) {
                    sect.Deselect( aGroupKey );
                }
                _selectedSections.Clear();
                return;
            }

            //Find section in this group
            RadialMenuSection lSection = null;
            foreach ( RadialMenuSection sect in sections ) {
                if ( sect.sectionKey == aSectionKey ) {
                    lSection = sect;
                    break;
                }
            }

            //Make sure the section exists
            if( lSection == null ) {
                Debug.LogWarning( $"[{typeof( RadialMenu )}] - Unable to select section \"{aSectionKey}\" to group \"{aGroupKey}\". Section with given key does not exist in group." );
                return;
            }

            //Multiple items can be selected
            if ( maxSelections > 1 ) {
                //Section is already selected, deselect
                if ( lSection.selected ) {
                    lSection.Deselect( groupKey );
                    _selectedSections.Remove( lSection );
                    return;
                }
                //Max selections already reached, cant select
                else if ( _selectedSections.Count >= maxSelections ) {
                    return;
                }
                //Add section to selections
                else {
                    lSection.Select( groupKey );
                    _selectedSections.Add( lSection );
                }
            }
            else {
                foreach( RadialMenuSection sect in _selectedSections ) {
                    sect.Deselect( groupKey );
                }
                _selectedSections.Clear();

                lSection.Select( groupKey );
                _selectedSections.Add( lSection );
            }
        }
    }

    public Vector2 Direction_Dead_Zone { get{ return new Vector2( 0.25f, 0.25f ); } }

    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _sectionPrefab;
    [SerializeField] private GameObject _groupDividerPrefab;
    [SerializeField] private GameObject _sectionDividerPrefab;

    /// <summary>
    /// If enabled, the radial menu will build when start runs.
    /// </summary>
    [SerializeField] private bool _buildOnStart = true;

    /// <summary>
    /// If enabled, selections will be made automatically when a section is hovered.
    /// This is useful when using a gamepad or making quick selections.
    /// <br></br>
    /// per group selection does not support this.
    /// </summary>
    public bool selectOnHover = true;
    
    /// <summary>
    /// Allows groups to have multiple selections per group.
    /// </summary>
    public bool perGroupSelection = false;

    [SerializeField] private List<RadialMenuGroup> _groups = new List<RadialMenuGroup>();

    private RadialMenuGroup _hoveredGroup = null;
    private RadialMenuSection _hoveredSection = null;

    /// <summary>
    /// The root object of the menu. The menu will be generated inside this object.
    /// </summary>
    public GameObject root => _root;

    /// <summary>
    /// The group currently being hovered.
    /// </summary>
    public RadialMenuGroup hoveredGroup => _hoveredGroup;

    /// <summary>
    /// The section currently being hovered.
    /// </summary>
    public RadialMenuSection hoveredSection => _hoveredSection;

    [SerializeField, ReadOnly]private Vector2 _selectDirection = Vector2.zero;

    /// <summary>
    /// The direction of the selection. Set this value using input such as mouse delta or gamepad stick axis.
    /// </summary>
    public Vector2 selectDirection {
        get {
            return _selectDirection;
        }
        set {
            _hoveredGroup = GetGroup( value, Direction_Dead_Zone );
            RadialMenuSection lHoveredSection = GetSection( value, Direction_Dead_Zone );
            if( _hoveredSection != null && !selectOnHover && lHoveredSection != _hoveredSection ) {
                _hoveredSection.HoverExit();
            }
            _hoveredSection = lHoveredSection;
            if( _hoveredSection != null && !selectOnHover ) {
                _hoveredSection.HoverEnter();
            }

            if ( selectOnHover ) {
                SelectHoveredSection();
            }
            
            _selectDirection = value;
        }
    }

    private UnityEvent<string, string, bool> OnSelect = new UnityEvent<string, string, bool>();

    private void Start() {
        if( _buildOnStart ) {
            BuildMenu();
        }
    }

    /// <summary>
    /// Returns the selections of all groups in the menu.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, List<string>> GetSelections() {
        Dictionary<string, List<string>> lSelections = new Dictionary<string, List<string>>();
        foreach ( RadialMenuGroup lGroup in _groups ) {
            List<string> lSelected = new List<string>();
            foreach ( RadialMenuSection lSection in lGroup.selectedSections ) {
                lSelected.Add( lSection.sectionKey );
            }
            lSelections.Add( lGroup.groupKey, lSelected );
        }
        return lSelections;
    }

    /// <summary>
    /// Returns the selections of the first avalible group.
    /// Useful if the menu only has one group.
    /// </summary>
    /// <returns></returns>
    public List<string> GetSelection() {
        List<string> lSelected = new List<string>();

        RadialMenuGroup lGroup = _groups[0];
        if( lGroup == null ) {
            return lSelected;
        }

        foreach ( RadialMenuSection lSection in lGroup.selectedSections ) {
            lSelected.Add( lSection.sectionKey );
        }

        return lSelected;
    }

    /// <summary>
    /// Retruns the selections of a specified group by key.
    /// </summary>
    public List<string> GetSelection( string aGroupKey ) {
        List<string> lSelected = new List<string>();

        foreach ( RadialMenuGroup lGroup in _groups ) {
            if( lGroup.groupKey == aGroupKey ) {
                foreach ( RadialMenuSection lSection in lGroup.selectedSections ) {
                    lSelected.Add( lSection.sectionKey );
                }
                return lSelected;
            }    
        }

        return lSelected;
    }

    /// <summary>
    /// Builds/rebuilds the radial menu using current settings.
    /// </summary>
    public void BuildMenu() {
        //Clear children before populating
        foreach( Transform lChild in _root.transform ) {
            Destroy( lChild.gameObject );
        }

        float lRotationDeg = 0;
        foreach( RadialMenuGroup lGroup in _groups ) {

            OnSelect.AddListener( lGroup.OnSelectSection );

            lGroup.startDegree = lRotationDeg;
            foreach( RadialMenuSection lSection in lGroup.sections ) {
                RadialMenuSectionObject lNewSection = Instantiate( ( lGroup.sectionPrefabOverride != null ) ? lGroup.sectionPrefabOverride : _sectionPrefab, _root.transform.position, Quaternion.identity, _root.transform ).GetComponent<RadialMenuSectionObject>();
                //Image lSectionBackground = lNewSection.GetComponent<Image>();
                lNewSection.Initialize( lSection );
                lSection.obj = lNewSection;
                
                if( lSection.obj.backgroundImage != null ) {
                    lSection.obj.backgroundImage.fillMethod = Image.FillMethod.Radial360;
                    lSection.obj.backgroundImage.fillOrigin = 0;
                    lSection.startDegree = lRotationDeg;
                    lSection.obj.backgroundImage.fillAmount = ( 1 / 360f ) * ( lGroup.degrees / lGroup.sections.Count ) - lGroup.additionalFillClip;
                }
                
                lNewSection.transform.Rotate( new Vector3( 0, 0, -1 ), lRotationDeg + ( lGroup.additionalFillClip / 2f ) );

                if( lSection.repositionSprite && lSection.displaySprite != null ) {
                    //Image lImage = lSection.obj.transform.GetChild( 0 ).GetComponent<Image>();
                    if( lSection.obj.displayImage != null ) {
                        lSection.UpdateSprite();
                        lSection.obj.displayImage.transform.Rotate( new Vector3( 0, 0, -1 ), ( ( lGroup.degrees / lGroup.sections.Count ) / 2 ) + ( lGroup.additionalFillClip / 2f ) );
                    }
                }

                lRotationDeg +=  ( 1f * lGroup.degrees ) / lGroup.sections.Count;
                lSection.endDegree = lRotationDeg;

                if ( _sectionDividerPrefab != null ) {
                    Transform lNewSectionDivider = Instantiate( _sectionDividerPrefab, _root.transform.position, Quaternion.identity, _root.transform ).transform;
                    lNewSectionDivider.Rotate( new Vector3( 0, 0, -1 ), lRotationDeg );
                } 
            }
            lGroup.endDegree = lRotationDeg;

            if( _groupDividerPrefab != null ) {
                Transform lNewGroupDivider = Instantiate( _groupDividerPrefab, _root.transform.position, Quaternion.identity, _root.transform ).transform;
                lNewGroupDivider.Rotate( new Vector3( 0, 0, -1 ), lRotationDeg );
            }
        }
        
    }

    /// <summary>
    /// Finds and returns a RadialMenuSection via a group key and section key.
    /// </summary>
    public RadialMenuSection GetSection( string aGroupKey, string aSectionKey ) {
        foreach ( RadialMenuGroup lGroup in _groups ) {
            if ( lGroup.groupKey == aGroupKey ) {
                foreach ( RadialMenuSection lSection in lGroup.sections ) {
                    if ( lSection.sectionKey == aSectionKey ) {
                        return lSection;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a RadialMenuSection using a direction and deadzone.
    /// </summary>
    public RadialMenuSection GetSection( Vector2 aDirection, Vector2 aDeadZone ) {
        if (  Mathf.Abs( aDirection.x ) >= aDeadZone.x || Mathf.Abs( aDirection.y ) >= aDeadZone.y ) {
            float value = ( float )( ( Mathf.Atan2( -aDirection.x, -aDirection.y ) / Math.PI ) * 180f );
            if ( value < 0 ) value += 360f;

            foreach ( RadialMenuGroup lGroup in _groups ) {
                if ( lGroup.startDegree <= value && lGroup.endDegree >= value ) {
                    foreach ( RadialMenuSection lSection in lGroup.sections ) {
                        if ( lSection.startDegree <= value && lSection.endDegree >= value ) {
                            return lSection;
                        }
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Finds and returns a RadialMenuGroup by its key.
    /// </summary>
    /// <param name="aGroupKey"></param>
    /// <returns></returns>
    public RadialMenuGroup GetGroup( string aGroupKey ) {
        foreach ( RadialMenuGroup lGroup in _groups ) {
            if ( lGroup.groupKey == aGroupKey ) {
                return lGroup;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a RadialMenuGroup using a direction and dedzone.
    /// </summary>
    /// <param name="aDirection"></param>
    /// <param name="aDeadZone"></param>
    /// <returns></returns>
    public RadialMenuGroup GetGroup( Vector2 aDirection, Vector2 aDeadZone ) {

        if( Mathf.Abs( aDirection.x ) >= aDeadZone.x || Mathf.Abs( aDirection.y ) >= aDeadZone.y ) {
            float value = ( float )( ( Mathf.Atan2( -aDirection.x, -aDirection.y ) / Math.PI ) * 180f );
            if ( value < 0 ) value += 360f;

            foreach ( RadialMenuGroup lGroup in _groups ) {
                if ( lGroup.startDegree <= value && lGroup.endDegree >= value ) {
                    return lGroup;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Selects the hovered section
    /// </summary>
    public void SelectHoveredSection() {
        if( _hoveredSection != null ) {
            OnSelect?.Invoke( _hoveredGroup.groupKey, _hoveredSection.sectionKey, perGroupSelection );
        }
    }

    /// <summary>
    /// Selects a section via a group and section key.
    /// </summary>
    public void SelectSection( string aGroupKey, string aSectionKey ) {
        foreach( RadialMenuGroup lGroup in _groups ) {
            if( lGroup.groupKey == aGroupKey ) {
                foreach ( RadialMenuSection lSection in lGroup.sections ) {
                    if( lSection.sectionKey == aSectionKey ) {
                        OnSelect?.Invoke( lGroup.groupKey, lSection.sectionKey, perGroupSelection );
                        return;
                    }
                }
                Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to selection section \"{aSectionKey}\" of group \"{aGroupKey}\". Group with given key does not exist." );
                return;
            }
            
        }
        Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to selection section \"{aSectionKey}\" of group \"{aGroupKey}\". Group with given key does not exist." );
    }

    /// <summary>
    /// Adds a section to a group with the specified group key.
    /// </summary>
    public void AddSection( string aGroupKey, RadialMenuSection aSection ) {
        foreach ( RadialMenuGroup lGroup in _groups ) {
            if( lGroup.groupKey == aGroupKey ) {
                lGroup.AddSection( aSection );
                return;
            }
        }
        Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to add new section \"{aSection.sectionKey}\" to group \"{aGroupKey}\". Group with given key does not exist." );
    }

    /// <summary>
    /// Adds a section to a group with the specified group key at the specified index.
    /// </summary>
    public void AddSection( string aGroupKey, RadialMenuSection aSection, int aIndex ) {
        foreach ( RadialMenuGroup lGroup in _groups ) {
            if ( lGroup.groupKey == aGroupKey ) {
                lGroup.AddSection( aSection, aIndex );
                return;
            }
        }
        Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to add new section \"{aSection.sectionKey}\" to group \"{aGroupKey}\". Group with given key does not exist." );
    }

    /// <summary>
    /// Adds a group to the radial menu.
    /// </summary>
    public void AddGroup( RadialMenuGroup aGroup ) {
        foreach ( RadialMenuGroup lGroup in _groups ) {
            if ( lGroup.groupKey == aGroup.groupKey ) {
                 Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to add group \"{aGroup.groupKey}\". Group already with given key already exists." );
            }
        }
        _groups.Add( aGroup );
    }

    /// <summary>
    /// Adds a group to the radial menu at the specified index.
    /// </summary>
    public void AddGroup( RadialMenuGroup aGroup, int aIndex ) {
        foreach ( RadialMenuGroup lGroup in _groups ) {
            if ( lGroup.groupKey == aGroup.groupKey ) {
                Debug.LogError( $"[{typeof( RadialMenu )}] - Unable to add group \"{aGroup.groupKey}\". Group already with given key already exists." );
            }
        }
        _groups.Insert( aIndex, aGroup );
    }
}
