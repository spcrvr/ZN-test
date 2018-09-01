using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatePattern
{
	public class Walker : Enemy
	{
		protected Transform enemyObj;
		protected enum EnemyFSM
		{
			Attack,
			Idle,
			Chase
		}
		private void Start()
		{

		}
		public virtual void UpdateEnemy(Transform playerObj)
		{

		}

		 protected void DoAction(Transform playerObj, EnemyFSM enemyMode)
        {
            float strollSpeed = 1f;
            float attackSpeed = 5f;

            switch (enemyMode)
            {
                case EnemyFSM.Attack:
                    //Attack player
                    break;
                case EnemyFSM.Idle:
                    //Perform random idle movement
                    break;
                case EnemyFSM.Chase:
                    //Follow closest player
                    break;
            }
        }
	}


}