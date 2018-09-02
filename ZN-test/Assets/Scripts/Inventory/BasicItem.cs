using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicItem {
	#region properties

	private string name; // item name
	public UnityEvent OnNameChange = new UnityEvent();
	public string Name
	{
		get { return name;}
		set
		{ 
			name = value;
		    OnNameChange.Invoke();
		}
		
	}
#endregion
private ushort maxAmount; // number that represents the maximum amount of an item in one stack (like in Minecraft)
public UnityEvent OnMaxAmountChange = new UnityEvent();
public ushort MaxAmount
{
    get { return maxAmount; }
    set { maxAmount = value; OnMaxAmountChange.Invoke(); }
}
private ushort amount; // number that represents amount of an item in one stack
public UnityEvent OnAmountChange = new UnityEvent();
public ushort Amount
{
    get { return amount; }
    set { amount = value; OnAmountChange.Invoke(); }
}
private float durability; // number 0-1 that represents the durability
public UnityEvent OnDurabilityChange = new UnityEvent();
public float Durability
{
    get { return durability; }
    set { durability = value; OnDurabilityChange.Invoke(); }
}
}
