using UnityEngine;

namespace GameScripts
{
    public class Candy : MonoBehaviour
    {
        public Game game { set; private get; }
        private Rigidbody2D rb2d;
        
        private void Start()
        {
            rb2d = this.GetComponent<Rigidbody2D>();
        }
        
        void Update()
        {
            rb2d.velocity = new Vector3(0,game.transportBelt.GetCurrentSpeed(),0);
        }
    }    
}

