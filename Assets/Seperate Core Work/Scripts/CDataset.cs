using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Linq;
using UnityEngine;

//namespace Assets.Seperate_Core_Work.Scripts.TherapyItemRecSys
//{
class CDataset
{
    int m_intDatasetId = 0;
    List<CLexicalItem> m_lsLexicalItem = new List<CLexicalItem>();
    List<CChallengeItem> m_lsChallengeItem = new List<CChallengeItem>();
    List<CChallengeItemFeatures> m_lsChallengeItemFeatures = new List<CChallengeItemFeatures>();
    List<CVector_ChallengeItemFeaturesNeighbours> m_lsVector_Neighbours = new List<CVector_ChallengeItemFeaturesNeighbours>();
    List<int> m_lsChallengeItemFeatures_Forced = new List<int>();
    List<int> m_lsChallengeItemFeatures_StarterPool = new List<int>();
    List<int> m_lsChallengeItemFeatures_MedianPool = new List<int>();

    double m_dCorpusComplexity_StdDeviation = 0;
    List<double> m_lsCifComplexity_Distinct = new List<double>();

    string m_strFile_LexicalItem_Xml = "stimuli_lexicalitems_all.xml";  //"stimuli_noun_verb_lexicalitems.xml";
    string m_strFile_ChallengeItem_Xml = "stimuli_challengeitems_all.xml";  //"stimuli_noun_verb_challengeitems.xml";
    string m_strFile_ChallengeItemFeature_Csv = "stimuli_challengeitemfeatures_all.csv";  //"stimuli_noun_verb_challengeitemfeatures.csv";
    string m_strFile_ChallengeItemFeatureNeighbours_Csv = "stimuli_challengeitemfeatures_neighbours_all.csv";  //"stimuli_noun_verb_challengeitemfeatures_neighbours.csv";
    string m_strFile_ChallengeItemFeatureForced_Csv = "stimuli_challengeitemfeatures_forced_all.csv";  //"stimuli_noun_verb_challengeitemfeatures_forced.csv";
    string m_strFile_ChallengeItemFeatureStarterPool_Csv = "stimuli_challengeitemfeatures_starterpool.csv";
    string m_strFile_ChallengeItemFeatureMedianPool_Csv = "stimuli_challengeitemfeatures_medianpool.csv";

    //----------------------------------------------------------------------------------------------------
    // CDataset
    //----------------------------------------------------------------------------------------------------
    public CDataset()
    {

    }

    //----------------------------------------------------------------------------------------------------
    // getLexicalItemList
    //----------------------------------------------------------------------------------------------------
    public List<CLexicalItem> getLexicalItemList()
    {
        return m_lsLexicalItem;
    }

    //----------------------------------------------------------------------------------------------------
    // getChallengeItemList
    //----------------------------------------------------------------------------------------------------
    public List<CChallengeItem> getChallengeItemList()
    {
        return m_lsChallengeItem;
    }

    //----------------------------------------------------------------------------------------------------
    // getChallengeItemFeaturesList
    //----------------------------------------------------------------------------------------------------
    public List<CChallengeItemFeatures> getChallengeItemFeaturesList()
    {
        return m_lsChallengeItemFeatures;
    }

    //----------------------------------------------------------------------------------------------------
    // getChallengeItemFeaturesForcedList
    //----------------------------------------------------------------------------------------------------
    public List<int> getChallengeItemFeaturesForcedList()
    {
        return m_lsChallengeItemFeatures_Forced;
    }

    //----------------------------------------------------------------------------------------------------
    // getChallengeItemFeaturesStarterPoolList
    //----------------------------------------------------------------------------------------------------
    public List<int> getChallengeItemFeaturesStarterPoolList()
    {
        return m_lsChallengeItemFeatures_StarterPool;
    }

    //----------------------------------------------------------------------------------------------------
    // getChallengeItemFeaturesMedianPoolList
    //----------------------------------------------------------------------------------------------------
    public List<int> getChallengeItemFeaturesMedianPoolList()
    {
        return m_lsChallengeItemFeatures_MedianPool;
    }

    //----------------------------------------------------------------------------------------------------
    // getVectorNeighboursList
    //----------------------------------------------------------------------------------------------------
    public List<CVector_ChallengeItemFeaturesNeighbours> getVectorNeighboursList()
    {
        return m_lsVector_Neighbours;
    }

    //----------------------------------------------------------------------------------------------------
    // getCorpusComplexityStdDeviation
    //----------------------------------------------------------------------------------------------------
    public double getCorpusComplexityStdDeviation()
    {
        return m_dCorpusComplexity_StdDeviation;
    }

    //----------------------------------------------------------------------------------------------------
    // getCifComplexity_DistinctList  
    //----------------------------------------------------------------------------------------------------
    public List<double> getCifComplexity_DistinctList()
    {
        return m_lsCifComplexity_Distinct;
    }

    //----------------------------------------------------------------------------------------------------
    // loadDataset
    //----------------------------------------------------------------------------------------------------
    public void loadDataset(int intDatasetId)
    {
        m_intDatasetId = intDatasetId;
        if (m_intDatasetId == 0)
        {
            m_strFile_LexicalItem_Xml = "2016-08-setA_stimuli_lexicalitems_all";
            m_strFile_ChallengeItem_Xml = "2016-08-setA_stimuli_challengeitems_all";
            m_strFile_ChallengeItemFeature_Csv = "2016-08-setA_stimuli_challengeitemfeatures_all";
            m_strFile_ChallengeItemFeatureNeighbours_Csv = "2016-08-setA_stimuli_challengeitemfeatures_neighbours_all_30";
            m_strFile_ChallengeItemFeatureForced_Csv = "2016-08-setA_stimuli_challengeitemfeatures_forced_all";
            m_strFile_ChallengeItemFeatureStarterPool_Csv = "2016-08-setA_stimuli_challengeitemfeatures_starterpool";
            m_strFile_ChallengeItemFeatureMedianPool_Csv = "2016-08-setA_stimuli_challengeitemfeatures_medianpool";
        }
        else if (m_intDatasetId == 1)
        {
            m_strFile_LexicalItem_Xml = "2016-10-setB_stimuli_lexicalitems_all";
            m_strFile_ChallengeItem_Xml = "2016-10-setB_stimuli_challengeitems_all";
            m_strFile_ChallengeItemFeature_Csv = "2016-10-setB_stimuli_challengeitemfeatures_all";
            m_strFile_ChallengeItemFeatureNeighbours_Csv = "2016-10-setB_stimuli_challengeitemfeatures_neighbours_all_30";
            m_strFile_ChallengeItemFeatureForced_Csv = "2016-10-setB_stimuli_challengeitemfeatures_forced_all";
            m_strFile_ChallengeItemFeatureStarterPool_Csv = "2016-10-setB_stimuli_challengeitemfeatures_starterpool";
            m_strFile_ChallengeItemFeatureMedianPool_Csv = "2016-10-setB_stimuli_challengeitemfeatures_medianpool";
        }
        else if (m_intDatasetId == 2)
        {
            m_strFile_LexicalItem_Xml = "2016-11-setA_stimuli_lexicalitems_all";
            m_strFile_ChallengeItem_Xml = "2016-11-setA_stimuli_challengeitems_all";
            m_strFile_ChallengeItemFeature_Csv = "2016-11-setA_stimuli_challengeitemfeatures_all";
            m_strFile_ChallengeItemFeatureNeighbours_Csv = "2016-11-setA_stimuli_challengeitemfeatures_neighbours_all_30";
            m_strFile_ChallengeItemFeatureForced_Csv = "2016-11-setA_stimuli_challengeitemfeatures_forced_all";
            m_strFile_ChallengeItemFeatureStarterPool_Csv = "2016-11-setA_stimuli_challengeitemfeatures_starterpool";
            m_strFile_ChallengeItemFeatureMedianPool_Csv = "2016-11-setA_stimuli_challengeitemfeatures_medianpool";
        }
        else if (m_intDatasetId == 3)
        {
            m_strFile_LexicalItem_Xml = "2016-12-setB_stimuli_lexicalitems_all";
            m_strFile_ChallengeItem_Xml = "2016-12-setB_stimuli_challengeitems_all";
            m_strFile_ChallengeItemFeature_Csv = "2016-12-setB_stimuli_challengeitemfeatures_all";
            m_strFile_ChallengeItemFeatureNeighbours_Csv = "2016-12-setB_stimuli_challengeitemfeatures_neighbours_all_30";
            m_strFile_ChallengeItemFeatureForced_Csv = "2016-12-setB_stimuli_challengeitemfeatures_forced_all";
            m_strFile_ChallengeItemFeatureStarterPool_Csv = "2016-12-setB_stimuli_challengeitemfeatures_starterpool";
            m_strFile_ChallengeItemFeatureMedianPool_Csv = "2016-12-setB_stimuli_challengeitemfeatures_medianpool";
        }

        // load lexical items (xml)
        loadLexicalItems();

        // load challenge items (xml)
        loadChallengeItems();

        // load features (csv)
        loadChallengeItemFeatures();

        // load features (csv)
        loadChallengeItemFeatures_Forced();

        // load features (csv)
        loadChallengeItemFeatures_StarterPool();
        loadChallengeItemFeatures_MedianPool();

        // load features neighbours (csv)
        loadChallengeItemFeatures_Neighbours();

        // calculate corpus complexity std deviation
        List<double> lsComplexity = new List<double>();
        for (int i = 0; i < m_lsChallengeItemFeatures.Count; i++)
        {
            lsComplexity.Add((double)m_lsChallengeItemFeatures[i].m_dComplexity_Overall);
            int intIdx = m_lsCifComplexity_Distinct.FindIndex(a => a == m_lsChallengeItemFeatures[i].m_dComplexity_Overall);
            if (intIdx <= -1)
                m_lsCifComplexity_Distinct.Add(m_lsChallengeItemFeatures[i].m_dComplexity_Overall);
        }
        m_dCorpusComplexity_StdDeviation = Math.Round(calculateStdDeviation(lsComplexity), 4);
        Console.WriteLine("corpus complexity sd = " + m_dCorpusComplexity_StdDeviation);  

        // sort complexity list
        m_lsCifComplexity_Distinct = m_lsCifComplexity_Distinct.OrderBy(p => p).ToList();
    }

    //----------------------------------------------------------------------------------------------------
    // loadLexicalItems (xml)
    //----------------------------------------------------------------------------------------------------
    public void loadLexicalItems()
    {
        // if xml has already been loaded, then return
        // if (m_lsLexicalItem.Count > 0) return;

        m_lsLexicalItem.Clear();

        // check if file exists
        /*string strXmlFile = m_strFile_LexicalItem_Xml; // "stimuli_noun_1syllables_lexicalitems.xml"; 
        if (!System.IO.File.Exists(strXmlFile))
            return;
        XElement root = XElement.Load(strXmlFile);*/

        TextAsset textAsset = Resources.Load("Doc/" + m_strFile_LexicalItem_Xml) as TextAsset;
        XElement root = XElement.Parse(textAsset.text);

        int intIdx = 0;
        m_lsLexicalItem = (
            from el in root.Elements("item")
            select new CLexicalItem
            {
                m_intId = intIdx++,  /*(int)el.Attribute("idx"),*/
                m_strName = (string)el.Element("name"),
                m_lsChallengeItemIdx = (
                    from el2 in el.Elements("ciIdx")
                    select ((int)el2)
                ).ToList(),
            }
        ).ToList();

        /*for (int i = 0; i < m_lsLexicalItem.Count; i++)
        {
            Console.WriteLine(m_lsLexicalItem[i].m_strName);
            string str = "";
            for (int j = 0; j < m_lsLexicalItem[i].m_lsChallengeItemIdx.Count; j++)
                str = str + m_lsLexicalItem[i].m_lsChallengeItemIdx[j] + ", ";
            Console.WriteLine(str);
        }*/
    }

    //----------------------------------------------------------------------------------------------------
    // loadChallengeItems (xml)
    //----------------------------------------------------------------------------------------------------
    public void loadChallengeItems()
    {
        // if xml has already been loaded, then return
        //if (m_lsChallengeItem.Count > 0) return;

        m_lsChallengeItem.Clear();

        // check if file exists
        /*string strXmlFile = m_strFile_ChallengeItem_Xml; // "stimuli_noun_1syllables_challengeitems.xml";
        if (!System.IO.File.Exists(strXmlFile))
            return;
        XElement root = XElement.Load(strXmlFile);*/

        TextAsset textAsset = Resources.Load("Doc/" + m_strFile_ChallengeItem_Xml) as TextAsset;
        XElement root = XElement.Parse(textAsset.text);

        int intIdx = 0;
        m_lsChallengeItem = (
            from el in root.Elements("item")
            select new CChallengeItem
            {
                m_intId = intIdx++,  /*(int)el.Attribute("idx"),*/
                m_strName = (string)el.Element("name"),
                m_intFrequency = (int)el.Element("freq"),
                m_intConcreteness = (int)el.Element("conc"),
                m_intSyllableNum = (int)el.Element("syll"),
                m_intLexicalItemIdx = (int)el.Element("li"),
                m_intLinguisticType = (int)el.Element("lt"),
                m_strLinguisticCategoryName = (string)el.Element("lcn"),
                    /*m_strAudioFile = (string)el.Element("audioFile"),*/
                m_lsAudioFile = (
                    from el2 in el.Elements("aud")
                    select ((string)el2)
                ).ToList(),
                m_intTargetIdx = (int)el.Element("tgt"),
                m_lsPictureChoice = (
                    from el2 in el.Elements("picC")
                    select new CPictureChoice
                    {
                        m_strName = (string)el2.Attribute("n"),
                        m_strImageFile = (string)el2.Attribute("img"),
                        m_strType = (string)el2.Attribute("t"),
                    }
                ).ToList(),
                m_lsChallengeItemFeaturesIdx = (
                    from el2 in el.Elements("cifIdx")
                    select ((int)el2)
                ).ToList(),
                m_intForcedItem = (int)el.Element("forced"),
            }
        ).ToList();

    }

    //----------------------------------------------------------------------------------------------------
    // loadChallengeItemFeatures
    //----------------------------------------------------------------------------------------------------
    public void loadChallengeItemFeatures()
    {
        // if xml has already been loaded, then return
        //if (m_lsChallengeItemFeatures.Count > 0) return;

        m_lsChallengeItemFeatures.Clear();

        //string strWholeFile = System.IO.File.ReadAllText(m_strFile_ChallengeItemFeature_Csv); // @"stimuli_noun_1syllables_neighbours.csv");

        TextAsset textAsset = Resources.Load("Doc/" + m_strFile_ChallengeItemFeature_Csv) as TextAsset;
        string strWholeFile = textAsset.text;

        // split into lines
        strWholeFile = strWholeFile.Replace('\n', '\r');
        string[] lines = strWholeFile.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // see how many rows & columns there are
        int intNumRows = lines.Length;
        //int intNumCols = lines[0].Split(',').Length;

        int i = 0;
        while (i < intNumRows)
        {
            //Console.WriteLine (lines [i]);

            CChallengeItemFeatures features = new CChallengeItemFeatures();

            string[] line_r = lines[i].Split(',');
            int intNumCols = line_r.Length;

            // first col = idx
            for (int j = 1; j < intNumCols; j++)
            {
                if (j == 1) features.m_intChallengeItemIdx = Convert.ToInt32(line_r[j]);
                if (j == 2) features.m_intFrequency = Convert.ToInt32(line_r[j]);
                if (j == 3) features.m_intConcreteness = Convert.ToInt32(line_r[j]);
                if (j == 4) features.m_intDistractorNum = Convert.ToInt32(line_r[j]);
                if (j == 5) features.m_intLinguisticCategory = Convert.ToInt32(line_r[j]);
                if (j == 6) features.m_intLinguisticType = Convert.ToInt32(line_r[j]);
                if (j == 7) features.m_dComplexity_Frequency = Math.Round(Convert.ToDouble(line_r[j]), 4);
                if (j == 8) features.m_dComplexity_Concreteness = Math.Round(Convert.ToDouble(line_r[j]), 4);
                if (j == 9) features.m_dComplexity_DistractorNum = Math.Round(Convert.ToDouble(line_r[j]), 4);
                if (j == 10) features.m_dComplexity_LinguisticType = Math.Round(Convert.ToDouble(line_r[j]), 4);
                if (j == 11) features.m_dComplexity_Overall = Math.Round(Convert.ToDouble(line_r[j]), 4);
            }
            m_lsChallengeItemFeatures.Add(features);

            i++; // next line
        }    // end while

        /*{
            string str = "";
            str = str + m_lsChallengeItemFeatures[0].m_dComplexity_Frequency + ", " +
                    m_lsChallengeItemFeatures[0].m_dComplexity_Concreteness + ", " +
                    m_lsChallengeItemFeatures[0].m_dComplexity_DistractorNum + ", " +
                    m_lsChallengeItemFeatures[0].m_dComplexity_LinguisticCategory + ", " +
                    m_lsChallengeItemFeatures[0].m_dComplexity_NoiseLevel + ", ";
            Console.WriteLine(str);
        }*/
    }

    //----------------------------------------------------------------------------------------------------
    // loadChallengeItemFeatures_Forced
    //----------------------------------------------------------------------------------------------------
    public void loadChallengeItemFeatures_Forced()
    {
        // if xml has already been loaded, then return
        //if (m_lsChallengeItemFeatures_Forced.Count > 0) return;

        m_lsChallengeItemFeatures_Forced.Clear();

        //string strWholeFile = System.IO.File.ReadAllText(m_strFile_ChallengeItemFeatureForced_Csv);

        TextAsset textAsset = Resources.Load("Doc/" + m_strFile_ChallengeItemFeatureForced_Csv) as TextAsset;
        string strWholeFile = textAsset.text;

        // split into lines
        strWholeFile = strWholeFile.Replace('\n', '\r');
        string[] lines = strWholeFile.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // see how many rows & columns there are
        int intNumRows = lines.Length;
        //int intNumCols = lines[0].Split(',').Length;

        int i = 0;
        while (i < intNumRows)
        {
            CChallengeItemFeatures features = new CChallengeItemFeatures();

            string[] line_r = lines[i].Split(',');
            int intNumCols = line_r.Length;
            
            m_lsChallengeItemFeatures_Forced.Add(Convert.ToInt32(line_r[0]));

            i++; // next line
        }    // end while            
    }

    //----------------------------------------------------------------------------------------------------
    // loadChallengeItemFeatures_StarterPool
    //----------------------------------------------------------------------------------------------------
    public void loadChallengeItemFeatures_StarterPool()
    {
        // if xml has already been loaded, then return
        //if (m_lsChallengeItemFeatures_StarterPool.Count > 0) return;

        m_lsChallengeItemFeatures_StarterPool.Clear();

        //string strWholeFile = System.IO.File.ReadAllText(m_strFile_ChallengeItemFeatureStarterPool_Csv);

        TextAsset textAsset = Resources.Load("Doc/" + m_strFile_ChallengeItemFeatureStarterPool_Csv) as TextAsset;
        string strWholeFile = textAsset.text;

        // split into lines
        strWholeFile = strWholeFile.Replace('\n', '\r');
        string[] lines = strWholeFile.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // see how many rows & columns there are
        int intNumRows = lines.Length;
        //int intNumCols = lines[0].Split(',').Length;

        int i = 0;
        while (i < intNumRows)
        {
            string[] line_r = lines[i].Split(',');
            int intNumCols = line_r.Length;
            m_lsChallengeItemFeatures_StarterPool.Add(Convert.ToInt32(line_r[0]));

            i++; // next line
        }    // end while            
    }

    //----------------------------------------------------------------------------------------------------
    // loadChallengeItemFeatures_MedianPool
    //----------------------------------------------------------------------------------------------------
    public void loadChallengeItemFeatures_MedianPool()
    {
        // if xml has already been loaded, then return
        //if (m_lsChallengeItemFeatures_StarterPool.Count > 0) return;

        m_lsChallengeItemFeatures_MedianPool.Clear();

        //string strWholeFile = System.IO.File.ReadAllText(m_strFile_ChallengeItemFeatureMedianPool_Csv);

        TextAsset textAsset = Resources.Load("Doc/" + m_strFile_ChallengeItemFeatureMedianPool_Csv) as TextAsset;
        string strWholeFile = textAsset.text;

        // split into lines
        strWholeFile = strWholeFile.Replace('\n', '\r');
        string[] lines = strWholeFile.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // see how many rows & columns there are
        int intNumRows = lines.Length;
        //int intNumCols = lines[0].Split(',').Length;

        int i = 0;
        while (i < intNumRows)
        {
            string[] line_r = lines[i].Split(',');
            int intNumCols = line_r.Length;
            m_lsChallengeItemFeatures_MedianPool.Add(Convert.ToInt32(line_r[0]));

            i++; // next line
        }    // end while            
    }


    //----------------------------------------------------------------------------------------------------
    // loadChallengeItemFeatures_Neighbours
    //----------------------------------------------------------------------------------------------------
    public void loadChallengeItemFeatures_Neighbours()
    {
        // if xml has already been loaded, then return
        //if (m_lsVector_Neighbours.Count > 0) return;

        m_lsVector_Neighbours.Clear();

        //string strWholeFile = System.IO.File.ReadAllText(m_strFile_ChallengeItemFeatureNeighbours_Csv);

        TextAsset textAsset = Resources.Load("Doc/" + m_strFile_ChallengeItemFeatureNeighbours_Csv) as TextAsset;
        string strWholeFile = textAsset.text;


        // split into lines
        strWholeFile = strWholeFile.Replace('\n', '\r');
        string[] lines = strWholeFile.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // see how many rows & columns there are
        int intNumRows = lines.Length;
        //int intNumCols = lines[0].Split(',').Length;

        int i = 0;
        while (i < intNumRows)
        {
            //Console.WriteLine (lines [i]);

            CVector_ChallengeItemFeaturesNeighbours lsNeighbour = new CVector_ChallengeItemFeaturesNeighbours();

            string[] line_r = lines[i].Split(',');
            int intNumCols = line_r.Length;

            int j = 0;
            j++; // first col = idx
            while (j < intNumCols - 1)
            {
                CChallengeItemFeatures_Neighbour neighbour = new CChallengeItemFeatures_Neighbour();

                for (int k = 1; k < 3; k++)  // each neighbour has 1 similarity metric
                {
                    if (k == 1) neighbour.m_intChallengeItemFeaturesIdx = Convert.ToInt32(line_r[j]);
                    if (k == 2) neighbour.m_dSimilarity = Math.Round(Convert.ToDouble(line_r[j]), 4);
                    j++;
                }
                lsNeighbour.m_lsChallengeItemFeatures_Neighbour.Add(neighbour);
            }

            m_lsVector_Neighbours.Add(lsNeighbour);

            i++; // next line
        }    // end while

        /*for (i = 0; i < m_lsChallengeItem_NeighbourList[0].m_lsChallengeItem_Neighbour.Count; i++)
        {
            string str = "";
            str = str + m_lsChallengeItem_NeighbourList[0].m_lsChallengeItem_Neighbour[i].m_dSimilarity_Frequency + ", " +                        
                    m_lsChallengeItem_NeighbourList[0].m_lsChallengeItem_Neighbour[i].m_dSimilarity_LexicalItem + ", ";
            //Console.WriteLine(str);
        }*/
    }

    //----------------------------------------------------------------------------------------------------
    // calculateStdDeviation
    //----------------------------------------------------------------------------------------------------
    private double calculateStdDeviation(IEnumerable<double> values)
    {
        double ret = 0;
        if (values.Count() > 0)
        {
            //Compute the Average      
            double avg = values.Average();
            //Perform the Sum of (value-avg)_2_2      
            double sum = values.Sum(d => Math.Pow(d - avg, 2));
            //Put it all together      
            ret = Math.Sqrt((sum) / (values.Count()));  // Math.Sqrt((sum) / (values.Count() - 1));
        }
        return ret;
    }
}
//}
