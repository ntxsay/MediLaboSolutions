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
Base de données (PostgreSQL) dédiée du conteneur **backPatientWebapi**.

Il utilise le fichier **db-init\init-multiple-db.sql** pour créer les bases de données nommées **DbAuth** *(Pour gérer les utilisateurs de l'application via Microsoft Identity)* et **DbPatient** *(Pour gérer les dossiers des patients)*.

```
docker-compose up postgresBackPatient
```

*Remarque : Docker peut démarrer automatiquement ce conteneur à son lancement, pensez à vérifier avant de lancer la commande.*

### 2. mongoNotesPatient (Base de données)
Base de données (MongoDb) dédié du conteneur **backPatientNoteHistoryWebapi**

```
docker-compose up mongoNotesPatient
```

*Remarque : Docker peut démarrer automatiquement ce conteneur à son lancement, pensez à vérifier avant de lancer la commande.*


### 3. mongoNotesPatientExpress (Outils de gestion de base de données)
Interface MongoDB Express pour la gestion de la base de données MongoDB

```
docker-compose up mongoNotesPatientExpress
```

*Remarque : Docker peut démarrer automatiquement ce conteneur à son lancement, pensez à vérifier avant de lancer la commande.*


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

Il interroge les deux autres microservices (gestion du patient et gestion de notes) pour calculer et générer le niveau de risque du patient.

Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.

```
docker-compose build backPatientRiskAnticipationWebapi
docker-compose up backPatientRiskAnticipationWebapi
```

### 7. ocelotWebapi (Service Web & Gateway)

Projet d'API web en ASP.NET Core 9 : **OcelotGatewayApi**.

Ce microservice implémente **Microsoft Identity** et **OCELOT**, une passerelle API (API Gateway) qui sert à faire l’intermédiaire entre les clients et les micro-services.

Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.

```
docker-compose build ocelotWebapi
docker-compose up ocelotWebapi
```

### 8. frontPatientWebApp (Front-End)

Projet web en ASP.NET Core 9 : **FrontPatient.AspNetCore**.

Ce projet d'interface web va permettre de gérer via une interface utilisateur le dossier des patients ainsi que leur notes.

*Pour ce projet, docker devra d'abord construire l'image du projet avant de lancer le conteneur.*

```
docker-compose build frontPatientWebApp
docker-compose up frontPatientWebApp
```

*Remarque : Si l'url de l'application est le suivant : http://0.0.0.0:8086/ vous devrez le remplacer par http://localhost:8086/*


## Sécurité

L'utilisateur doit être authentifié pour utiliser l'application.

Le système d'authentification est géré par **Microsoft.AspNetCore.Identity** et implémenté dans le projet **OcelotGatewayApi**, qui gère aussi la passerelle entre les différents microservices.

La méthode d'authentification est essentieelement basée sur le **JWT** (JSON Web Token) qui correspont à l'architecture en microservices de ce projet cepdendant étant donné que le projet implémente un client web *(ASP.NET Core 9)*.

### Connexion

Etant données que toutes les routes permettant l'affichage et la gestion des données des patients sont sécurisées, vous serez automatiquement redirigé vers la page de connexion.

Dans le cadre de ce projet, il n'est pas permis aux utilisateurs quelqu'ils soient de pouvoir créer de comptes, à la place un système de peuplement d'utilisateurs a été implémenté.

Ci-dessous les identifiants de l'utilisateur :

- Nom d'utilisateur : **admin@mediclabo.fr**
- Mot de passe : **P@ssword123**

## Green Code

Voici quelques recommendations green code pour le projet :

- Mettre en cache les données,
- Ne récupérer que les données nécessaires
- Éviter de prévoir trop de cas extrêmes inutiles et respecter le principe de responsabilité unique permet de garder un code plus simple, plus performant et moins énergivore.
- Ne pas retenir les fonctionnalités non essentielles
- Optimiser les requêtes aux bases de données (index)
- Choisir les technologies les plus adaptées



## Maquette:

![image](Medicalo.png)