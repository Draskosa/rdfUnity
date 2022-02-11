using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Writing;

public class HUD : MonoBehaviour
{
    public GameObject MessagePanel;
    public GameObject InfoPanel;
    public Text Info;
    public Text Description;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenMessagePanel(string text)
    {
        MessagePanel.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
    }

    public void OpenInfoPanel(string oeuvre)
    {
        Info.text = "Le titre est ";
        Description.text = "";
        string resultat = "";
        TripleStore store = new TripleStore();
        //store.LoadFromFile("musee.ttl");
        //Object results = store.ExecuteQuery(@"PREFIX dbo: <http://dbpedia.org/ontology/>  SELECT * WHERE { ?s dbo:region ?o . } LIMIT 100");
        store.LoadFromFile("Assets/Scripts/louvres.ttl");
        string nameSearch = "{ ?s foaf:name ?o . FILTER regex(?o,'"+oeuvre+"') ?s rdfs:comment ?n . } LIMIT 1";
        Debug.Log(nameSearch);
        System.Object results = store.ExecuteQuery(@"PREFIX foaf: <http://xmlns.com/foaf/0.1/> PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>  SELECT ?o ?n WHERE " + nameSearch);
        //System.Object results = store.ExecuteQuery(@"PREFIX dbo: <http://dbpedia.org/ontology/>   select ?n where { { ?x dbo:region ?n } union { ?x dbo:department ?l } }");
        //Object results = store.ExecuteQuery(@"PREFIX dbo: <http://dbpedia.org/ontology/>   select * where {  ?x dbo:region ?n  optional { ?x dbo:department ?l } }");
        //System.Uri uri = new System.Uri("C:\\Users\\ivan9\\source\repos\\RDFprojet\\RDFprojet\\bin\\Debug\\netcoreapp3.1\\musee.ttl");
        if (results is SparqlResultSet)
        {
            //Print out the Results
            SparqlResultSet rset = (SparqlResultSet)results;
            
            
            resultat = rset[0].ToString();
            
            string[] result = resultat.Split(new string[] { "=" }, StringSplitOptions.None);
            
            Info.text += result[2];
            Description.text += result[1].Substring(0, result[1].Length - 5);

        }
        
        InfoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        InfoPanel.SetActive(false);
    }
}
