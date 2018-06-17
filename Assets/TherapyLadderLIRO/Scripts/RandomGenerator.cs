using System;
using System.Collections;

public static class RandomGenerator  {

    public static Random rand = new Random((int) DateTime.Now.Ticks & 0x0000FFFF) ;

    public static int GetRandom()
    {
        return rand != null ? rand.Next() : 0;
    }
}
