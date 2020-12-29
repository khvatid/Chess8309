using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;
using System;
using TMPro;

public class MainMenu : MonoBehaviour
{

    private Profile player;
    public TMP_Text Text;
    public void Start()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Profile));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory + $"/dxmlp/profile.xml", FileMode.OpenOrCreate))
        {
            player = (Profile)serializer.Deserialize(fs);
        }
        Text.text = player.namePlayer;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GamePvPScence",LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT COMPLITE");
        Application.Quit();
    }

    public void CloseFromGame()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }

    public void OpenAccount()
    {
        SceneManager.LoadScene("SignInScene");
    }
   
}
