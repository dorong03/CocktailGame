using System;
using UnityEngine;

public class ServingController : MonoBehaviour
{
    public Action<Grade> OnServingCompleted;

    private MixResult result;
    private int seatIndex;

    public void StartServing()
    {

    }
    public void ThrowCocktail()
    {

    }
    public Grade Judge()
    {
        return Grade.A;
    }
}
