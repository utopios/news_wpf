using System.Collections.ObjectModel;

public class CommandeViewModel 
{
    //private readonly ICommandeService _commandeService;
    //private ObservableCollection<Commande> _commandes;

    //public CommandeViewModel(
    //    ICommandeService commandeService, 
    //    ILogger<CommandeViewModel> logger) 
    //    : base(logger)
    //{
    //    _commandeService = commandeService;
        
    //    ChargerCommandesCommand = new AsyncRelayCommand(
    //        execute: ChargerCommandesAsync,
    //        canExecute: () => !IsBusy,
    //        logger: logger,
    //        errorHandler: ex => ErrorMessage = ObtenirMessageUtilisateur(ex)
    //    );

    //    SauvegarderCommandeCommand = new AsyncRelayCommand(
    //        execute: SauvegarderCommandeAsync,
    //        canExecute: () => !IsBusy && CommandeSelectionnee != null,
    //        logger: logger,
    //        errorHandler: GererErreurSauvegarde
    //    );
    //}

    //public ObservableCollection<Commande> Commandes
    //{
    //    get => _commandes;
    //    set => SetProperty(ref _commandes, value);
    //}

    //public ICommand ChargerCommandesCommand { get; }
    //public ICommand SauvegarderCommandeCommand { get; }

    //private async Task ChargerCommandesAsync()
    //{
    //    await ExecuterAvecGestionErreurs(async () =>
    //    {
    //        using (_logger.BeginScope("ChargementCommandes"))
    //        {
    //            var commandes = await _commandeService.ObtenirToutesAsync();
    //            Commandes = new ObservableCollection<Commande>(commandes);
                
    //            _logger.LogInformation("{Count} commandes chargées", commandes.Count);
    //        }
    //    }, "Chargement des commandes");
    //}

    //private async Task SauvegarderCommandeAsync()
    //{
    //    await ExecuterAvecGestionErreurs(async () =>
    //    {
    //        await _commandeService.SauvegarderAsync(CommandeSelectionnee);
            
    //        MessageBox.Show("Commande sauvegardée avec succès!");
    //    }, "Sauvegarde de la commande");
    //}

    //private void GererErreurSauvegarde(Exception ex)
    //{
    //    if (ex is DbUpdateException)
    //    {
    //        ErrorMessage = "Erreur lors de la sauvegarde en base de données. " +
    //                      "Veuillez réessayer.";
    //    }
    //    else
    //    {
    //        ErrorMessage = ObtenirMessageUtilisateur(ex);
    //    }
    //}
}