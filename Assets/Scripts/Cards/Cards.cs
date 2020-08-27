using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards
{
    //Hold all the data about each card


    public string ID;
    public string Label;
    public string Name;
    public string Category;
    public string Type;
    public string Text;
    public string Front_Image;
    public string Back_Image;
    public string Use_Count;
    public Texture2D frontImg;
    public Texture2D backImg;
    // general constructor
    public Cards()
    {
    }

    //Constructor for cards from CSV. Used to load them in.
    public Cards(string[] row)
    {
        ID = row[0];
        Label = row[1];
        Name = row[2];
        Category = row[3];
        Type = row[4];
        Text = row[5];
        Front_Image = row[6];
        Back_Image = row[7];
        Use_Count = row[8];
    }

}
