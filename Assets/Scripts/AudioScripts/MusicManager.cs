using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;

public class MusicManager : MonoBehaviour
{
    public SelectedSongData selectedSongData;
    public string fileDirectory;
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public TextMeshProUGUI noMusicText;
    public TextMeshProUGUI testText;

    private AudioSource audioSource;
    private AudioClip audioClip;

    public List<string> files = new List<string>();
    public List<string> directories = new List<string>();
    public List<AudioClip> clips = new List<AudioClip>();
    public Button quitButton;

    void Start()
    {
        Time.timeScale = 1f;

        audioSource = GetComponent<AudioSource>();
        GetAudiofromDirectories(fileDirectory);
    }

    public void GetAudiofromDirectories(string directory)
    {
        Debug.Log("directory: " + directory);
        //testText.text = "directory: " + directory;
        string[] allDirectories = new string[10];
        try
        {
            allDirectories = Directory.GetDirectories(directory);
            testText.text = "allDirectories: " + allDirectories.Length;
            Debug.Log("allDirectories: " + allDirectories.Length);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("Directory not found excetption: " + e);
            testText.text = "Directory not found excetption:" + e;
            noMusicText.text = "Directory not found excetption: " + e;
            return;
        }
        catch(Exception e)
        {
            testText.text = "Allfiles: " + allDirectories.Length + " exception: " + e;
            Debug.Log("Exception " + e);
        }
        for (int i = 0; i < allDirectories.Length; i++)
        {
            try
            {
                if (!Directory.Exists(allDirectories[i]))
                {
                    Debug.Log("Doesn't exist: " + allDirectories[i]);
                    continue;
                }
                if (allDirectories[i].IndexOf("Music", StringComparison.OrdinalIgnoreCase) > 0 || allDirectories[i].IndexOf("Download", StringComparison.OrdinalIgnoreCase) > 0 || allDirectories[i].IndexOf("Record", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    GetAudioFilesFrom(allDirectories[i]);
                }
                else
                {
                    //GetAudiofromDirectories(allDirectories[i]);
                }
            }
            catch(UnauthorizedAccessException e)
            {
                Debug.Log("unauthorized exception: " + e);
                continue;
            }
            catch(Exception e)
            {
                Debug.Log("Exception: " + e);
            }
        }

        CreateButtons();

        if (files.Count == 0)
        {
            noMusicText.gameObject.SetActive(true); // Enable the TextMeshPro object
        }
        else
        {
            noMusicText.gameObject.SetActive(false); // Disable the TextMeshPro object
        }
    }

    public void GetAudioFilesFrom(string directory)
    {
        Debug.Log("files from directory: " + directory);
        //testText.text = "files from directory: " + directory;
        string[] allFiles = new string[10];
        string[] allDirectories;
        try
        {
            allFiles = Directory.GetFiles(directory);
            allDirectories = Directory.GetDirectories(directory);

            testText.text = "Allfiles: " + allFiles.Length;
            Debug.Log("Allfiles: " + allFiles.Length);
        }
        catch(Exception e)
        {
            testText.text = "Allfiles: "+allFiles.Length+" exception: " + e;
            Debug.Log("Exception " + e);
            return;
        }
        for (int i = 0; i < allFiles.Length; i++)
        {
            try
            {
                if (allFiles[i].EndsWith(".wav"))
                {
                    files.Add(allFiles[i]);
                    clips.Add(new WWW("file://" + allFiles[i]).GetAudioClip(false, true, AudioType.WAV));
                }
                else if (allFiles[i].EndsWith(".mp3"))
                {
                    files.Add(allFiles[i]);
                    clips.Add(new WWW("file://" + allFiles[i]).GetAudioClip(false, true, AudioType.MPEG));
                }
            }
            catch(UnauthorizedAccessException e)
            {
                Debug.Log("unauthorized exception: " + e);
                continue;
            }
            catch(Exception e)
            {
                Debug.Log("Exception: " + e);
            }
        }
        for (int i = 0; i < allDirectories.Length; i++)
        {
            GetAudioFilesFrom(allDirectories[i]);
        }
    }

    public void CreateButtons()
    {
        Color[] buttonColors = new Color[] { Color.white, new Color(0.831f, 0.533f, 0.459f), new Color(0f, 0.660f, 0.272f) }; // Add more colors as needed

        for (int i = 0; i < files.Count; i++)
        {
            string fileName = Path.GetFileName(files[i]);

            GameObject buttonGO = Instantiate(buttonPrefab, buttonContainer);
            Button button = buttonGO.GetComponent<Button>();
            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            buttonText.text = fileName;

            Image buttonImage = buttonGO.GetComponent<Image>();
            buttonImage.color = buttonColors[i % buttonColors.Length]; 

            int index = i; // Capture the index in a local variable for the button click event
            button.onClick.AddListener(() => { 
                PlaySong(index);
                selectedSongData.songName = fileName;
                selectedSongData.songLocation = files[index];
                selectedSongData.songFormat = fileName.EndsWith(".wav") ? AudioType.WAV : fileName.EndsWith(".mp3") ? AudioType.MPEG : AudioType.UNKNOWN;
            }) ;

            if(index == 0)
            {
                button.onClick.Invoke();
            }
        }
    }


    public void PlaySong(int index)
    {
        if (index >= 0 && index < clips.Count)
        {
            audioClip = clips[index];

            // Check if the requested clip is already playing
            if (audioSource.clip == audioClip && audioSource.isPlaying)
            {
                return;
            }

            audioSource.clip = audioClip;
            audioSource.Play();

            // Store the selected song in the SelectedSongData scriptable object
            //selectedSongData.selectedSong = audioClip;
        }
    }
    public AudioClip GetSelectedSong()
    {
        if (audioSource.isPlaying)
        {
            return audioSource.clip;
        }
        else
        {
            return null;
        }
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
