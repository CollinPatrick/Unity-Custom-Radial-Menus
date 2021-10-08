using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuSectionObject : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _displayImage;
    [SerializeField] private GameObject _selectedOverlay;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private GameObject _hoverOverlay;
    [SerializeField] private Color _hoverColor;

    private RadialMenu.RadialMenuSection _radialMenuSection;
    private Color _idleColor;

    /// <summary>
    /// The background image of this section. The sprite radial fill will be applied to this image.
    /// </summary>
    public Image backgroundImage => _backgroundImage;

    /// <summary>
    /// The icon inside this section. The sprite will automatically be set and centered inside the section.
    /// </summary>
    public Image displayImage => _displayImage;

    public void Initialize( RadialMenu.RadialMenuSection aSection ) {
        _idleColor = _backgroundImage.color;
        _radialMenuSection = aSection;
    }

    public void OnHoverEnter() {
        if ( _selectedOverlay != null ) {
            _selectedOverlay.SetActive( true );
        }

        if ( _hoverOverlay != null ) {
            _hoverOverlay.SetActive( false );
        }

        _backgroundImage.color = _hoverColor;
    }

    public void OnHoverExit() {
        //Disable hover overlay
        if ( _hoverOverlay != null ) {
            _hoverOverlay.SetActive( false );
        }

        //Re-enable selected overlay if already selected
        if ( _selectedOverlay != null && _radialMenuSection.selected ) {
            _selectedOverlay.SetActive( true );
        }

        //Set selected color if already selected
        if( _radialMenuSection.selected ) {
            _backgroundImage.color = _selectedColor;
        }
        //Set idle color if not already selected
        else {
            _backgroundImage.color = _idleColor;
        }
    }

    public void OnSelect() {
        //Diable hover overlay
        if( _hoverOverlay != null ) {
            _hoverOverlay.SetActive( false );
        }

        //Enable selectOverlay
        if( _selectedOverlay != null ) {
            _selectedOverlay.SetActive( true );
        }

        //Set to selected color
        _backgroundImage.color = _selectedColor;
    }

    public void OnDeselect() {
        //Disable Hover overlay
        if ( _hoverOverlay != null ) {
            _hoverOverlay.SetActive( false );
        }

        //Diable select Overlay
        if ( _selectedOverlay != null ) {
            _selectedOverlay.SetActive( true );
        }

        //Set to idle color
        _backgroundImage.color = _idleColor;
    }
}
