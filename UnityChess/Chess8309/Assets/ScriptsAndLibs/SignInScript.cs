using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SignInScript : MonoBehaviour
{
    public InputField field;
    Profile player = new Profile()
    {
        id_player ="user0",
        point = 0
    };

    public void AcceptButton()
    {
        if (field.text != null)
        player.id_player = field.text;

        XmlSerializer serializer = new XmlSerializer(typeof(Profile));
        using (TextWriter writer = new StreamWriter(@"C:\Users\hv200\Desktop\programms\Profile.xml"))
        {
            serializer.Serialize(writer,player);
        }
        SceneManager.LoadSceneAsync("MainMenuScene");
        SceneManager.UnloadSceneAsync("SignInScene");
    }
}


public class Profile
{
    public string id_player { get; set; }
    public int point { get; set; }

}