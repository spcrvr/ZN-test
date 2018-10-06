using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {
	public Text NPC_count;
	private void Awake()
	{
		NPC_count.text = "0";
	}
	private void FixedUpdate()
	{
		UpdateNPC_count();
	}
	private void UpdateNPC_count()
	{
		NPC_count.text = "NPC_count :" + GameObject.FindGameObjectsWithTag("Walker_enemy").Length.ToString();
	}
}
