Les 10 Commandements du Logging

1. **Ne jamais logger de données sensibles**
```csharp
// ❌ DANGEREUX
_logger.LogInformation("Mot de passe: {Password}", password);

// ✅ SÉCURISÉ
_logger.LogInformation("Authentification réussie pour {UserId}", userId);
```

2. **Utiliser le bon niveau de log**
```csharp
// ❌ MAUVAIS - Information n'est pas une erreur
_logger.LogInformation("Erreur de connexion");

// ✅ BON
_logger.LogError(ex, "Échec de connexion à la base de données");
```

3. **Contexte, contexte, contexte**
```csharp
// ❌ Pas assez de contexte
_logger.LogError("Erreur");

// ✅ Contexte riche
_logger.LogError(ex, 
    "Échec de sauvegarde pour le document {DocumentId} de l'utilisateur {UserId}", 
    documentId, userId);
```

4. **Scopes pour le contexte transversal**
```csharp
public async Task TraiterCommande(int commandeId)
{
    using (_logger.BeginScope("CommandeId: {CommandeId}", commandeId))
    {
        _logger.LogInformation("Début du traitement");
        // Tous les logs incluront automatiquement CommandeId
        
        await ValiderCommande();
        await FacturerCommande();
        
        _logger.LogInformation("Traitement terminé");
    }
}
```

5. **Performance : Vérifier IsEnabled**
```csharp
// Pour des opérations coûteuses
if (_logger.IsEnabled(LogLevel.Debug))
{
    var detailsCouteux = CalculerStatistiquesComplexes();
    _logger.LogDebug("Statistiques: {Stats}", detailsCouteux);
}
```

6. **Messages templates, pas de concaténation**
```csharp
// ❌ MAUVAIS - Concaténation
_logger.LogInformation("Utilisateur " + userName + " connecté");

// ✅ BON - Template structuré
_logger.LogInformation("Utilisateur {UserName} connecté", userName);
```

7. **Logger les entrées/sorties des méthodes critiques**
```csharp
public async Task<Result> OperationCritique(string param)
{
    _logger.LogInformation("Début OperationCritique avec {Param}", param);
    
    try
    {
        var result = await ExecuterOperation(param);
        _logger.LogInformation("OperationCritique réussie: {Result}", result);
        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "OperationCritique échouée pour {Param}", param);
        throw;
    }
}
```

8. **Éviter les logs dans les boucles**
```csharp
// ❌ MAUVAIS - Spam de logs
foreach (var item in items)
{
    _logger.LogDebug("Traitement de {Item}", item);
}

// ✅ BON - Log récapitulatif
_logger.LogInformation("Traitement de {Count} éléments", items.Count);
```

9. **Logs métier vs Logs techniques**
```csharp
// Log technique
_logger.LogDebug("Requête SQL exécutée en {Duration}ms", duration);

// Log métier
_logger.LogInformation("Commande {OrderId} validée pour un montant de {Amount}€", 
    orderId, amount);
```

10. **Centraliser la configuration**
```csharp
public static class LoggingConfiguration
{
    public static ILoggingBuilder ConfigureAppLogging(this ILoggingBuilder builder)
    {
        return builder
            .SetMinimumLevel(LogLevel.Information)
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning);
    }
}

### Package Serilog

```bash
Install-Package Serilog.Extensions.Logging
Install-Package Serilog.Sinks.Console
Install-Package Serilog.Sinks.File
Install-Package Serilog.Sinks.Seq

// Configuration de Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProperty("Application", "MonAppWPF")
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/app-.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Seq("http://localhost:5341") // Serveur Seq optionnel
            .CreateLogger();

```

