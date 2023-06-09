﻿using System.Collections.Generic;
using System.Reflection;

namespace XGBoostNetLib
{
#if false
    internal delegate void SignatureXGBoostBooster();

    [TlcModule.ComponentKind("BoosterParameterFunction")]
#endif
    internal interface IBoosterParameterFactory
    #if false
    : IComponentFactory<BoosterParameterBase>
    #endif
    {
#pragma warning disable CS0109
        new BoosterParameterBase CreateComponent(
#if false
	IHostEnvironment env
#endif	
	);
#pragma warning restore CS0109
    }

    public abstract class BoosterParameterBase
    {
        private protected static Dictionary<string, string> NameMapping = new Dictionary<string, string>()
        {
           {nameof(OptionsBase.MaximumTreeDepth),               "max_depth"},
           {nameof(OptionsBase.MinimumChildWeight),             "min_child_weight"},
   	#if false
	   {nameof(OptionsBase.SubsampleFraction),              "subsample"},
           {nameof(OptionsBase.SubsampleFrequency),             "subsample_freq"},
           {nameof(OptionsBase.L1Regularization),               "reg_alpha"},
           {nameof(OptionsBase.L2Regularization),               "reg_lambda"},
	#endif
        };

        public BoosterParameterBase(OptionsBase options)
        {
#if false
            Contracts.CheckUserArg(options.MinimumChildWeight >= 0, nameof(OptionsBase.MinimumChildWeight), "must be >= 0.");
            Contracts.CheckUserArg(options.SubsampleFraction > 0 && options.SubsampleFraction <= 1, nameof(OptionsBase.SubsampleFraction), "must be in (0,1].");
            Contracts.CheckUserArg(options.FeatureFraction > 0 && options.FeatureFraction <= 1, nameof(OptionsBase.FeatureFraction), "must be in (0,1].");
            Contracts.CheckUserArg(options.L2Regularization >= 0, nameof(OptionsBase.L2Regularization), "must be >= 0.");
            Contracts.CheckUserArg(options.L1Regularization >= 0, nameof(OptionsBase.L1Regularization), "must be >= 0.");
#endif
            BoosterOptions = options;
        }

        public abstract class OptionsBase : IBoosterParameterFactory
        {
            internal BoosterParameterBase GetBooster() { return null; }

            /// <summary>
            /// The maximum depth of a tree.
            /// </summary>
            /// <value>
            /// 0 means no limit.
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Maximum depth of a tree. 0 means no limit. However, tree still grows by best-first.")]
            public int MaximumTreeDepth = 6;

            /// <summary>
            /// Step size shrinkage used in update to prevents overfitting
            /// </summary>
            /// <value>
            /// 0 means no limit.
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Step size shrinkage used in update to prevents overfitting")]
#if false
            [TlcModule.Range(Min = 0.0, Max = 1.0)]
#endif
            public double LearningRate = 0.3;

	    /// <summary>
            /// The minimum sum of instance weight needed to form a new node.
            /// </summary>
            /// <value>
            /// If the tree partition step results in a leaf node with the sum of instance weight less than <see cref="MinimumChildWeight"/>,
            /// the building process will give up further partitioning. In linear regression mode, this simply corresponds to minimum number
            /// of instances needed to be in each node. The larger, the more conservative the algorithm will be.
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Minimum sum of instance weight(hessian) needed in a child. If the tree partition step results in a leaf " +
                    "node with the sum of instance weight less than min_child_weight, then the building process will give up further partitioning. In linear regression mode, " +
                    "this simply corresponds to minimum number of instances needed to be in each node. The larger, the more conservative the algorithm will be.")]
#if false
            [TlcModule.Range(Min = 0.0)]
#endif
            public double MinimumChildWeight = 1;

#if false
            /// <summary>
            /// The frequency of performing subsampling (bagging).
            /// </summary>
            /// <value>
            /// 0 means disable bagging. N means perform bagging at every N iterations.
            /// To enable bagging, <see cref="SubsampleFraction"/> should also be set to a value less than 1.0.
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Subsample frequency for bagging. 0 means no subsample. "
                + "Specifies the frequency at which the bagging occurs, where if this is set to N, the subsampling will happen at every N iterations." +
                "This must be set with Subsample as this specifies the amount to subsample.")]
            public int SubsampleFrequency = 0;

            /// <summary>
            /// The fraction of training data used for creating trees.
            /// </summary>
            /// <value>
            /// Setting it to 0.5 means that LightGBM randomly picks half of the data points to grow trees.
            /// This can be used to speed up training and to reduce over-fitting. Valid range is (0,1].
            /// </value>
            /// TODO: Fix LightGBM-specific comment
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Subsample ratio of the training instance. Setting it to 0.5 means that LightGBM randomly collected " +
                    "half of the data instances to grow trees and this will prevent overfitting. Range: (0,1].")]
            public double SubsampleFraction = 1;

            /// <summary>
            /// The fraction of features used when creating trees.
            /// </summary>
            /// <value>
            /// If <see cref="FeatureFraction"/> is smaller than 1.0, LightGBM will randomly select fraction of features to train each tree.
            /// For example, if you set it to 0.8, LightGBM will select 80% of features before training each tree.
            /// This can be used to speed up training and to reduce over-fitting. Valid range is (0,1].
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "Subsample ratio of columns when constructing each tree. Range: (0,1].",
                ShortName = "ff")]
            public double FeatureFraction = 1;

            /// <summary>
            /// The L2 regularization term on weights.
            /// </summary>
            /// <value>
            /// Increasing this value could help reduce over-fitting.
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "L2 regularization term on weights, increasing this value will make model more conservative.",
                ShortName = "l2")]
            public double L2Regularization = 0.01;

            /// <summary>
            /// The L1 regularization term on weights.
            /// </summary>
            /// <value>
            /// Increasing this value could help reduce over-fitting.
            /// </value>
            [Argument(ArgumentType.AtMostOnce,
                HelpText = "L1 regularization term on weights, increase this value will make model more conservative.",
                ShortName = "l1")]
            public double L1Regularization = 0;
#endif

#if false
            BoosterParameterBase IComponentFactory<BoosterParameterBase>.CreateComponent(IHostEnvironment env)
                => BuildOptions();
#endif

            BoosterParameterBase IBoosterParameterFactory.CreateComponent(
#if false
	    IHostEnvironment env
#endif
	    )
                => BuildOptions();

            internal abstract BoosterParameterBase BuildOptions();
        }

        internal void UpdateParameters(Dictionary<string, object> res)
        {
            FieldInfo[] fields = BoosterOptions.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<ArgumentAttribute>(false);

                if (attribute == null)
                    continue;

                var name = NameMapping.ContainsKey(field.Name) ? NameMapping[field.Name] : XGBoostInterfaceUtils.GetOptionName(field.Name);
                res[name] = field.GetValue(BoosterOptions);
            }
        }

        /// <summary>
        /// Create <see cref="IBoosterParameterFactory"/> for supporting legacy infra built upon <see cref="IComponentFactory"/>.
        /// </summary>
        internal abstract IBoosterParameterFactory BuildFactory();
        internal abstract string BoosterName { get; }

        private protected OptionsBase BoosterOptions;
    }
    
    /// <summary>
    /// Gradient boosting decision tree.
    /// </summary>
    /// <remarks>
    /// For details, please see <a href="https://en.wikipedia.org/wiki/Gradient_boosting#Gradient_tree_boosting">gradient tree boosting</a>.
    /// </remarks>
    public sealed class GradientBooster : BoosterParameterBase
    {
        internal const string Name = "gbtree";
        internal const string FriendlyName = "Tree Booster";

        /// <summary>
        /// The options for <see cref="GradientBooster"/>, used for setting <see cref="Booster"/>.
        /// </summary>
#if false
        [TlcModule.Component(Name = Name, FriendlyName = FriendlyName, Desc = "Traditional Gradient Boosting Decision Tree.")]
#endif
        public sealed class Options : OptionsBase
        {
            internal override BoosterParameterBase BuildOptions() => new GradientBooster(this);
        }

        internal GradientBooster(Options options)
            : base(options)
        {
        }

        internal override IBoosterParameterFactory BuildFactory() => BoosterOptions;

        internal override string BoosterName => Name;
    }

    /// <summary>
    /// DART booster (Dropouts meet Multiple Additive Regression Trees)
    /// </summary>
    /// <remarks>
    /// For details, please see <a href="https://arxiv.org/abs/1505.01866">here</a>.
    /// </remarks>
    public sealed class DartBooster : BoosterParameterBase
    {
        internal const string Name = "dart";
        internal const string FriendlyName = "Tree Dropout Tree Booster";

        /// <summary>
        /// The options for <see cref="DartBooster"/>, used for setting <see cref="Booster"/>.
        /// </summary>
#if false
        [TlcModule.Component(Name = Name, FriendlyName = FriendlyName, Desc = "Dropouts meet Multiple Additive Regresion Trees. See https://arxiv.org/abs/1505.01866")]
#endif
        public sealed class Options : OptionsBase
        {
            static Options()
            {
                // Add additional name mappings
                NameMapping.Add(nameof(TreeDropFraction), "rate_drop");
                NameMapping.Add(nameof(SkipDropFraction), "skip_drop");
            }

            /// <summary>
            /// The dropout rate, i.e. the fraction of previous trees to drop during the dropout.
            /// </summary>
            /// <value>
            /// Valid range is [0,1].
            /// </value>
            [Argument(ArgumentType.AtMostOnce, HelpText = "The drop ratio for trees. Range:(0,1).")]
#if false
            [TlcModule.Range(Inf = 0.0, Max = 1.0)]
#endif
            public double TreeDropFraction = 0.0;

            /// <summary>
            /// The probability of skipping the dropout procedure during a boosting iteration.
            /// </summary>
            [Argument(ArgumentType.AtMostOnce, HelpText = "Probability for not dropping in a boosting round.")]
#if false
            [TlcModule.Range(Inf = 0.0, Max = 1.0)]
#endif
            public double SkipDropFraction = 0.0;

#if false
            /// <summary>
            /// Whether to enable uniform drop.
            /// Allowed values: "uniform" or "weighted"
            /// </summary>
            [Argument(ArgumentType.AtMostOnce, HelpText = "True will enable uniform drop.")]
            public bool UniformDrop = false;
#endif

            /// <summary>
            /// Type of sampling algorithm
            /// </summary>
            [Argument(ArgumentType.AtMostOnce, HelpText = "Type of sampling algorithm")]
#if false
            [TlcModule.SweepableDiscreteParam("SampleType", new object[] { "uniform", "weighted" })]
#endif
            public string SampleType = "uniform";

            /// <summary>
            /// Type of normalization algorithm
            /// </summary>
            [Argument(ArgumentType.AtMostOnce, HelpText = "Type of normalization algorithm")]
#if false
            [TlcModule.SweepableDiscreteParam("NormalizeType", new object[] { "tree", "forest" })]
#endif
            public string NormalizeType = "tree";

            /// <summary>
            /// Whether at least one tree is always dropped during the dropout
            /// </summary>
            [Argument(ArgumentType.AtMostOnce, HelpText = "Whether at least one tree is always dropped during the dropout")]
#if false
            [TlcModule.SweepableDiscreteParam("OneDrop", new object[] { true, false })]
#endif
            public bool OneDrop = false;

            internal override BoosterParameterBase BuildOptions() => new DartBooster(this);
        }

        internal DartBooster(Options options)
            : base(options)
        {
#if false
            Contracts.CheckUserArg(options.TreeDropFraction > 0 && options.TreeDropFraction < 1, nameof(options.TreeDropFraction), "must be in (0,1).");
            Contracts.CheckUserArg(options.SkipDropFraction >= 0 && options.SkipDropFraction < 1, nameof(options.SkipDropFraction), "must be in [0,1).");
#endif
            BoosterOptions = options;
        }

        internal override IBoosterParameterFactory BuildFactory() => BoosterOptions;
        internal override string BoosterName => Name;
    }

}

