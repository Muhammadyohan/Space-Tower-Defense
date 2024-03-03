using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class setQuality : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown graphicsDropdown;

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, false);
    }
    
}
