ARG LANG=C.UTF-8 LC_ALL=C.UTF-8
ARG ARCH=64  

FROM mcr.microsoft.com/devcontainers/dotnet:0-7.0

RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
     && apt upgrade -y

RUN curl -Ls https://micro.mamba.pm/api/micromamba/linux-64/latest | tar xvj bin/micromamba \
    && mv bin/micromamba /usr/local/bin
# && rm -rf bin

USER vscode

ENV MAMBA_ROOT_PREFIX=/home/vscode/micromamba
RUN micromamba create -n xgboostlib && micromamba install -y libxgboost -c conda-forge -n xgboostlib
RUN micromamba shell init -s bash
