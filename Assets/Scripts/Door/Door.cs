using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    bool toggle;
    public Animator anim;

    public void openClose()
    {
        toggle = !toggle;
        if (toggle == false)
        {
            anim.ResetTrigger("open");
            anim.SetTrigger("close");
        }
        if (toggle == true)
        {
            anim.ResetTrigger("close");
            anim.SetTrigger("open");
        }
    }
}

