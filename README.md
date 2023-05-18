# XGBoostNETTest

A simple .NET wrapper over XGBoost to test hyperparameter effect and
compare against C++ and Python bindings.  To use you'll need the
xgboost library installed somewhere accessible for this code (either
system-wide or in your `LD_LIBRARY_PATH`/`PATH`).

If you're using the devcontainer in this repo, xgboost is installed
automatically via `micromamba`, so you'll need to activate the
relevant environment.

There are two projects, the wrapper and a driver, it's meant to be run
as
```
$ LD_LIBRARY_PATH=/home/vscode/micromamba/envs/xgboostlib/lib docker run --project XGBoostNetLibDriver
```

## TODO
* Document c# and f# clients
* Add benchmarks
