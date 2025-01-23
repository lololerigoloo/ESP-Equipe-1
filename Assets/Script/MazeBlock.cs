using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject MurGauche;
    [SerializeField]
    private GameObject MurDroite;
    [SerializeField]
    private GameObject MurAvant;
    [SerializeField]
    private GameObject MurArrière;
    [SerializeField]
    private GameObject BlocNonVisités;
    public bool EstVisite {  get; private set; }

    public void MarquerCommeVisite()
    {
        EstVisite=true;
        Destroy(BlocNonVisités);
    }
    public void DetruireMurGauche()
    {
        Destroy(MurGauche);
    }
    public void DetruireMurDroite()
    {
        Destroy(MurDroite);
    }
    public void DetruireMurAvant()
    {
        Destroy(MurAvant);
    }
    public void DetruireMurArriere()
    {
        Destroy(MurArrière);
    }
}
