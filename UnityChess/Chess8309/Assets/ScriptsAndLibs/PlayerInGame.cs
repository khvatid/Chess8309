using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using TMPro;
using System.IO;
using System;

public class PlayerInGame : MonoBehaviour
{
    public TMP_Text player1;
    public TMP_Text player2;
    SqlConnectionStringBuilder builder;
    gameSetting chessDB;
    Profile profile;
    // Start is called before the first frame update
    
    void Start()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(gameSetting));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory + $"/dxmlp/chessSetting.xml", FileMode.OpenOrCreate))
        {
            chessDB = (gameSetting)serializer.Deserialize(fs);
        }
        serializer = new XmlSerializer(typeof(Profile));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory + $"/dxmlp/profile.xml", FileMode.OpenOrCreate))
        {
            profile = (Profile)serializer.Deserialize(fs);
        }
        builder = new SqlConnectionStringBuilder();
        builder.DataSource = "25.77.44.224";
        builder.UserID = "test";
        builder.Password = "qwep[]ghjB1";
        builder.InitialCatalog = "ChessDatabase";
    }

    void FixedUpdate()
    {
        using (SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString))
        {
            sqlConnection.Open();

            using (SqlCommand cmdSql = new SqlCommand("SELECT * FROM GameAllInfo WHERE ID_game =" + chessDB.id_game, sqlConnection))
            {
                using (SqlDataReader reader = cmdSql.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        player1.text = reader.GetString(1);
                        player2.text = reader.GetString(2);
                    }
                }
            }
        }
    }
}
