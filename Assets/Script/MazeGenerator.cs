using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Paramètres du Labyrinthe")]
    [SerializeField] private MazeBlock _mazeBlockPrefab;
    [SerializeField] private int _largeur = 10;
    [SerializeField] private int _longueur = 10;

    private MazeBlock[,] _grille;

    void Start()
    {
        InitialiserGrille();
        GenererLabyrinthe(null, _grille[0, 0]);
    }

    private void InitialiserGrille()
    {
        _grille = new MazeBlock[_largeur, _longueur];

        for (int x = 0; x < _largeur; x++)
        {
            for (int z = 0; z < _longueur; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                _grille[x, z] = Instantiate(_mazeBlockPrefab, position, Quaternion.identity, transform);
                _grille[x, z].name = $"Bloc ({x},{z})";
            }
        }
    }

    private void GenererLabyrinthe(MazeBlock blocPrecedent, MazeBlock blocActuel)
    {
        blocActuel.MarquerCommeVisite();
        RetirerMursAdjacents(blocPrecedent, blocActuel);

        MazeBlock prochainBloc;
        do
        {
            prochainBloc = TrouverProchainBlocAleatoire(blocActuel);
            if (prochainBloc != null)
            {
                GenererLabyrinthe(blocActuel, prochainBloc);
            }
        }
        while (prochainBloc != null);
    }

    private MazeBlock TrouverProchainBlocAleatoire(MazeBlock blocActuel)
    {
        List<MazeBlock> voisinsNonVisites = GetVoisinsNonVisites(blocActuel);
        return voisinsNonVisites.OrderBy(_ => Random.value).FirstOrDefault();
    }

    private List<MazeBlock> GetVoisinsNonVisites(MazeBlock blocActuel)
    {
        List<MazeBlock> voisins = new List<MazeBlock>();

        Vector3 position = blocActuel.transform.position;
        int x = Mathf.FloorToInt(position.x);
        int z = Mathf.FloorToInt(position.z);

   
        VerifierVoisin(x + 1, z, voisins); // Droite
        VerifierVoisin(x - 1, z, voisins); // Gauche
        VerifierVoisin(x, z + 1, voisins); // Avant
        VerifierVoisin(x, z - 1, voisins); // Arrière

        return voisins;
    }

    private void VerifierVoisin(int x, int z, List<MazeBlock> liste)
    {
        if (EstDansGrille(x, z))
        {
            MazeBlock voisin = _grille[x, z];
            if (!voisin.EstVisite)
            {
                liste.Add(voisin);
            }
        }
    }

    private bool EstDansGrille(int x, int z)
    {
        return x >= 0 && x < _largeur && z >= 0 && z < _longueur;
    }

    private void RetirerMursAdjacents(MazeBlock blocPrecedent, MazeBlock blocActuel)
    {
        if (blocPrecedent == null) return;

        Vector3 direction = blocActuel.transform.position - blocPrecedent.transform.position;

        
        if (direction.x > 0) // Vers la droite
        {
            blocPrecedent.DetruireMurDroite();
            blocActuel.DetruireMurGauche();
        }
        else if (direction.x < 0) // Vers la gauche
        {
            blocPrecedent.DetruireMurGauche();
            blocActuel.DetruireMurDroite();
        }
        else if (direction.z > 0) // Vers l'avant
        {
            blocPrecedent.DetruireMurAvant();
            blocActuel.DetruireMurArriere();
        }
        else if (direction.z < 0) // Vers l'arrière
        {
            blocPrecedent.DetruireMurArriere();
            blocActuel.DetruireMurAvant();
        }
    }
}