using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roles : MonoBehaviour
{
    public enum Role // enums are just numbers that are represented with words
    {
        Escort,
        Disruptive,
        Killer,
        Loner,
        Insider,
        Thief,
    }
    private Roles player;
    private bool isDead = false;
    private void Start()
    {
        player = GetComponent<Roles>();
    }
    private void Update()
    {
        
    }
    private void WinState(bool _achieved)
    {
        if (!player.isDead)
        {

        }
    }
}
/* 
 * escort.     locate and secure in world item
 * disruptive. cause chaos, have fun
 * killer.     eliminate your target
 * loner.      all other players must be dead
 * insider.    you know one other role. they know that you know. others are as aware. you do not know who it is nor do they know you
 * thief.      kill other player and win their role. new role not disclosed
 */