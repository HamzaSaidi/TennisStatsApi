# TennisStatsApi
🚀 Lancement de l'application
Cloner le projet :
       
        git clone https://github.com/votre-utilisateur/TennisStatsApi.git

Restaurer les dépendances :

        dotnet restore

Lancer l’application :
 
          dotnet run

# Structure du projet
            TennisStatsApi/
            │
            ├── Controllers/          # Endpoints API
            ├── Services/             # Logique métier
            ├── Repository/           # Accès aux données JSON
            ├── Models/               # Modèles de données
            ├── Helpers/              # Méthodes utilitaires (extensions, calculs)
            ├── Middelwares/          # middleware pour intercepter les requêtes(global exceptionhandler)
            ├── Program.cs            # Point d'entrée de l'application
            ├── TennisStatsApi.csproj
            └── README.md  
            TennisStatsApiTests/       # projets de tests
            │
            ├── Controllers/          # Endpoints API Test
            ├── Services/             # Logique métier Test
            └──Repository/            # repository  Test
            azure-build-deploy.yml    # azure Ci/Cd se déclenche apres chaque commit sur la branche develop
