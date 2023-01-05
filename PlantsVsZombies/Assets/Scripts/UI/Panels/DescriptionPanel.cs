using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ֲ��������
/// </summary>
public class DescriptionPanel : BasePanel
{
    public void SetPlantName(string name)
    {
        GetControl<Text>("PlantName").text = name;
    }
    public void SetDescription(string description)
    {
        GetControl<Text>("Description").text = description;
    }
}
