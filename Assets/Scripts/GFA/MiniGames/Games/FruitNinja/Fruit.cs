using Unity.VisualScripting;
using UnityEngine;

namespace GFA.MiniGames.Games.FruitNinja
{
    public class Fruit : MonoBehaviour, ICuttable
    {
        [SerializeField]
        private Renderer _graphics;
        public bool IsCut { get; set; }
        
        public void Cut(Vector3 normal, float distance)
        {
            var velocity = GetComponent<Rigidbody>().velocity;
            
            var side1 = Instantiate(_graphics, transform.position, transform.rotation);
            
            var localNormal = transform.InverseTransformDirection(normal);
            
            side1.material.SetVector("_Plane",new Vector4(localNormal.x, localNormal.y, localNormal.z, distance));
            var rb1 = side1.AddComponent<Rigidbody>();
            rb1.velocity = velocity;
            rb1.AddForce(normal * 3, ForceMode.Impulse);
            
            var side2 = Instantiate(_graphics, transform.position, transform.rotation);
            side2.material.SetVector("_Plane",-new Vector4(localNormal.x, localNormal.y, localNormal.z, distance));
            var rb2 = side2.AddComponent<Rigidbody>();
            rb2.velocity = velocity;
            rb2.AddForce(-normal * 3, ForceMode.Impulse);
            
            Destroy(gameObject);
        }

    }
}
