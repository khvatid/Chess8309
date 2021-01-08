using System.Collections;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.UI;
using ChessLib;
using System.Xml.Serialization;
using System.IO;

public class PvEScript : MonoBehaviour
{
   DragAndDrop drAdr;
   ChessClass chess;
   gameSetting chessDB;
   Profile player;
   public Text text;
   public PvEScript()
   {
       drAdr = new DragAndDrop();
        chess = new ChessClass();
   }

    void Start()
    {
        ShowFigures();
        XmlSerializer serializer = new XmlSerializer(typeof(gameSetting));
        serializer = new XmlSerializer(typeof(Profile));
        using (FileStream fs = new FileStream(Environment.CurrentDirectory + $"/dxmlp/profile.xml", FileMode.OpenOrCreate))
        {
            player = (Profile)serializer.Deserialize(fs);
        }
        chessDB = new gameSetting();
        chessDB.color = "white";
        chessDB.id_game = 0;
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
            chess = chess.Move(move);
            ShowFigures();
        }
    }

    private void FixedUpdate()
    {
        if( chess.GetColor() != chessDB.color)
        {

        }
        if (chess.IsCheckAfter() && chess.IsCheck())
        {

            text.text = "Game Over!";
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

    public void LOSEBUTTON()
    {
        text.text = "Game Over!";
    }

    public void NPCmove()
    {
        ChessClass clone = new ChessClass(chess.fen);
        List<string> list = clone.GetAllMoves();
        System.Random random = new System.Random();
        string move = list[random.Next(0, list.Count)];
        Debug.Log(move);
        chess = chess.Move(move);
        ShowFigures();
    }

    public void CloseGame()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenuScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
