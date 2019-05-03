using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MediumD365.Helper;
using Microsoft.Xrm.Sdk;

namespace MediumD365.OrganizationServiceSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConnectionHelper connectionHelper = new ConnectionHelper("D365TR_Sandbox");
                var organizationService = connectionHelper.GetIOrganizationService();

                if (organizationService != null)
                {
                    OptionSetValueCollection multiselect = new OptionSetValueCollection();
                    multiselect.Add(new OptionSetValue(1));
                    multiselect.Add(new OptionSetValue(2));
                    multiselect.Add(new OptionSetValue(3));

                    Entity entity = new Entity("contact");
                    entity["d365tr_multiselect"] = multiselect;

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
    }
}
