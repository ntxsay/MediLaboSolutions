## Arrêter et libérer toutes les ressources des conteneurs suivants :

```
docker-compose down -v backPatientWebapi
docker-compose down -v backPatientNoteHistoryWebapi
docker-compose down -v backPatientRiskAnticipationWebapi
docker-compose down -v ocelotWebapi
docker-compose down -v frontPatientWebApp
```

## Arrêter le conteneur postgresBackPatient, libérer toutes ses ressources puis le lancer :

```
docker-compose down -v postgresBackPatient
docker-compose up postgresBackPatient
```

## Arrêter le conteneur mongoNotesPatient, libérer toutes ses ressources puis le lancer :

```
docker-compose down -v mongoNotesPatient
docker-compose up mongoNotesPatient
```

## Arrêter le conteneur mongoNotesPatientExpress, libérer toutes ses ressources puis le lancer :

```
docker-compose down -v mongoNotesPatientExpress
docker-compose up mongoNotesPatientExpress
```


## Arrêter le conteneur backPatientWebapi, libérer toutes ses ressources, le builder puis le lancer :

```
docker-compose down -v backPatientWebapi
docker-compose build backPatientWebapi
docker-compose up backPatientWebapi
```

## Arrêter le conteneur backPatientNoteHistoryWebapi, libérer toutes ses ressources, le builder puis le lancer :

```
docker-compose down -v backPatientNoteHistoryWebapi
docker-compose build backPatientNoteHistoryWebapi
docker-compose up backPatientNoteHistoryWebapi
```

## Arrêter le conteneur backPatientRiskAnticipationWebapi, libérer toutes ses ressources, le builder puis le lancer :

```
docker-compose down -v backPatientRiskAnticipationWebapi
docker-compose build backPatientRiskAnticipationWebapi
docker-compose up backPatientRiskAnticipationWebapi
```


## Arrêter le conteneur ocelotWebapi, libérer toutes ses ressources, le builder puis le lancer :

```
docker-compose down -v ocelotWebapi
docker-compose build ocelotWebapi
docker-compose up ocelotWebapi
```

## Arrêter le conteneur frontPatientWebApp, libérer toutes ses ressources, le builder puis le lancer :

```
docker-compose down -v frontPatientWebApp
docker-compose build frontPatientWebApp
docker-compose up frontPatientWebApp
```