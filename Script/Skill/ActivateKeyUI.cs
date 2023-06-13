using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateKeyUI : MonoBehaviour
{
    [SerializeField] GameObject activateKeyUI;
    [SerializeField] Text activateKeyWordUI;

    public Text _activateKeyWordUI => activateKeyWordUI;

    public bool _IsActivateKeyUIOn { get; private set; }

    public void SetActivateKeyUI(bool IsOn)
    {
        _IsActivateKeyUIOn = IsOn;
        activateKeyUI.SetActive(IsOn);
    }
}
