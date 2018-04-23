using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class CoreItemReader : ICsvReader<Challenge>
{
    IEnumerable<Challenge> ICsvReader<Challenge>.ParseCsv(string path)
    {
        List<Challenge> currList = new List<Challenge>();

        //Read a single file and extract the list of challenges

        return currList;

    }
}
