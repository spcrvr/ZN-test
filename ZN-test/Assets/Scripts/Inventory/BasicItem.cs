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
}
