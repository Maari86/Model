using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject mainQuitButton;
    [SerializeField] private GameObject weaponCloseButton;
    [SerializeField] private GameObject swordsButton;
    [SerializeField] private GameObject swordPage;

    public void swordPageClicked()
    {
        swordPage.SetActive(true);
        weaponCloseButton.SetActive(true);
        mainPage.SetActive(false);
        swordsButton.SetActive(false);
        mainQuitButton.SetActive(false);
    }

    public void closeShopPage()
    {
        swordPage.SetActive(false);
        weaponCloseButton.SetActive(false);
        mainPage.SetActive(true);
        swordsButton.SetActive(true);
        mainQuitButton.SetActive(true);
    }
}
