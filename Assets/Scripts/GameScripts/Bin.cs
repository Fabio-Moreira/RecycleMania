using UnityEngine;

namespace GameScripts
{
    public class Bin : MonoBehaviour
    {
        public Game game { set; private get; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                game.AddPoints();
                Destroy(collision.gameObject);
            }
            else
            {
                game.LoseLive();
                Destroy(collision.gameObject);
            }
        }
    }
}