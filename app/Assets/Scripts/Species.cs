using UnityEngine;

public class Species : MonoBehaviour
{
    public string speciesName;
    public string description;
    public string link;
    
    public void SetInfo(string _speciesName, string _desc, string _link)
    {
        speciesName = _speciesName;
        description = _desc;
        link = _link;
    }
}