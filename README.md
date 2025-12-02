# 🛒 E-Commerce Microservices

Une application de démonstration e-commerce construite avec une architecture microservices en **.NET 9.0**.

🔗 **Démo en ligne** : [http://68.221.16.40/](http://68.221.16.40/)

---

## 🏗 Architecture

Le projet est composé de trois services principaux interconnectés :

| Service | Port | Description | Tech Stack |
|---------|------|-------------|------------|
| **Frontend** | `5000` | Interface utilisateur web (Razor Pages) | ASP.NET Core, HTTP Client |
| **ProductService** | `5002` | API de gestion du catalogue produits | Web API, EF Core, SQL Server |
| **PanierService** | `5003` | API de gestion du panier d'achat | Web API, Redis |

### Infrastructure de Données
- **SQL Server 2022** : Stockage persistant pour le catalogue de produits.
- **Redis 7** : Stockage en mémoire rapide pour les sessions de panier.

---

## 🛠 Technologies Utilisées

- **Framework** : .NET 9.0
- **Conteneurisation** : Docker & Docker Compose
- **Base de données** : Microsoft SQL Server, Redis
- **CI/CD** : GitHub Actions (Déploiement automatique sur Azure VM)

---

## 🚀 Installation et Démarrage

### Option 1 : Avec Docker (Recommandé)

C'est la méthode la plus simple pour lancer tout l'environnement.

1. **Cloner le projet**
   ```bash
   git clone https://github.com/NadaAttassi/dotnet_microservice
   cd dotnet_microservice
   ```

2. **Lancer les services**
   ```bash
   docker-compose up --build
   ```
   *Cela va construire les images, lancer SQL Server, Redis et les 3 microservices.*

3. **Accéder à l'application**
   - **Frontend** : [http://localhost:5000](http://localhost:5000)
   - **Swagger ProductService** : [http://localhost:5002/swagger](http://localhost:5002/swagger) (si activé)
   - **Swagger PanierService** : [http://localhost:5003/swagger](http://localhost:5003/swagger) (si activé)

4. **Arrêter les services**
   ```bash
   docker-compose down
   # Pour supprimer aussi les volumes (données BDD) :
   docker-compose down -v
   ```

### Option 2 : Développement Local (Sans Docker)

**Prérequis :**
- .NET SDK 9.0
- Une instance SQL Server locale (Port 1433)
- Une instance Redis locale (Port 6379)

**Commandes de lancement individuel :**

1. **ProductService**
   ```bash
   cd ProductService
   dotnet run
   ```
   *Assurez-vous que la chaîne de connexion dans `appsettings.json` pointe vers votre SQL Server local.*

2. **PanierService**
   ```bash
   cd PanierService
   dotnet run
   ```
   *Assurez-vous que Redis est accessible.*

3. **Frontend**
   ```bash
   cd Frontend
   dotnet run
   ```

---

## 📡 Endpoints Principaux

### ProductService (`:5002`)
- `GET /api/products` : Liste tous les produits.
- `GET /api/products/{id}` : Détails d'un produit.

### PanierService (`:5003`)
- `GET /api/panier/{id}` : Récupère le panier d'un utilisateur (ou guest).
- `POST /api/panier` : Met à jour le panier.
- `DELETE /api/panier/{id}` : Vide le panier.

---

## 🔄 CI/CD & Déploiement

Le projet utilise **GitHub Actions** pour l'intégration et le déploiement continu.
Le workflow est défini dans `.github/workflows/deploy.yml`.

**Flux de déploiement :**
1. Push sur la branche `main`.
2. Checkout du code.
3. Copie des fichiers vers la VM Azure via SCP.
4. Connexion SSH à la VM.
5. Redémarrage des conteneurs via `docker-compose up -d --build`.

---

## 📂 Structure du Projet

```
dotnet_microservice/
├── Frontend/           # Application Web ASP.NET Core
├── PanierService/      # Microservice Panier (Redis)
├── ProductService/     # Microservice Produits (SQL Server)
├── docker-compose.yml  # Orchestration des conteneurs
├── .github/            # Workflows CI/CD
└── README.md           # Documentation
```
