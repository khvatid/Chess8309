using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Data;
using System.Data.SqlClient;
public class SignInScript : MonoBehaviour
{
    public InputField email;
    public InputField pass;


    SqlConnectionStringBuilder builder;
    void Start()
    {
        builder = new SqlConnectionStringBuilder();
        builder.DataSource = "192.168.31.180";
        builder.UserID = "test";
        builder.Password = "qwep[]ghjB1";
        builder.InitialCatalog = "ChessDatabase";
    }

    public void ButtonAccept()
    {
        try
        {
            using ( SqlConnection Sql = new SqlConnection(builder.ConnectionString))
            {
                Sql.Open();
                Debug.Log("db connect");
           
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PlayerProfile " +
                      "WHERE passwordPlayer = '" + pass.text + "' AND emailPlayer = '" + email.text + "'", Sql))
                {
                    Debug.Log("query complite");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Profile user = new Profile();
                            user.emailPlayer = reader.GetString(0);
                            user.namePlayer = reader.GetString(1);
                            user.passwordPlayer = reader.GetString(2);
                            user.victory = reader.GetInt32(3);
                            user.defeat = reader.GetInt32(4);
                            Debug.Log("ProfileAccept");
                            XmlSerializer serializer = new XmlSerializer(typeof(Profile));
                            using (FileStream fs = new FileStream(Environment.CurrentDirectory +
                                $"/dxmlp/profile.xml", FileMode.OpenOrCreate))
                            {
                                serializer.Serialize(fs, user);
                            }
                            SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Single);
                        }
                    }
                }
            }
        }
        catch(SqlException e)
        {
            Debug.Log(e.ToString());
        }
        


    }
}

public class Profile
{
    public string emailPlayer { get; set; }
    public string namePlayer { get; set; }
    public string passwordPlayer { get; set; }

    public int victory { get; set; }
    public int defeat { get; set; }
    public Profile()
    {
        emailPlayer = "123@gmail.com ";
        namePlayer = "Ghost";
        passwordPlayer = "123";
        victory = 0;
        defeat = 0;
    }
    
    public override string ToString()
    {
        return "Name: " + namePlayer;
    }
}

