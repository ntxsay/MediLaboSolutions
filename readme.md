# Projet 10 : MediLabo Solutions

## Objectif

Développer une solution en microservices avec Docker, ASP.NET Core 9, PostgreSQL, MongoDB et Ocelot

## Installation

- .NET 9 SDK
- Docker Desktop
- IDE Visual Studio / JetBrains Rider
- Outils de gestion de base de données : DBeaver ou autres

## Lancement des services 

Ce projet utilise Docker-Compose pour la gestion des conteneurs et des services.
Présentation et lancement des services par ordre d'importance :

### 1. postgresBackPatient (Base de données)
Base de données (PostgreSQL) dédiée du conteneur **backPatientWebapi**

```
docker-compose up postgresBackPatient
```

### 2. mongoNotesPatient (Base de données)
Base de données (MongoDb) dédié du conteneur **backPatientNoteHistoryWebapi**

```
docker-compose up mongoNotesPatient
```

### 3. mongoNotesPatientExpress (Outils de gestion de base de données)
Interface MongoDB Express pour la gestion de la base de données MongoDB

```
docker-compose up mongoNotesPatientExpress
```

### 4. backPatientWebapi (Service Web)
Projet d'API web en ASP.NET Core 9 : **BackPatient.WebApi**.

Ce microservice expose des endpoints REST pour gérer le dossier du patient.

Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.

```
docker-compose build backPatientWebapi
docker-compose up backPatientWebapi
```

### 5. backPatientNoteHistoryWebapi (Service Web)

Projet d'API web en ASP.NET Core 9 : **BackPatient.NoteHistory.WebApi**.

Ce microservice expose des endpoints REST pour gérer les notes du patient.

Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.

```
docker-compose build backPatientNoteHistoryWebapi
docker-compose up backPatientNoteHistoryWebapi
```

### 6. backPatientRiskAnticipationWebapi (Service Web)

Projet d'API web en ASP.NET Core 9 : **BackPatient.RiskAnticipation.WebApi**.

Ce microservice expose des endpoints REST pour anticiper le niveau de risque qu’un patient développe du diabète.

Il interroge les 2 autres microservices (gestion du patient et gestion de notes) pour calculer et générer le niveau de risque du patient.

Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.

```
docker-compose build backPatientRiskAnticipationWebapi
docker-compose up backPatientRiskAnticipationWebapi
```

### 7. ocelotWebapi (Service Web & Gateway)

Projet d'API web en ASP.NET Core 9 : **OcelotGatewayApi**.

Ce microservice implémente **OCELOT**, une passerelle API (API Gateway) qui sert à faire l’intermédiaire entre les clients et les micro-services.

Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.

```
docker-compose build ocelotWebapi
docker-compose up ocelotWebapi
```

### 8. frontPatientWebApp (Front-End)

Projet web en ASP.NET Core 9 : **FrontPatient.AspNetCore**.

Ce projet d'interface web va permettre de gérer visuellement le dossier des patients ainsi que leur notes.

Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.

```
docker-compose build frontPatientWebApp
docker-compose up frontPatientWebApp
```