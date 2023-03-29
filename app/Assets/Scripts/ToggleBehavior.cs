using UnityEngine;
using UnityEngine.UI;

public class ToggleBehavior : MonoBehaviour
{
    private Toggle toggle;
    private bool isSelected = false;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isOn)
    {
        isSelected = isOn;
        toggle.isOn = isSelected;
    }
}
