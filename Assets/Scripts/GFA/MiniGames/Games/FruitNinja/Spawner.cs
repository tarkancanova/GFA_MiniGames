using System;
using System.Collections;
using System.Collections.Generic;
using GFA.MiniGames.Data;
using GFA.MiniGames.Data.Datasets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GFA.MiniGames.Games.FruitNinja
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField] private WeightedGameObjectSet _data;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Vector2 _randomVelocityMagnitudeRange;
		[SerializeField] private float _randomVelocityAngle;
		[SerializeField] private float _maxAngularVelocity;

		private void Start()
		{
			StartCoroutine(Spawn());
		}

		private Vector3 CalculateRandomVelocity()
		{
			var dir = Vector3.up;
			var randomAngle = Mathf.Lerp(-_randomVelocityAngle, _randomVelocityAngle, Random.value);
			var randomMagnitude = Mathf.Lerp(_randomVelocityMagnitudeRange.x, _randomVelocityMagnitudeRange.y, Random.value);
			
			dir = Quaternion.Euler(0, 0, randomAngle) * dir;
			
			return dir * randomMagnitude;
		}
		
		
		private float CalculateSpawnDuration()
		{
			return 1f;
		}

		private IEnumerator Spawn()
		{
			while (true)
			{
				yield return new WaitForSeconds(CalculateSpawnDuration());
				var objToInstantiate = _data.SelectRandom();
				var inst = Instantiate(objToInstantiate, _spawnPoint.position, Quaternion.identity);
				
				if (inst.TryGetComponent(out Rigidbody rigidbody))
				{
					rigidbody.velocity = CalculateRandomVelocity();
					rigidbody.angularVelocity = Random.onUnitSphere * _maxAngularVelocity;
				}
			}
		}
	}
}