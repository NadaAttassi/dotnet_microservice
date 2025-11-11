# E-Commerce Microservices
Access via [website](http://68.221.16.40/)
## Architecture
- **Frontend** (Port 5000) - Interface utilisateur
- **ProductService** (Port 5002) - Gestion des produits avec SQL Server
- **PanierService** (Port 5003) - Gestion du panier avec Redis

## Lancement avec Docker

### 1. Construire et lancer tous les services :
```bash
docker-compose up --build
```

### 2. Accéder à l'application :
- Frontend: http://localhost:5000
- ProductService API: http://localhost:5002/api/products
- PanierService API: http://localhost:5003/api/panier/guest

### 3. Arrêter les services :
```bash
docker-compose down
```

### 4. Supprimer les volumes (données) :
```bash
docker-compose down -v
```

## Développement local

### Prérequis :
- SQL Server (localhost:1433)
- Redis (localhost:6379)

### Lancer individuellement :
```bash
# ProductService
cd ProductService && dotnet run

# PanierService  
cd PanierService && dotnet run

# Frontend
cd Frontend && dotnet run
```

## Base de données
La base `ProductCatalog` et les données de test sont créées automatiquement au démarrage.