@echo off
echo Test du PanierService...

echo.
echo 1. Récupérer panier vide:
curl -X GET http://localhost:5003/api/panier/user123

echo.
echo.
echo 2. Ajouter un laptop:
curl -X POST http://localhost:5003/api/panier/user123/items ^
  -H "Content-Type: application/json" ^
  -d "{\"ProductId\":1,\"ProductName\":\"Laptop\",\"Price\":999.99,\"Quantity\":1}"

echo.
echo.
echo 3. Ajouter un smartphone:
curl -X POST http://localhost:5003/api/panier/user123/items ^
  -H "Content-Type: application/json" ^
  -d "{\"ProductId\":2,\"ProductName\":\"Smartphone\",\"Price\":699.99,\"Quantity\":2}"

echo.
echo.
echo 4. Voir le panier complet:
curl -X GET http://localhost:5003/api/panier/user123

echo.
echo.
echo 5. Supprimer le laptop:
curl -X DELETE http://localhost:5003/api/panier/user123/items/1

echo.
echo.
echo 6. Voir le panier après suppression:
curl -X GET http://localhost:5003/api/panier/user123

pause