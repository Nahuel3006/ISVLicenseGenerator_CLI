using System;
using AASAXUtilLib;

namespace testGenerateLicense
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 11)
            {
                Console.WriteLine("Uso: <path> <licenseCode> <customer> <serialNumber> <expirationDate yyyy-MM-dd> <userCount> <vaultName> <keyName> <tenantId> <appId> <secret>");
                return;
            }

            var licenseInfo = new LicenseInfo
            {
                FilePath = Path.Combine(args[0], args[2] + "-" + args[1] + ".txt"),
                LicenseCode = args[1],
                Customer = args[2],
                SerialNumber = args[3],
                Timestamp = DateTime.Now,
                ExpirationDate = DateTime.Parse(args[4]),
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

            Console.WriteLine(result ? "Licencia generada correctamente." : "Error al generar la licencia.");
        }
    }
}