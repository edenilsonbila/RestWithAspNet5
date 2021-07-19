FROM mysql:5.7.22
EXPOSE 3306
COPY ./CalculadoraRest/db/migrations/ /home/database/
COPY ./CalculadoraRest/db/dataset/ /home/database/
COPY ./CalculadoraRest/CI/init_database.sh/ /docker-entrypoint-initdb.d/init_database.sh