using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;
using System;
using TMPro;

public class MainMenu : MonoBehaviour
{

    private Profile player;
    private gameSetting chessST;
    public TMP_Text Text;
    SqlConnectionStringBuilder builder;
    public void Start()
    {
        chessST = new gameSetting();
        XmlSerializer serializer = new XmlSerializer(typeof(Profile));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory + $"/dxmlp/profile.xml", FileMode.OpenOrCreate))
        {
            player = (Profile)serializer.Deserialize(fs);
        }
        builder = new SqlConnectionStringBuilder
        {
            DataSource = "25.77.44.224",
            UserID = "test",
            Password = "qwep[]ghjB1",
            InitialCatalog = "ChessDatabase"
        };
        Text.text = player.namePlayer;
    }

    public void PlayGame()
    {
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString))
            {
                sqlConnection.Open();
                Debug.Log("db connect");
                //Изначально ищем игру со статусом wait
                using (SqlCommand cmdSql = new SqlCommand(" SELECT ID_game, status  FROM Games WHERE status = 'wait' "
                    , sqlConnection))
                {
                    Debug.Log("wait search");
                    bool searchGame = false;
                    using (SqlDataReader reader = cmdSql.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Debug.Log("ID_game find");
                            chessST.id_game = reader.GetInt32(0);
                            searchGame = true;
                        }
                    }
                    if (searchGame == true)
                    {
                        cmdSql.CommandText = "SELECT Game_ID, Color FROM Sides WHERE Game_ID = " + chessST.id_game;
                        cmdSql.Connection = sqlConnection;
                        Debug.Log("color find");
                        using (SqlDataReader reader = cmdSql.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Debug.Log("color find true ");
                                chessST.color = reader.GetString(1) == "white"? "black": "white";
                                Debug.Log(chessST.color);
                            }
                        }
                        cmdSql.CommandText = "INSERT INTO Sides(Game_ID,Player_ID,Color, Result) " +
                            "VALUES(" + chessST.id_game + ",'" + player.emailPlayer + "','" + chessST.color + "',0)";
                        cmdSql.Connection = sqlConnection;
                        cmdSql.ExecuteNonQuery();
                        Debug.Log("Insert second player");
                        cmdSql.CommandText = "UPDATE Games SET status = 'play' WHERE ID_game =" + chessST.id_game + ";";
                        cmdSql.Connection = sqlConnection;
                        cmdSql.ExecuteNonQuery();
                        Debug.Log("update status");
                    }
                    else
                    {
                        cmdSql.CommandText = "INSERT INTO Games DEFAULT VALUES";
                        cmdSql.Connection = sqlConnection;
                        cmdSql.ExecuteNonQuery();
                        cmdSql.CommandText = "SELECT ID_game, status  FROM Games WHERE status = 'wait'";
                        cmdSql.Connection = sqlConnection;
                        using (SqlDataReader reader = cmdSql.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Debug.Log("ID_game find");
                                chessST.id_game = reader.GetInt32(0);
                                chessST.color = chessST.id_game % 2 == 0 ? "white" : "black" ;
                            }
                        }
                        cmdSql.CommandText = "INSERT INTO Sides(Game_ID,Player_ID,Color, Result) " +
                        "VALUES(" + chessST.id_game + ",'" + player.emailPlayer + "','" +
                        chessST.color + "',0)";
                        cmdSql.Connection = sqlConnection;
                        cmdSql.ExecuteNonQuery();
                       // cmdSql.CommandText = "SELECT * FROM Games WHERE ID_game = " + ID_Game;
                       // SqlDependency dependency = new SqlDependency(cmdSql);
                       // cmdSql.Connection = sqlConnection;
                       // dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);
                        //cmdSql.CommandText = " UPDATE Games SET status = 'play' WHERE status = 'wait'";
                        //cmdSql.Connection = sqlConnection;
                        //cmdSql.ExecuteNonQuery();
                    }

                }

            }
        }
        catch (SqlException e)
        {
            Debug.Log(e.ToString());
        }
        XmlSerializer serializer = new XmlSerializer(typeof(gameSetting));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory +
            $"/dxmlp/chessSetting.xml", FileMode.Create))
        {
            serializer.Serialize(fs, chessST);
        }
        SceneManager.LoadSceneAsync("GamePvPscence", LoadSceneMode.Single);
    }

    public void PlayOffline()
    {
        SceneManager.LoadSceneAsync("GamePvEscence", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT COMPLITE");
        Application.Quit();
    }
    public void OpenAccount()
    {
        SceneManager.LoadScene("SignInScene", LoadSceneMode.Single);
    }

    public void ProfileButton()
    {
        SceneManager.LoadSceneAsync("ProfileInfo", LoadSceneMode.Single);
    }
   
  //  void OnDependencyChange(object sender, SqlNotificationEventArgs e)
  //  {
   //     load ++;
   //     Debug.Log(e);
        
   // }


}
