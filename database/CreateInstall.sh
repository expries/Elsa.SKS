#!/bin/bash
# when using WSL(S) -> install dos2unix to 
# dos2unix create_aks_prod.sh

env=$1

echo "script will be executed under scope environment '$env'"

echo '' > FullInstall.sql
cat $PWD/createDatabase.sql >> FullInstall.sql