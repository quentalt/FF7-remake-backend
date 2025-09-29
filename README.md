# 🎮 Quiz FF7 Remake - Backend

Un projet backend développé en **C#** par deux passionnés, permettant de créer et gérer un quiz autour de **Final Fantasy VII Remake**.  
Ce projet fournit une API pour gérer les questions, réponses, scores et utilisateurs.

---

## 🚀 Fonctionnalités

- 📋 Gestion des **questions** (CRUD)  
- ✅ Vérification des **réponses**  
- 🏆 Système de **scores** et classement  
- 🔒 Authentification  

---

## 🛠️ Technologies utilisées

- **Langage** : C# (.NET 9)  
- **Framework** : ASP.NET Core Web API  
- **Base de données** : PostgreSQL avec Entity Framework Core  
- **Tests** : xUnit / NUnit  
- **Documentation** : Swagger / OpenAPI  

---

## 📂 Structure du projet

/FF7-Remake

├── **Controllers**  – Endpoints de l’API  
├── **DBContext**    -Classe de contexte pour EF Core
├── **Models**       – Entités (User, Question, Answer, Score…)  
├── **DTOs**         – Objets de transfert de données  
├── **Migrations**   - migrations 
├── **Services**     – Logique métier (quiz, utilisateurs, scores…) 
└── **Program.cs**   – Point d’entrée
