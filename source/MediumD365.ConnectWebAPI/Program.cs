using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediumD365.ConnectWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //INFO : Eğer projeniz .NET Framework 4.5.2 ve altında ise Dynamics 365 Web API 'ye bağlanabilmek için bu satırı eklemeniz gerekmekte
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string authorityURI = "https://login.microsoftonline.com/{TENANT_ID}"; //TODO: {TENANT_ID} bilginiz
                string d365Url = "{DYNAMICS_365_URI}"; //TODO: Dynamics 365 CE instance url bilginiz
                string clientId = "{APPLICATION_ID}"; //TODO: Azure Active Directory 'de oluşturduğunuz Application ID bilgisi
                string clientSecret = "{CLIENT_SECRET}"; //TODO: Azure Active Directory 'de olluşturduğunuz Application için CLIENT SECRET bilgisi

                S2SAuthentication s2sAuthentication = new S2SAuthentication(authorityURI, d365Url, clientId, clientSecret);
                var token = s2sAuthentication.RetrieveAuthTokenByUsingADAL();

                if (!string.IsNullOrEmpty(token))
                {
                    #region | CRUD |

                    CRUDSamples crudSamples = new CRUDSamples(d365Url, token);

                    JObject contactCreateData = new JObject();
                    contactCreateData.Add("firstname", "Emre");
                    contactCreateData.Add("lastname", "GÜLCAN");
                    contactCreateData.Add("telephone1", "123456789");

                    var createdDataURI = crudSamples.Create("contact", contactCreateData);
                    var createdDataId = WebAPIHelper.GetRecordId(createdDataURI);

                    JObject contactUpdateData = new JObject();
                    contactUpdateData.Add("jobtitle", "Microsoft Dynamics 365 Senior Developer");
                    contactUpdateData.Add("emailaddress1", "emregulcan@gmail.com");
                    contactUpdateData.Add("parentcustomerid_account@odata.bind", "/accounts(75161572-876A-E911-A838-000D3AB8652F)");

                    crudSamples.Update("contact", createdDataId, contactUpdateData);
                    crudSamples.UpdateSingleField("contact", createdDataId, "telephone1", "5413511983");

                    #endregion

                    #region | Query |

                    QuerySamples querySamples = new QuerySamples(d365Url, token);
                    var retrievedData = querySamples.Retrieve("contacts?$select=fullname,_parentcustomerid_value");

                    Console.WriteLine("Dynamics 365 CE Alınan data");
                    Console.WriteLine(retrievedData);

                    #endregion
                }
            }
            catch (CrmHttpResponseException crmHttpEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dynamics 365 Web API işlemi sırasında bir hata oluştu : ");
                Console.WriteLine(crmHttpEx.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Bir hata oluştu : ");
                Console.WriteLine(ex.Message);
            }


            Console.WriteLine("");
            Console.WriteLine("Uygulamayı kapatmak için bir tuşa basınız");
            Console.ReadLine();
        }
    }
}
