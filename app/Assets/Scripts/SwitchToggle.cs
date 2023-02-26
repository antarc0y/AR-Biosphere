using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;

/// <summary>
/// Represents a switch toggle object that is clickable
/// switch toggle will handle changing the surface for the AR environment
/// we will only use Land or Water surfaces at this stage for simplicity
/// </summary>
public class SwitchToggle : MonoBehaviour {
   [SerializeField] RectTransform uiHandleRectTransform ;
   [SerializeField] Color backgroundActiveColor ;
   [SerializeField] Color handleActiveColor ;
   [SerializeField] Surface surface;

   Image backgroundImage, handleImage ;

   Color backgroundDefaultColor, handleDefaultColor ;

   Toggle toggle ;

   Vector2 handlePosition ;
   
    /// <summary>
    /// handles initializing the toggle button
    /// sets relevant information including anchor position, image components, colors
    /// </summary>
   void Awake ( ) {
      toggle = GetComponent <Toggle> ( ) ;

      handlePosition = uiHandleRectTransform.anchoredPosition ;

      backgroundImage = uiHandleRectTransform.parent.GetComponent <Image> ( ) ;
      handleImage = uiHandleRectTransform.GetComponent <Image> ( ) ;

      backgroundDefaultColor = backgroundImage.color ;
      handleDefaultColor = handleImage.color ;

      toggle.onValueChanged.AddListener (OnSwitch) ;

      if (toggle.isOn)
         OnSwitch (true) ;
   }

    /// <summary>
    ///  handles onClick effects for the toggle switch
    /// </summary>
    /// <param name="on"></param>
   void OnSwitch (bool on) {
    
      // changes the position of the toggle button to create the sliding animation
      uiHandleRectTransform.DOAnchorPos (on ? handlePosition * -1 : handlePosition, .4f).SetEase (Ease.InOutBack) ;

      // changes switch background color upon clicking
      backgroundImage.DOColor (on ? backgroundActiveColor : backgroundDefaultColor, .6f) ;

      // changes toggle button color upon clicking
      handleImage.DOColor (on ? handleActiveColor : handleDefaultColor, .4f) ;

      // change surface every time the user clicks the toggle, defaults to land
      surface.ChangeSurface();
   }

   void OnDestroy ( ) {
      toggle.onValueChanged.RemoveListener (OnSwitch) ;
   }
}