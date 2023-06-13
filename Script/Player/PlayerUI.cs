using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] Image Aim;
    [SerializeField] Text Reloading;
    [SerializeField] Slider HpSlider;
    [SerializeField] Image GameOver;
    [SerializeField] Button ReStartBtn;

    [SerializeField] Slider TimeCheckUI;

    Player player;
    public Slider m_timeCheckUI => TimeCheckUI;

    public Button m_ReStartBtn => ReStartBtn;


    [Header("RayHitUI")]
    [SerializeField] GameObject ItemUIActive;
    [SerializeField] Text ItemName;

    bool callinvenUI = true;
    bool callEquipUI = true;
    public GameObject playerinvenUI;
    public GameObject equipUI;

    public void Callinven()
    {
        callinvenUI = !callinvenUI;
        playerinvenUI.SetActive(callinvenUI);
    }
    public void CallEquip()
    {
        callEquipUI = !callEquipUI;
        equipUI.SetActive(callEquipUI);
    }


    public void SetRayHitUI(bool IsOn,string name = "")
    {
        ItemUIActive.SetActive(IsOn);
        ItemName.text = name;
    }

    /// ������Ʈ���� ������ üũ�Ҷ� �ߴ� UI
    [Header("ActivateKeyUI")]
    [SerializeField] ActivateKeyUI activateKeyUI;

    /// <summary>
    /// ui �Ѱ� ����
    /// </summary>
    public void ActivateKeyUIActive(bool IsOn)
    {
        activateKeyUI.SetActivateKeyUI(IsOn);
    }

    /// <summary>
    /// ����� ����� ���� ���¸� ��Ÿ��
    /// </summary>
    public void ActivateKeyTextUI(string state)
    {
        activateKeyUI._activateKeyWordUI.text = state;
    }

    /// <summary>
    /// ���� ui�� ����
    /// </summary>
    public bool IsOnUI()
    {
        return activateKeyUI._IsActivateKeyUIOn;
    }

    public void IsUseItem(bool Use)
    {
        m_timeCheckUI.gameObject.SetActive(Use);
    }

    public void TimeCheck(float nowTime,float maxTime)
    {
        float percent = nowTime / maxTime;
        m_timeCheckUI.value = percent;
    }

    public void HPUI(float nowHP,float MaxHP)
    {
        float percent = nowHP / MaxHP;
        HpSlider.value = percent;
    }

    public void GameOverUI(bool IsTrue)
    {
        GameOver.gameObject.SetActive(IsTrue);
    }

    public void CaughtOnAim(string type)
    {
        if(type == "Item")
        {
            Aim.color = Color.gray;
        }
        else if(type == "Enemy")
        {
            Aim.color = Color.red;
        }
        else Aim.color = Color.green; 
    }

    public void ReloadingUI(bool IsOn)
    {
        Reloading.gameObject.SetActive(IsOn);
    }

}
