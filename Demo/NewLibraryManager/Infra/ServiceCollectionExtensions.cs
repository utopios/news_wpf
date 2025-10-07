using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace NewLibraryManager.Infra
{
    static class ServiceCollectionExtensions
    {
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<BookListViewModel>();
        }


        public static void AddWindows(this IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
        }

    }
}
