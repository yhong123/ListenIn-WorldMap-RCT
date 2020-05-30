using UnityEngine;
using System.Collections;

public class DatabaseQuery {

    public string query_url;
    public WWWForm query_form;

    public DatabaseQuery(string new_query_url, WWWForm new_query_form)
    {
        query_url = new_query_url;
        query_form = new_query_form;
    }
}
