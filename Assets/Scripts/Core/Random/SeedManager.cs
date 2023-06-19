using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SeedManager
{

    protected static System.Random _rngGenerator;

    protected static bool seedGenerated;

    protected static string seed;

    protected static int _intSeed;
    
    public static void SetSeed(string passwordseed)
    {
        seed = passwordseed;
        _intSeed = seed.GetHashCode();
        _rngGenerator = new System.Random(_intSeed);
    }

    public static int NextInt(int min,int max)
    {
        return _rngGenerator.Next(min, max);
    }

    public static void GenerateRandomSeed()
    {
        string alphanumerics = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        seed = "";
        System.Random r= new System.Random();
        for( int i = 0 ; i < 8 ; i++)
        {
            seed = string.Format("{0}{1}", seed,
                r.Next(0, alphanumerics.Length));
        }
    }
  
}
