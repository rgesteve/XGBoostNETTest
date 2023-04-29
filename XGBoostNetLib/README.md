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
