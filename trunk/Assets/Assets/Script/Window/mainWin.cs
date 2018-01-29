using UnityEngine;
using System.Collections;

public class mainWin : SingletonWindow<mainWin>
{
    public override string m_resUrl
    {
        get
        {
            return "UI Root";
        }
    }
}
