using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Analysis;

namespace XGBoostNetLibDriver;

using XGBoostNetLib;

class Program
{
    static void Main(string[] args)
    {
        // for a classification task
	const string datasetFname = "breast_cancer.csv";
	var dataDir = Path.Combine(AppContext.BaseDirectory, "data");
	var dataPath = Path.Combine(dataDir, datasetFname);
	if (!File.Exists(dataPath)) {
	  Console.WriteLine($"Can't find necessary filename");
	  Environment.Exit(1);
	}

	var df = DataFrame.LoadCsv(dataPath, header: false);
	// (df as IDataView).Schema

	var sampleCount = (uint)(df.Rows.Count);
	var featureCount = (uint)(df.Columns.Count - 1);
	
	List<float> features = new();
	List<float> labels = new();
	for (int i = 0; i < df.Rows.Count; i++) {
	  for (int j = 0; j < df.Columns.Count - 1; j++) {
	    features.Add((float)(df[i, j]));
	  }
    	  labels.Add((float)(df[i, df.Columns.Count - 1]));
	}

        Console.WriteLine($"Checking that I have {sampleCount} samples: ({labels.Count()}, {features.Count() / featureCount})");

        Console.WriteLine($"Dealing with XGBoost version {XGBoostUtils.XgbMajorVersion()}.");

	var dTrain = new DMatrix(features.ToArray(), sampleCount, featureCount, labels.ToArray());
        Console.WriteLine($"The created matrix has {dTrain.GetNumRows()} rows.");

	Booster bst = new Booster(dTrain);

        bst.SetParameter("booster", "gbtree");
        bst.SetParameter("objective", "binary:logistic");
        bst.SetParameter("max_depth", "1");

        int numBoostRound = 2;
        for (int i = 0; i < numBoostRound; i++) {
                bst.Update(dTrain, i);
        }
	                
        var modelSersArray = bst.DumpModel();
	Console.WriteLine($"Got {modelSersArray.Length} serialized components.");
	for (int i = 0; i < modelSersArray.Length; i++) {
	  Console.WriteLine($"Model {i}: {modelSersArray[i]}.");
	}

        Console.WriteLine("Done!");
    }
}
