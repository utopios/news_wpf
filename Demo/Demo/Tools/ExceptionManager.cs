using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Demo.Tools
{
    public class ExceptionManager
    {
        private readonly ILogger<ExceptionManager> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ExceptionManager(
            ILogger<ExceptionManager> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void ConfigurerGestionnairesCentralises()
        {
            Application.Current.DispatcherUnhandledException +=
                GererDispatcherException;

            AppDomain.CurrentDomain.UnhandledException +=
                GererAppDomainException;

            TaskScheduler.UnobservedTaskException +=
                GererTaskException;
        }

        private void GererDispatcherException(
            object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            LoggerException(e.Exception, "DispatcherUnhandledException");

            if (EstExceptionRecuperable(e.Exception))
            {
                AfficherMessageUtilisateur(e.Exception, false);
                e.Handled = true;
            }
            else
            {
                AfficherMessageUtilisateur(e.Exception, true);
                e.Handled = false; // Laisser l'application crasher
            }
        }

        private void GererAppDomainException(
            object sender,
            UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            LoggerException(exception, "AppDomainUnhandledException");

            if (e.IsTerminating)
            {
                EffectuerActionsFinales(exception);
            }
        }

        private void GererTaskException(
            object sender,
            UnobservedTaskExceptionEventArgs e)
        {
            LoggerException(e.Exception, "UnobservedTaskException");
            e.SetObserved();
        }

        private void LoggerException(Exception ex, string source)
        {
            _logger.LogCritical(ex,
                "Exception non gérée - Source: {Source}, Type: {Type}, Message: {Message}",
                source, ex.GetType().Name, ex.Message);

            // Log de la stack trace complète
            _logger.LogDebug("Stack Trace: {StackTrace}", ex.StackTrace);

            // Log des exceptions internes
            var innerEx = ex.InnerException;
            int niveau = 1;
            while (innerEx != null)
            {
                _logger.LogError("Exception interne niveau {Niveau}: {Message}",
                    niveau, innerEx.Message);
                innerEx = innerEx.InnerException;
                niveau++;
            }
        }

        private bool EstExceptionRecuperable(Exception ex)
        {
            // Exceptions critiques non récupérables
            return ex is not (
                OutOfMemoryException or
                StackOverflowException or
                AccessViolationException or
                AppDomainUnloadedException
            );
        }

        private void AfficherMessageUtilisateur(Exception ex, bool estFatal)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var message = estFatal
                    ? $"Une erreur critique s'est produite:\n\n{ex.Message}\n\n" +
                      "L'application va se fermer."
                    : $"Une erreur s'est produite:\n\n{ex.Message}\n\n" +
                      "L'application va continuer.";

                var icon = estFatal ? MessageBoxImage.Stop : MessageBoxImage.Error;

                MessageBox.Show(message, "Erreur", MessageBoxButton.OK, icon);
            });
        }

        private void EffectuerActionsFinales(Exception ex)
        {
            try
            {
                // Sauvegarde d'urgence
                _logger.LogInformation("Sauvegarde d'urgence avant fermeture...");

                // Envoi d'un rapport d'erreur
                EnvoyerRapportErreur(ex);

                // Nettoyage des ressources
                _logger.LogInformation("Nettoyage des ressources...");
            }
            catch (Exception cleanupEx)
            {
                _logger.LogError(cleanupEx, "Erreur lors du nettoyage final");
            }
            finally
            {
                Log.CloseAndFlush(); // Serilog
            }
        }

        private void EnvoyerRapportErreur(Exception ex)
        {
            // Intégration avec un service de reporting (Sentry, AppInsights, etc.)
            _logger.LogInformation("Envoi du rapport d'erreur au serveur...");
        }
    }
}
