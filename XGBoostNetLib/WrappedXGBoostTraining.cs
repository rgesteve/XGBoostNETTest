﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

#if false
namespace XGBoostNetLib
{
    /// <summary>
    /// Helpers to train a booster with given parameters.
    /// </summary>
    internal static class WrappedXGBoostTraining
    {
        /// <summary>
        /// Train and return a booster.
        /// </summary>
        public static Booster Train(IHost host, IChannel ch, IProgressChannel pch,
            Dictionary<string, object> parameters, DMatrix dtrain
#if false
            , Dataset dvalid = null, int numIteration = 100,
            bool verboseEval = true, int earlyStoppingRound = 0
#endif
            )
        {
#if false
            // create Booster.
            Booster bst = new Booster(parameters, dtrain, dvalid);

            // Disable early stopping if we don't have validation data.
            if (dvalid == null && earlyStoppingRound > 0)
            {
                earlyStoppingRound = 0;
                ch.Warning("Validation dataset not present, early stopping will be disabled.");
            }

            int bestIter = 0;
            double bestScore = double.MaxValue;
            double factorToSmallerBetter = 1.0;

            var metric = (string)parameters["metric"];
            if (earlyStoppingRound > 0 && (metric == "auc" || metric == "ndcg" || metric == "map"))
                factorToSmallerBetter = -1.0;

            const int evalFreq = 50;

            var metrics = new List<string>() { "Iteration" };
            var units = new List<string>() { "iterations" };

            if (verboseEval)
            {
                ch.Assert(parameters.ContainsKey("metric"));
                metrics.Add("Training-" + parameters["metric"]);
                if (dvalid != null)
                    metrics.Add("Validation-" + parameters["metric"]);
            }

            var header = new ProgressHeader(metrics.ToArray(), units.ToArray());

            int iter = 0;
            double trainError = double.NaN;
            double validError = double.NaN;
            pch.SetHeader(header, e =>
            {
                e.SetProgress(0, iter, numIteration);
                if (verboseEval)
                {
                    e.SetProgress(1, trainError);
                    if (dvalid != null)
                        e.SetProgress(2, validError);
                }
            });
            for (iter = 0; iter < numIteration; ++iter)
            {
                host.CheckAlive();
                if (bst.Update())
                    break;

                if (earlyStoppingRound > 0)
                {
                    validError = bst.EvalValid();
                    if (validError * factorToSmallerBetter < bestScore)
                    {
                        bestScore = validError * factorToSmallerBetter;
                        bestIter = iter;
                    }
                    if (iter - bestIter >= earlyStoppingRound)
                    {
                        ch.Info($"Met early stopping, best iteration: {bestIter + 1}, best score: {bestScore / factorToSmallerBetter}");
                        break;
                    }
                }
                if ((iter + 1) % evalFreq == 0)
                {
                    if (verboseEval)
                    {
                        trainError = bst.EvalTrain();
                        if (dvalid == null)
                            pch.Checkpoint(new double?[] { iter + 1, trainError });
                        else
                        {
                            if (earlyStoppingRound == 0)
                                validError = bst.EvalValid();
                            pch.Checkpoint(new double?[] { iter + 1,
                                trainError, validError });
                        }
                    }
                    else
                        pch.Checkpoint(new double?[] { iter + 1 });
                }
            }
            // Set the BestIteration.
            if (iter != numIteration && earlyStoppingRound > 0)
            {
                bst.BestIteration = bestIter + 1;
            }
            return bst;
#else

            Booster bst = new Booster(dtrain);

            // TODO: Pass all configuration parameters to the booster.
            bst.SetParameter("objective", "reg:squarederror");
            //bst.SetParameter("tree_method", "exact");
            bst.SetParameter("tree_method", "hist");
            bst.SetParameter("max_depth", "2");
            //bst.SetParameter("nthread", "1");
            /*
bst.SetParameter("booster", "gbtree");
bst.SetParameter("max_depth", "3");
bst.SetParameter("min_child_weight", "1000");
bst.SetParameter("seed", "0");
*/
            // bst.SetParameter("nthread", "1");

#if false
            XGBoosterSetParam(booster, "booster", "gbtree");
            XGBoosterSetParam(booster, "objective", "reg:squarederror");
            XGBoosterSetParam(booster, "max_depth", "1");
            XGBoosterSetParam(booster, "base_score", "0")
#endif

            var bstConfig = bst.DumpConfig();
            System.Console.WriteLine($"The starting configuration of the booster is {bstConfig}..");

            System.Console.WriteLine("Starting training loop");
            int numBoostRound = 5;
            for (int i = 0; i < numBoostRound; i++)
            {
                bst.Update(dtrain, i);
            }
            System.Console.WriteLine($"------- Dumping the model into an internal regression tree ---------------");
            bst.DumpModel();

            return bst;
#endif
        }
    }
}
#endif
