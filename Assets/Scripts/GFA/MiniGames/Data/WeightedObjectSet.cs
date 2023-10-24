using TMPro;
using UnityEngine;

namespace GFA.MiniGames.Data
{
    public abstract class WeightedObjectSet<T> : ScriptableObject where T: class
    {
        [SerializeField]
        private WeightedObject<T>[] _objects;

        private float TotalWeight
        {
            get
            {
                float weight = 0;
                foreach (var obj in _objects)
                {
                    weight += obj.Weight;
                }

                return weight;
            }
        }
        
        public T SelectRandom()
        {
            var totalWeight = TotalWeight;
            var randomValue = Random.value * totalWeight;
            float currWeight = 0;
            
            foreach (var obj in _objects)
            {
                currWeight += obj.Weight;
                if (randomValue < currWeight)
                {
                    return obj.Object;
                }
            }
            return null;
        }
        

        [System.Serializable]
        public class WeightedObject<T> where T: class
        {
            public T Object;
            public float Weight;
        }
    }
}
