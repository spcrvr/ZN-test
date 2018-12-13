using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {
	[SerializeField] private float _hp;

	public void SetHP(float value){
		 _hp = value;
	}

	public void DecreaseHP(float value){
		if(_hp != 0) { _hp -= value;}
	}

	public float GetHP(){
		return _hp;
	}
}
