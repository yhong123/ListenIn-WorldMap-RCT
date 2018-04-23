using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ICsvReader<T> {

    IEnumerable<T> ParseCsv(string path);

}
