using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using MediumD365.Helper;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

namespace ConnectToDynamics365
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IOrganizationService organizationService = null;

                //INFO : Alternate_01 metodunu kullanabilmek için kullanıcı adı + parola + Dynamics 365 Online url bilgisi gereklidir
                organizationService = Alternate_01("KULLANICIADI@ORGANIZASYONADI.onmicrosoft.com", "PAROLA", "https://ORGANIZASYONADI.crm4.dynamics.com");

                //INFO : Alternate_02 metodunu kullanabilmek için app.config dosyasında <connectionStrings> altında tanımlama yapılmalıdır.
                organizationService = Alternate_02("D365TR_Sandbox");


                /*
                 * INFO : Tüm projelerimizde ortak bir yapı kullanmak istersek "MediumD365.Helper" projesinde yer alan ConnectionHelper class kullanılabilir.
                 * Alternate_02 ile benzer şekilde Dynamics 365 bağlantı bilgileri config yapısı üzerinde tutulmaktadır.
                 * app.config 'e direk yazmak yerine Properties>Settings üzerinden ekleme yapılmıştır.
                 */
                ConnectionHelper connectionHelper = new ConnectionHelper("D365TR_Sandbox");
                organizationService = connectionHelper.GetCrmServiceClient();
                organizationService = connectionHelper.GetIOrganizationService();


                if (organizationService != null)
                {
                    WhoAmIRequest request = new WhoAmIRequest();
                    WhoAmIResponse response = (WhoAmIResponse)organizationService.Execute(request);

                    Console.WriteLine($"User ID : {response.UserId}");
                    Console.WriteLine($"BusinessUnit ID : {response.BusinessUnitId}");
                    Console.WriteLine($"Organization ID : {response.OrganizationId}");
                }
            }
            catch (FaultException<OrganizationServiceFault> organizationFaultException)
            {
                /*
                 * INFO : 
                 * Dynamics 365 SDK tarafından oluşan hataları FaultException<OrganizationServiceFault>
                 * yapısını kullanarak yakalabiliriz.
                 * 
                 */
            }
            catch (Exception ex)
            {

            }


            Console.WriteLine(" ");
            Console.WriteLine("Kapatmak için bir tuşa basınız");
            Console.ReadLine();
        }



        static IOrganizationService Alternate_01(string username, string password, string organizationServiceUrl)
        {
            IOrganizationService result = null;

            ClientCredentials clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = username;
            clientCredentials.UserName.Password = password;

            Uri organizationServiceUri = new Uri(organizationServiceUrl);

            OrganizationServiceProxy proxy = new OrganizationServiceProxy(organizationServiceUri, null, clientCredentials, null);

            result = (IOrganizationService)proxy;

            return result;
        }

        static IOrganizationService Alternate_02(string connectionName)
        {
            IOrganizationService result = null;

            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);

            if (crmServiceClient.OrganizationServiceProxy != null)
            {
                result = (IOrganizationService)crmServiceClient.OrganizationServiceProxy;
            }
            else if (crmServiceClient.OrganizationWebProxyClient != null)
            {
                result = (IOrganizationService)crmServiceClient.OrganizationWebProxyClient;
            }

            return result;
        }
    }
}
