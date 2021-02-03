using UnityEngine;

namespace GameScripts
{
    public class Bin : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                Game.GetInstance().AddPoints();
                Destroy(collision.gameObject);
            }
            else
            {
                Game.GetInstance().LoseLive();
                Destroy(collision.gameObject);
            }
        }
    }
}