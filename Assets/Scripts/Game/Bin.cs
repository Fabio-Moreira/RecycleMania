using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(this.tag) && collision.gameObject.GetComponent<Trash>().destroy==false)
        {
            switch(this.tag)
            {
                case ("Bio"):
                    Game.instance.addPoints(1);
                    break;
                case ("Electronics"):
                    Game.instance.addPoints(5);
                    break;
                case ("Glass"):
                    Game.instance.addPoints(3);
                    break;
                case ("Metal"):
                    Game.instance.addPoints(4);
                    break;
                case ("Paper"):
                    Game.instance.addPoints(2);
                    break;
                case ("Plastic"):
                    Game.instance.addPoints(1);
                    break;
                default:
                    break;

            }
            collision.gameObject.GetComponent<Trash>().BeginDestroy();
        }
        else if(collision.gameObject.GetComponent<Trash>().destroy==false)
        {
            Handheld.Vibrate();
            Game.instance.loseLive();
            collision.gameObject.GetComponent<Trash>().BeginDestroy();
            switch (this.tag)
            {
                case ("Bio"):
                    Game.instance.addPoints(-4);
                    break;
                case ("Electronics"):
                    Game.instance.addPoints(-5);
                    break;
                case ("Glass"):
                    Game.instance.addPoints(-2);
                    break;
                case ("Metal"):
                    Game.instance.addPoints(-3);
                    break;
                case ("Paper"):
                    Game.instance.addPoints(-1);
                    break;
                case ("Plastic"):
                    Game.instance.addPoints(-1);
                    break;
                default:
                    break;

            }
        }
    }
}
