#!/bin/bash
sleep 15 && /opt/mssql-tools/bin/sqlcmd -S localhost -l 60 -U SA -P $1 -i FullInstall.sql &
/opt/mssql/bin/sqlservr