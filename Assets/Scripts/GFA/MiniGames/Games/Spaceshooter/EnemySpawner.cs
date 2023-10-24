using System.Collections;
using UnityEngine;

namespace GFA.MiniGames.Games.Spaceshooter
{
    public class EnemySpawner : MonoBehaviour
    {
        
        private float _startTime;

        [SerializeField]
        private AnimationCurve _spawnDelayCurve;
        
        [SerializeField]
        private Enemy _enemyPrefab;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private float _radius;
        
        private IEnumerator Start()
        {
            _startTime = Time.time;
            while (true)
            {
                yield return new WaitForSeconds(_spawnDelayCurve.Evaluate(Time.time - _startTime));
                var inst = Instantiate(_enemyPrefab);

                var randomPoint = Random.insideUnitCircle.normalized;
                var spawnPoint = randomPoint * _radius;

                inst.transform.position = spawnPoint;
                
                inst.Player = _player;
            }
        }
    }
}
