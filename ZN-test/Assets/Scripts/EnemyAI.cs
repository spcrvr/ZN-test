using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
	public class EnemyAI : MonoBehaviour
	{
		public GameObject playerObj;
		public GameObject walkerObj;
		List<Enemy> enemies = new List<Enemy>();

		void Start()
		{
			enemies.Add(new Walker(walkerObj.transform));

		}

		void FixedUpdate()
		{
			for(int i=0;i<enemies.Count;i++)
			{
				enemies[i].UpdateEnemy(playerObj.transform);
			}

		}
	}


}