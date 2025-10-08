// Global usings pour tout le projet
// Simplifie les imports dans tous les fichiers

// System de base
global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Input;
global using System.Windows.Controls;

// Microsoft Extensions (Injection de dépendances, Configuration, Logging)
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

// Namespaces du projet
global using WpfDIExample.Models;
global using WpfDIExample.Services;
global using WpfDIExample.Repositories;
global using WpfDIExample.ViewModels;
global using WpfDIExample.Views;