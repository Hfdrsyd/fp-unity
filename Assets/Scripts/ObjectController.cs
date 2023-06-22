using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    [SerializeField] string itemName;
    [TextArea] [SerializeField] string itemExtraInfo;
    [SerializeField] InspectController inspectController;

    public void ShowObjectName()
    {
        inspectController.ShowName(itemName);
    }

    public void HideObjectName()
    {
        inspectController.HideName();
    }

    public void ShowExtraInfo()
    {
        inspectController.ShowAdditionalInfo(itemExtraInfo);
    }

}
