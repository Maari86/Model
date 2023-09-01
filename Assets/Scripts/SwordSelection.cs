using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SwordSelection : MonoBehaviour
{
    public GameObject[] swordSkins;
    public int[] buttonIndices; 
    public int selectedSword;

    private void Awake()
    {
        selectedSword = PlayerPrefs.GetInt("SelectedSword", 0);

        // Enable the first two game objects, disable the rest
        for (int i = 0; i < swordSkins.Length; i++)
        {
            if (i < 2)
            {
                swordSkins[i].SetActive(true);
            }
            else
            {
                swordSkins[i].SetActive(false);
            }
        }
    }



    public void SelectSwords(int buttonIndex)
    {
        // Calculate the start and end indices based on the buttonIndex
        int startIndex = buttonIndices[buttonIndex * 2];
        int endIndex = buttonIndices[buttonIndex * 2 + 1];

        // Deactivate previously selected sword
        swordSkins[selectedSword].SetActive(false);

        // Activate the selected swords and deactivate the rest
        for (int i = 0; i < swordSkins.Length; i++)
        {
            if (i >= startIndex && i <= endIndex)
            {
                swordSkins[i].SetActive(true);
            }
            else
            {
                swordSkins[i].SetActive(false);
            }
        }

        // Update the selected sword index
        selectedSword = startIndex;
        PlayerPrefs.SetInt("SelectedSword", selectedSword);
    }
  
}