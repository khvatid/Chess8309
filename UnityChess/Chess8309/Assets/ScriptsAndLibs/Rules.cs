using System.Collections;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.UI;
using ChessLib;
using System.Xml.Serialization;
using System.IO;

public class Rules : MonoBehaviour
{
   DragAndDrop drAdr;
   ChessClass chess;
    gameSetting chessDB;
   SqlConnectionStringBuilder builder;
   public Text text;
    public Text colorTXT;
   public Rules()
   {
       drAdr = new DragAndDrop();
        chess = new ChessClass();
   }

    void Start()
    {
        ShowFigures();
        XmlSerializer serializer = new XmlSerializer(typeof(gameSetting));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory + $"/dxmlp/chessSetting.xml", FileMode.OpenOrCreate))
        {
            chessDB = (gameSetting)serializer.Deserialize(fs);
        }
        builder = new SqlConnectionStringBuilder
        {
            DataSource = "192.168.31.180",
            UserID = "test",
            Password = "qwep[]ghjB1",
            InitialCatalog = "ChessDatabase"
        };
        colorTXT.text = chessDB.color;
    }


    void Update()
    {
        if(drAdr.Action())
        {
            string from = GetSquare(drAdr.pickPosition);
            string to = GetSquare(drAdr.dropPosition) ;
            string figure = chess.GetFigureAt((int)(drAdr.pickPosition.x / 2.0),
                (int)(drAdr.pickPosition.y / 2.0)).ToString();
            string move = figure + from + to;
            Debug.Log(move);
            if( chessDB.color == chess.GetColor())
            {
                chess = chess.Move(move);
                using (SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString))
                {
                    sqlConnection.Open();
                    Debug.Log("db connect");
                    using (SqlCommand cmdSql = new SqlCommand
                    ("UPDATE Games SET Fen = '" + chess.fen + "' WHERE ID_game = " + chessDB.id_game, sqlConnection))
                    {
                        cmdSql.ExecuteNonQuery();
                    }
                }
            }             
            ShowFigures();
        }
        
        
    }

    private void FixedUpdate()
    {
        if( chess.GetColor() != chessDB.color)
        {
            using (SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString))
            {
                sqlConnection.Open();
                Debug.Log("db connect");
                using (SqlCommand cmdSql = new SqlCommand
                ("SELECT ID_game ,Fen FROM Games WHERE ID_game =" + chessDB.id_game, sqlConnection))
                {
                    using (SqlDataReader reader = cmdSql.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            chess = new ChessClass(reader.GetString(1));
                            ShowFigures();
                        }
                    }
                }
            }
        }
        if (chess.IsCheckAfter())
        {
            text.text = "КОНЕЦ ИГРЫ";
            Debug.Log("CHECK");
        }

    }

    string GetSquare (Vector2 position)
    {
        int x = Convert.ToInt32(position.x / 2.0);
        int y = Convert.ToInt32(position.y / 2.0);
        return ((char)('a' + x)).ToString() + (y + 1).ToString();
    }

    void ShowFigures()
    {
        int nr = 0;
        for(int y = 0; y<8; y++)
            for( int x = 0; x<8; x++)
            {
                string figure = chess.GetFigureAt(x, y).ToString();
                if (figure == "1") continue;
                PlaceFigure("box"+ nr,figure, x, y);
                nr++;
            }
        for (; nr < 32; nr++)
            PlaceFigure("box" + nr, "q", 9, 9);
    }

    void PlaceFigure(string box, string figure, int x, int y)
    {
       // Debug.Log(box + " " + figure + " " + x + y);
        GameObject goBox = GameObject.Find(box);
        GameObject goFigure = GameObject.Find(figure);
        GameObject goSquare = GameObject.Find("" + y + x);

        var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        var spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;

        goBox.transform.position = goSquare.transform.position;

    }
}


class DragAndDrop
{
    enum State
    {
        none,
        drag
    }

    public Vector2 pickPosition { get; private set; }
    public Vector2 dropPosition { get; private set; }

    State state;
    GameObject item;
    Vector2 offSet;

    public DragAndDrop()
    {
        state = State.none;
        item = null;
    }

    public bool Action()
    {
        switch(state)
        {
            case State.none:

            if(IsMouseButtonPressed())
            PickUp();

            break;
            case State.drag:
             if(IsMouseButtonPressed())
                    Drag();
             else
             {
                Drop();
                return true;
             }

            break;

        }
        return false;
    }

    bool IsMouseButtonPressed()
    {
        return Input.GetMouseButton(0);
    }

    void PickUp()
    {
      
        Vector2 clickPosition = GetClickPosition();
        Transform clickedItem = GetItemAt(clickPosition);
        if (clickedItem == null) return;
        pickPosition = clickedItem.position;
        item = clickedItem.gameObject;
        state = State.drag;
        offSet = pickPosition - clickPosition;
        Debug.Log("picked up!" + item.name);
    }

    Vector2 GetClickPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    Transform GetItemAt(Vector2 position)
    {
       RaycastHit2D[] figures =  Physics2D.RaycastAll(position, position, 0.5f);
        if (figures.Length == 0)
            return null;
        return figures[0].transform;
    }

    void Drag()
    {
        item.transform.position = GetClickPosition() + offSet ;
    }

    void Drop()
    {
        dropPosition = item.transform.position;
        state = State.none;
        item = null;
    }

}