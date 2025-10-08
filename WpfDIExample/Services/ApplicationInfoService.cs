using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfDIExample.Services;

public interface IApplicationInfoService
{
    string ApplicationName { get; }
    string ApplicationVersion { get; }
    IEnumerable<PackageInfo> NuGetPackages { get; }
}

public record PackageInfo(string Name, string Version);

public class ApplicationInfoService : IApplicationInfoService
{
    public string ApplicationName { get; }
    public string ApplicationVersion { get; }
    public IEnumerable<PackageInfo> NuGetPackages { get; }

    public ApplicationInfoService()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

        ApplicationName = assembly.GetName().Name ?? "Unknown";

        var version = assembly.GetName().Version;
        ApplicationVersion = version?.ToString() ?? "1.0.0.0";

        NuGetPackages = GetNuGetPackages();
    }

    private IEnumerable<PackageInfo> GetNuGetPackages()
    {
        var packages = new List<PackageInfo>();
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

        try
        {
            // Lire les métadonnées des assemblies chargés
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .DistinctBy(a => a.GetName().Name);

            foreach (var asm in loadedAssemblies)
            {
                var name = asm.GetName();
                if (name.Name != null && !name.Name.StartsWith("System") &&
                    !name.Name.StartsWith("Microsoft.") &&
                    name.Name != ApplicationName)
                {
                    packages.Add(new PackageInfo(
                        name.Name,
                        name.Version?.ToString() ?? "Unknown"
                    ));
                }
            }
        }
        catch (Exception ex)
        {
            
        }

        return packages.OrderBy(p => p.Name).ToList();
    }
}
