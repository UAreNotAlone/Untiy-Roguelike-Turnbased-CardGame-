using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAlcoholic : MonoBehaviour,IEndBuff
{
    public void EndBuff()
    {
        if (IsInvoking("BanUseCard"))
        {
            CancelInvoke("BanUseCard");
        }
    }
}
