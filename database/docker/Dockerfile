FROM mcr.microsoft.com/mssql/server:2019-latest

COPY ["database/FullInstall.sql", "FullInstall.sql"]
COPY ["database/docker/entrypoint.sh", "entrypoint.sh"]

CMD /bin/bash ./entrypoint.sh ${SA_PASSWORD}
