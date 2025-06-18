using System;
using AASAXUtilLib;

namespace isvLicenseGenerator_CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 11)
            {
                Console.WriteLine("Uso: <path> <licenseCode> <customer> <serialNumber> <expirationDate MM-dd-yyyy> <userCount> <vaultName> <keyName> <tenantId> <appId> <secret>");
                Environment.Exit(1);
            }

            string[] formatosFecha = { "MM-dd-yyyy", "MM/dd/yyyy" };
            if (!DateTime.TryParseExact(args[4], formatosFecha, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime expiration))
            {
                Console.WriteLine("Error: el formato de fecha debe ser MM-dd-yyyy o MM/dd/yyyy (ejemplo: 06-12-2025 o 06/12/2025)");
                Environment.Exit(1);
            }

            try
            {
                var licenseInfo = new LicenseInfo
                {
                    FilePath = Path.Combine(args[0], args[2] + "-" + args[1] + ".txt"),
                    LicenseCode = args[1],
                    Customer = args[2],
                    SerialNumber = args[3],
                    Timestamp = DateTime.Now,
                    ExpirationDate = expiration,
                    UserCount = int.Parse(args[5])
                };

                var config = new AxUtilConfiguration { LicenseInfo = licenseInfo };
                var context = new AxUtilContext();
                var util = new AxUtil(context, config);

                bool result = util.GenerateLicenseKeyVault(
                    args[6], // vaultName
                    args[7], // keyName
                    args[8], // tenantId
                    args[9], // appId
                    args[10] // secret
                );

                if (!result)
                {
                    Console.WriteLine("Error: La licencia no se pudo generar.");
                    Environment.Exit(1);
                }

                Console.WriteLine($"Licencia generada exitosamente en: {licenseInfo.FilePath}");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}