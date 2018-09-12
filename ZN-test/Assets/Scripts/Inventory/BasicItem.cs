using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// The class that holds all the defenitions + it do invokes

public class BasicItem
{   // Item names and event when said item changes
    #region properties: name

    private string name; // Item name
    public UnityEvent OnNameChange = new UnityEvent(); // Invoke event when something changes, makes new event
    public string Name // Getters and setters of the property trigger the event
    {
        get { return name; }
        set { name = value; OnNameChange.Invoke(); }
    }

    #endregion
    // Graphics for items
    #region properties: graphic

    private string graphic; // A string that holds the name of the sprite or model for the item
    public UnityEvent OnGraphicChange = new UnityEvent();
    public string Graphic
    {
        get { return graphic; }
        set { graphic = value; OnGraphicChange.Invoke(); }
    }

    #endregion
    // Max amount of item in stack
    #region properties: max amount

    private ushort maxAmount; // A number that holds or represents the maximum amount of an item in one stack
    public UnityEvent OnMaxAmountChange = new UnityEvent();
    public ushort MaxAmount
    {
        get { return maxAmount; }
        set { maxAmount = value; OnMaxAmountChange.Invoke(); }
    }

    #endregion
    // Amount of an item in a stack
    #region properties: amount

    private ushort amount; // A number that represents the amount of an item in one stack
    public UnityEvent OnAmountChange = new UnityEvent();
    public ushort Amount
    {
        get { return amount; }
        set { amount = value; OnAmountChange.Invoke(); }
    }

    #endregion
    // Durability of an item
    #region properties: durability

    private float durability; // A number from 0 to 1 that represents the durability of an item (note, these values will be displayed as precentages in game)
    public UnityEvent OnDurabilityChange = new UnityEvent();
    public float Durability
    {
        get { return durability; }
        set { durability = value; OnDurabilityChange.Invoke(); }
    }

    #endregion
    // Item constructors with and without parameters
    #region constructors

    // Basic constr. without parameters
    public BasicItem()
    {
        Amount = 1;
        MaxAmount = 64;
        Name = "Unnamed Item";
        Graphic = "Unknown";
        Durability = 1.0f;
    }

    // Main constr. with basic parameters
    public BasicItem(string itemName, string itemGraphic)
    {
        Amount = 1;
        MaxAmount = 64;
        Name = itemName;
        Graphic = itemGraphic;
    }

    #endregion
}

