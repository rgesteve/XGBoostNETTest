# Architecture of bindings

## Hyper-parameters

A crucial aspect of using the XGBoost library is management of the
parameters of the algorithm.  Even the specific ML task that is to be
carried out is specified through a parameter (`objective`), so it's
not surprising that finer-details of the library operation are as well.

Parameters are roughly divided in four categories:
* Global
* Task-specific
* Relevant to the booster
* Booster-specific

### Implementation details

The hyperparameters are split in two parallel inheritance chains, one
relevant to the task and the other to the booster.  What makes this
design somewhat idiosyncratic is that these chains are each embedded
in *another* inheritance chain.  Syntactically, this provides some
uniformity, as the parameters can be passed as:
```c#
var pipeline = new XGBoostBinaryTrainer(new XGBoostBinaryTrainer.Options() {
  UnbalancedSets = true,
  WeightOfPositiveExamples = 0.5,
  Booster = new DartBooster.Options()
  {
      TreeDropFraction = 0.124
  }
});
```

Notice the use of the "embedded" class `Options` for both task and
booster.

The actual options are POD fields marked with the `Argument` attribute
(which allows them to be used by the AutoML system in ML.NET).  Before
calling the XGBoost training code, the argumetns are collected into a
.NET `Dictionary` and then used to configure the algorithm.
Collecting the arguments from their C# representation is not done
uniformly.  For example, Booster arguments are collected via
reflection, whereas arguments in the task-specific inheritance chain
are hardcoded.

To collect arguments, global, generic booster and booster-specific
arguments are consolidated in the
`*TrainerBase.OptionBase.ToDictionary` method.  As mentioned above,
options are consolidated in a `Dictionary` called `GbmOptions`.
Task-specific options are included in this dictionary when the virtual
method `*TrainerBase.CheckAndUpdateParametersBeforeTraining`.

