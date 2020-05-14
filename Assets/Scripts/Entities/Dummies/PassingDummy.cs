﻿using System.Collections;
using UnityEngine;

public class PassingDummy : BasicDummy
{
    public IEnumerator ThrowPass(ulong p)
    {
        yield return new WaitForSeconds(3.0f);
        GameManager.GetBallHandling().TryPassBall(m_player, GameManager.GetPlayer(p), PassType.CHESS);
    }

}
