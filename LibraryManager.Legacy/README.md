# LibraryManager - Projet Legacy

## Description

Application WPF de gestion de bibliothèque écrite en **C# 7 / .NET Framework 4.8**.

Ce projet utilise des patterns et syntaxes legacy qui doivent être refactorisés vers **C# 13 / .NET 9**.

## Structure du Projet

```
LibraryManager.Legacy/
├── Models/
│   ├── Book.cs          - Modèle de livre avec propriétés verboses
│   ├── Member.cs        - Modèle de membre
│   └── Loan.cs          - Modèle d'emprunt
├── Services/
│   ├── BookService.cs   - Service de gestion des livres
│   └── MemberService.cs - Service de gestion des membres
├── ViewModels/
│   ├── MainViewModel.cs     - ViewModel principal avec INotifyPropertyChanged manuel
│   └── BookListViewModel.cs - ViewModel de liste de livres
├── Views/
│   ├── MainWindow.xaml      - Fenêtre principale
│   └── MainWindow.xaml.cs   - Code-behind
├── App.xaml
└── App.xaml.cs
```

## Patterns Legacy Utilisés

### Models
- Classes avec champs privés et propriétés get/set verboses
- Constructeurs longs avec de nombreux paramètres
- Méthodes Get...() au lieu de propriétés calculées
- Switch statements classiques
- If/else en cascade

### Services
- Constructeurs sans injection de dépendances
- Boucles foreach et for au lieu de LINQ
- Instanciation directe des dépendances
- List<T> sans collection expressions

### ViewModels
- Implémentation manuelle de INotifyPropertyChanged
- Classe RelayCommand custom
- Champs privés + propriétés avec backing fields
- CommandManager.InvalidateRequerySuggested manuel
- Pas d'attributs source generators

## Objectif du Refactoring

Moderniser ce code vers C# 13 / .NET 9 en utilisant:
- Records
- Primary Constructors
- Collection Expressions
- Switch Expressions
- Pattern Matching
- CommunityToolkit.Mvvm
- Dependency Injection

Voir le fichier `TP_Refactoring_WPF.md` pour les instructions détaillées.


