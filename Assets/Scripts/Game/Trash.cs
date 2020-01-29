using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public bool destroy = false;

    private void FixedUpdate()
    {
        if (destroy)
        {
            Vector3 scale = this.transform.localScale;
            
            if(scale.x > 0f)
            {
                scale.x -= 0.03f;
                scale.y -= 0.03f;
                this.transform.localScale = scale;
            }
            else
            {
                
                Destroy(this.gameObject);
            }
        }
    }

    public void BeginDestroy()
    {
        destroy = true;
    }
}
