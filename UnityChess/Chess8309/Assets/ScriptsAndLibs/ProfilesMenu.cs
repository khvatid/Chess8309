using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Data;
using System.Data.SqlClient;
using TMPro;

public class ProfilesMenu : MonoBehaviour
{

    public TMP_Text email;
    public TMP_Text idtext;
    public TMP_Text win;
    public TMP_Text lose;
    public TMP_Text top;
    Profile profile;
    SqlConnectionStringBuilder builder;
    // Start is called before the first frame update
    void Start()
    {
        builder = new SqlConnectionStringBuilder();
        builder.DataSource = "25.77.44.224";
        builder.UserID = "test";
        builder.Password = "qwep[]ghjB1";
        builder.InitialCatalog = "ChessDatabase";
        XmlSerializer serializer = new XmlSerializer(typeof(Profile));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory + $"/dxmlp/profile.xml", FileMode.OpenOrCreate))
        {
            profile = (Profile)serializer.Deserialize(fs);
        }
        email.text = profile.emailPlayer;
        idtext.text = profile.namePlayer;
        win.text +=" "+ profile.victory.ToString();
        lose.text +=" "+ profile.defeat.ToString();
    }
    public void TopPlayers()
    {
        
        using(SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString))
        {
            sqlConnection.Open();
            using (SqlCommand cmdSql = new SqlCommand("SELECT namePlayer, victory FROM PlayerProfile ORDER BY victory", sqlConnection))
            { 
                using(SqlDataReader dataReader = cmdSql.ExecuteReader())
                {
                    for(int i = 0; dataReader.Read();i++)
                    {
                        top.text += i +". " + dataReader.GetString(0) +" - "+ dataReader.GetInt32(1)+"\n";
                    }
                }
            }
        }
    }

    public void BackButton()
    {
        SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);
    }

}
