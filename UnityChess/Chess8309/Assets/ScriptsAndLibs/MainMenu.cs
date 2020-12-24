using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

    private Profile player;
    public TMP_Text Text;
    public void Start()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Profile));
        using (TextReader reader = new StreamReader(@"C:\Users\hv200\Desktop\programms\Profile.xml"))
        {
            player = (Profile)serializer.Deserialize(reader);
        }
        Text.text = player.id_player;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GamePvPScence");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT COMPLITE");
        Application.Quit();
    }

    public void CloseFromGame()
    {
        SceneManager.LoadScene("MainMenuScene");
        SceneManager.UnloadSceneAsync(SceneManager.sceneCountInBuildSettings);
    }

    public void OpenAccount()
    {
        SceneManager.LoadScene("SignInScene");
    }
   
}
