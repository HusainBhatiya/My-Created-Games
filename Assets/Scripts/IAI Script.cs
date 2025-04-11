using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAIScript : MonoBehaviour
{
    public interface IAI
    {
        void ExecuteTurn();
    }
}
