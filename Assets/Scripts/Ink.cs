using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink {

    private float ink;

    public Ink(float ink)
    {
        this.ink = ink;
    }

    public void Use(float use)
    {
        ink -= use;
        if (ink < 0)
        {
            ink = 0;
        }
    }

    public float GetInk()
    {
        return ink;
    }

    public void AddInk(float extra)
    {
        ink += extra;
    }
}
