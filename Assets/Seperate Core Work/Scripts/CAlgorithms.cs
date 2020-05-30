using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts
//{

//************************************************************************************************************************
// CLASS- CCosineSimilarity
//************************************************************************************************************************
public class CCosineSimilarity
{
    public static double GetCosineSimilarity(List<int> V1, List<int> V2)
    {
        int N = 0;
        N = ((V2.Count < V1.Count) ? V2.Count : V1.Count);
        double dot = 0.0d;
        double mag1 = 0.0d;
        double mag2 = 0.0d;
        for (int n = 0; n < N; n++)
        {
            dot += V1[n] * V2[n];
            mag1 += Math.Pow(V1[n], 2);
            mag2 += Math.Pow(V2[n], 2);
        }

        return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
    }
    /*public static double GetCosineSimilarity(List<double> V1, List<double> V2)
    {
        double sim = 0.0d;
        int N = 0;
        N = ((V2.Count < V1.Count) ? V2.Count : V1.Count);
        double dot = 0.0d;
        double mag1 = 0.0d;
        double mag2 = 0.0d;
        for (int n = 0; n < N; n++)
        {
            dot += V1[n] * V2[n];
            mag1 += Math.Pow(V1[n], 2);
            mag2 += Math.Pow(V2[n], 2);
        }

        return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
    }*/
}

//************************************************************************************************************************
// CLASS- CEuclideanDistance
//************************************************************************************************************************
public class CEuclideanDistance
{
    /// <summary>
    /// Return the distance between 2 points
    /// </summary>
    public static double Euclidean(List<double> lsThisItemFeatures, List<double> lsItemFeatures)
    {
        double dPowSum = 0;
        for (var i = 0; i < lsThisItemFeatures.Count; i++)
        {
            dPowSum = dPowSum + Math.Pow(lsThisItemFeatures[i] - lsItemFeatures[i], 2);
        }

        return Math.Sqrt(dPowSum);
    }

    /*public static double Euclidean(Point p1, Point p2)
	{
		return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
	}*/

    /// <summary>
    /// Calculates the similarity between 2 points using Euclidean distance.
    /// Returns a value between 0 and 1 where 1 means they are identical
    /// </summary>
    public static double EuclideanSimilarity(List<double> lsThisItemFeatures, List<double> lsItemFeatures)
    {
        return 1 / (1 + Euclidean(lsThisItemFeatures, lsItemFeatures));
    }

    /*public static double EuclideanSimilarity(Point p1, Point p2)
	{
		return 1/(1 + Euclidean(p1, p2));
	}*/
}
//}
