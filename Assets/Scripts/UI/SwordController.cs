using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SwordController : MonoBehaviour
{
    public GameObject[] swordOptions;
    public int selectedSwordIndex;
    private GameObject selectedSword;

    private void Awake()
    {
        selectedSwordIndex = PlayerPrefs.GetInt("SelectedSword", 0);
        SelectSwordRange(selectedSwordIndex);

        Time.timeScale = 1;
    }

    public GameObject GetSelectedSword()
    {
        return selectedSword;
    }

    public void SetSelectedSwordIndex(int index)
    {
        selectedSwordIndex = index;
        SelectSwordRange(selectedSwordIndex);
    }

    public void SelectSwordRange(int buttonIndex)
    {
        int startIndex = buttonIndex * 1;
        int endIndex = startIndex + 1;

        for (int i = 0; i < swordOptions.Length; i++)
        {
            if (i >= startIndex && i <= endIndex)
            {
                swordOptions[i].SetActive(true);
            }
            else
            {
                swordOptions[i].SetActive(false);
            }
        }

        selectedSword = swordOptions[selectedSwordIndex];
    }
}
