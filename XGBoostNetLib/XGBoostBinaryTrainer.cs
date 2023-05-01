using System.Collections.Generic;

namespace XGBoostNetLib
{
#if false
    /// <summary>
    /// Model parameters for <see cref="XGBoostBinaryTrainer"/>.
    /// </summary>
    public sealed class XGBoostBinaryModelParameters : TreeEnsembleModelParametersBasedOnRegressionTree
    {
        internal const string LoaderSignature = "XGBoostBinaryExec";
        internal const string RegistrationName = "XGBoostBinaryPredictor";

        private static VersionInfo GetVersionInfo()
        {
            // REVIEW: can we decouple the version from FastTree predictor version ?
            return new VersionInfo(
                modelSignature: "XGBBINCL",
                // verWrittenCur: 0x00010001, // Initial
                // verWrittenCur: 0x00010002, // _numFeatures serialized
                // verWrittenCur: 0x00010003, // Ini content out of predictor
                //verWrittenCur: 0x00010004, // Add _defaultValueForMissing
                verWrittenCur: 0x00010005, // Categorical splits.
                verReadableCur: 0x00010004,
                verWeCanReadBack: 0x00010001,
                loaderSignature: LoaderSignature,
                loaderAssemblyName: typeof(XGBoostBinaryModelParameters).Assembly.FullName);
        }

        private protected override uint VerNumFeaturesSerialized => 0x00010002;
        private protected override uint VerDefaultValueSerialized => 0x00010004;
        private protected override uint VerCategoricalSplitSerialized => 0x00010005;
        private protected override PredictionKind PredictionKind => PredictionKind.BinaryClassification;

        internal XGBoostBinaryModelParameters(IHostEnvironment env, InternalTreeEnsemble trainedEnsemble, int featureCount, string innerArgs)
            : base(env, RegistrationName, trainedEnsemble, featureCount, innerArgs)
        {
        }

        private XGBoostBinaryModelParameters(IHostEnvironment env, ModelLoadContext ctx)
            : base(env, RegistrationName, ctx, GetVersionInfo())
        {
        }

        private protected override void SaveCore(ModelSaveContext ctx)
        {
            base.SaveCore(ctx);
            ctx.SetVersionInfo(GetVersionInfo());
        }

        internal static IPredictorProducing<float> Create(IHostEnvironment env, ModelLoadContext ctx)
        {
            Contracts.CheckValue(env, nameof(env));
            env.CheckValue(ctx, nameof(ctx));
            ctx.CheckAtModel(GetVersionInfo());
            var predictor = new XGBoostBinaryModelParameters(env, ctx);
            ICalibrator calibrator;
            ctx.LoadModelOrNull<ICalibrator, SignatureLoadModel>(env, out calibrator, @"Calibrator");
            if (calibrator == null)
                return predictor;
            return new ValueMapperCalibratedModelParameters<XGBoostBinaryModelParameters, ICalibrator>(env, predictor, calibrator);
        }
    }
#endif

    /// <summary>
    /// The <see cref="IEstimator{TTransformer}"/> for training a boosted decision tree binary classification model using XGBoost.
    /// </summary>
    public sealed class XGBoostBinaryTrainer
    : XGBoostTrainerBase<XGBoostBinaryTrainer.Options
#if false
      , float
      , BinaryPredictionTransformer<CalibratedModelParametersBase<XGBoostBinaryModelParameters, PlattCalibrator>>
      , CalibratedModelParametersBase<XGBoostBinaryModelParameters, PlattCalibrator>
#endif
      >
    {
        internal new const string UserNameValue = "XGBoost Binary Classifier";
        internal new const string LoadNameValue = "XGBoostBinary";
        internal const string ShortName = "XGBoostB";
        internal new const string Summary = "Train a XGBoost binary classification model.";

#if false
        private protected override PredictionKind PredictionKind => PredictionKind.BinaryClassification;
#endif

        public sealed class Options : OptionsBase
        {
            public enum EvaluateMetricType
            {
                None,
                Default,
                Logloss,
                Error,
                AreaUnderCurve,
            };

            /// <summary>
            /// Whether training data is unbalanced.
            /// </summary>
            [Argument(ArgumentType.AtMostOnce, HelpText = "Use for binary classification when training data is not balanced.", ShortName = "us")]
            public bool UnbalancedSets = false;

            /// <summary>
            /// Controls the balance of positive and negative weights in <see cref="XGBoostBinaryTrainer"/>.
            /// </summary>
            /// <value>
            /// This is useful for training on unbalanced data. A typical value to consider is sum(negative cases) / sum(positive cases).
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Control the balance of positive and negative weights, useful for unbalanced classes." +
                " A typical value to consider: sum(negative cases) / sum(positive cases).",
                ShortName = "ScalePosWeight")]
            public double WeightOfPositiveExamples = 1;

            /// <summary>
            /// Parameter for the sigmoid function.
            /// </summary>
            [Argument(ArgumentType.AtMostOnce, HelpText = "Parameter for the sigmoid function.", ShortName = "sigmoid")]
#if false
            [TGUI(Label = "Sigmoid", SuggestedSweeps = "0.5,1")]
#endif
            public double Sigmoid = 0.5;

            /// <summary>
            /// Determines what evaluation metric to use.
            /// </summary>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Evaluation metrics.",
                ShortName = "em")]
            public EvaluateMetricType EvaluationMetric = EvaluateMetricType.Logloss;

#if false
            static Options()
            {
                NameMapping.Add(nameof(EvaluateMetricType), "metric");
                NameMapping.Add(nameof(EvaluateMetricType.None), "None");
                NameMapping.Add(nameof(EvaluateMetricType.Default), "");
                NameMapping.Add(nameof(EvaluateMetricType.Logloss), "binary_logloss");
                NameMapping.Add(nameof(EvaluateMetricType.Error), "binary_error");
                NameMapping.Add(nameof(EvaluateMetricType.AreaUnderCurve), "auc");
                NameMapping.Add(nameof(WeightOfPositiveExamples), "scale_pos_weight");
            }

            internal override Dictionary<string, object> ToDictionary(IHost host)
            {
                var res = base.ToDictionary(host);
                res[GetOptionName(nameof(UnbalancedSets))] = UnbalancedSets;
                res[GetOptionName(nameof(WeightOfPositiveExamples))] = WeightOfPositiveExamples;
                res[GetOptionName(nameof(Sigmoid))] = Sigmoid;
                res[GetOptionName(nameof(EvaluateMetricType))] = GetOptionName(EvaluationMetric.ToString());

                return res;
            }
	    #endif
        }

        public XGBoostBinaryTrainer(/* IHostEnvironment env, */ Options options)
             : base(
#if false
	     env, LoadNameValue,
#endif
	     options
#if false
	     , TrainerUtils.MakeBoolScalarLabel(options.LabelColumnName)
#endif
	     )
        {
	System.Console.WriteLine("In binary trainer constructor");
#if false
            Contracts.CheckUserArg(options.Sigmoid > 0, nameof(Options.Sigmoid), "must be > 0.");
            Contracts.CheckUserArg(options.WeightOfPositiveExamples > 0, nameof(Options.WeightOfPositiveExamples), "must be > 0.");
#endif
        }

#if false
        /// <summary>
        /// Initializes a new instance of <see cref="XGBoostBinaryTrainer"/>
        /// </summary>
        /// <param name="env">The private instance of <see cref="IHostEnvironment"/>.</param>
        /// <param name="labelColumnName">The name of The label column.</param>
        /// <param name="featureColumnName">The name of the feature column.</param>
        /// <param name="exampleWeightColumnName">The name of the example weight column (optional).</param>
        /// <param name="numberOfLeaves">The number of leaves to use.</param>
        /// <param name="minimumExampleCountPerLeaf">The minimal number of data points allowed in a leaf of the tree, out of the subsampled data.</param>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="numberOfIterations">Number of iterations.</param>
        internal XGBoostBinaryTrainer(IHostEnvironment env,
            string labelColumnName = DefaultColumnNames.Label,
            string featureColumnName = DefaultColumnNames.Features,
            string exampleWeightColumnName = null,
            int? numberOfLeaves = null,
            int? minimumExampleCountPerLeaf = null,
            double? learningRate = null,
            int numberOfIterations = Defaults.NumberOfIterations)
            : this(env,
                  new Options()
                  {
                      LabelColumnName = labelColumnName,
                      FeatureColumnName = featureColumnName,
                      ExampleWeightColumnName = exampleWeightColumnName,
                      NumberOfLeaves = numberOfLeaves,
                      MinimumExampleCountPerLeaf = minimumExampleCountPerLeaf,
                      LearningRate = learningRate,
                      NumberOfIterations = numberOfIterations
                  })
        {
        }

        private protected override CalibratedModelParametersBase<XGBoostBinaryModelParameters, PlattCalibrator> CreatePredictor()
        {
            Host.Check(TrainedEnsemble != null, "The predictor cannot be created before training is complete");
            var innerArgs = XGBoostInterfaceUtils.JoinParameters(base.GbmOptions);
            var pred = new XGBoostBinaryModelParameters(Host, TrainedEnsemble, FeatureCount, innerArgs);
            var cali = new PlattCalibrator(Host, -XGBoostTrainerOptions.Sigmoid, 0);
            return new FeatureWeightsCalibratedModelParameters<XGBoostBinaryModelParameters, PlattCalibrator>(Host, pred, cali);
        }

        private protected override void CheckDataValid(IChannel ch, RoleMappedData data)
        {
            Host.AssertValue(ch);
            base.CheckDataValid(ch, data);
            var labelType = data.Schema.Label.Value.Type;
            if (!(labelType is BooleanDataViewType || labelType is KeyDataViewType || labelType == NumberDataViewType.Single))
            {
                throw ch.ExceptParam(nameof(data),
                    $"Label column '{data.Schema.Label.Value.Name}' is of type '{labelType.RawType}', but must be unsigned int, boolean or float.");
            }
        }
#endif

        private protected override void CheckAndUpdateParametersBeforeTraining(
#if false
	IChannel ch, RoleMappedData data, float[] labels, int[] groups
#endif
	)
            => GbmOptions["objective"] = "binary:logistic";

#if false
        private protected override SchemaShape.Column[] GetOutputColumnsCore(SchemaShape inputSchema)
        {
            return new[]
            {
                new SchemaShape.Column(DefaultColumnNames.Score, SchemaShape.Column.VectorKind.Scalar, NumberDataViewType.Single, false, new SchemaShape(AnnotationUtils.GetTrainerOutputAnnotation())),
                new SchemaShape.Column(DefaultColumnNames.Probability, SchemaShape.Column.VectorKind.Scalar, NumberDataViewType.Single, false, new SchemaShape(AnnotationUtils.GetTrainerOutputAnnotation(true))),
                new SchemaShape.Column(DefaultColumnNames.PredictedLabel, SchemaShape.Column.VectorKind.Scalar, BooleanDataViewType.Instance, false, new SchemaShape(AnnotationUtils.GetTrainerOutputAnnotation()))
            };
        }

        private protected override BinaryPredictionTransformer<CalibratedModelParametersBase<XGBoostBinaryModelParameters, PlattCalibrator>>
            MakeTransformer(CalibratedModelParametersBase<XGBoostBinaryModelParameters, PlattCalibrator> model, DataViewSchema trainSchema)
         => new BinaryPredictionTransformer<CalibratedModelParametersBase<XGBoostBinaryModelParameters, PlattCalibrator>>(Host, model, trainSchema, FeatureColumn.Name);

        /// <summary>
        /// Trains a <see cref="XGBoostBinaryTrainer"/> using both training and validation data, returns
        /// a <see cref="BinaryPredictionTransformer{CalibratedModelParametersBase}"/>.
        /// </summary>
        public BinaryPredictionTransformer<CalibratedModelParametersBase<XGBoostBinaryModelParameters, PlattCalibrator>> Fit(IDataView trainData, IDataView validationData)
            => TrainTransformer(trainData, validationData);
#endif
    }
}