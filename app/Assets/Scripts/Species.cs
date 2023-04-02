using UnityEngine;

public class Species : MonoBehaviour
{
    public string speciesName;
    public string speciesNameCapitalized;
    public string description;
    public string link;
    public string binomial;
    public float focusDistance;
    public bool isLiked;
    
    public void SetInfo(string _speciesName, string _binomial, string _desc, string _link, float _focusDistance, bool _isLiked)
    {
        speciesName = _speciesName;
        speciesNameCapitalized = char.ToUpper(_speciesName[0]) + _speciesName.Substring(1);
        description = _desc;
        link = _link;
        binomial = _binomial;
        focusDistance = _focusDistance;
        isLiked = _isLiked;
    }
}