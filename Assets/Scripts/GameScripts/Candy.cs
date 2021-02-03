using UnityEngine;

namespace GameScripts
{
    public class Candy : MonoBehaviour
    {
        private Rigidbody2D rb2d;
        public Spawner Spawner {set; private get; }
        
        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            rb2d.velocity = new Vector3(0,Game.GetInstance().transportBelt.GetCurrentSpeed(),0);
        }
    }    
}

