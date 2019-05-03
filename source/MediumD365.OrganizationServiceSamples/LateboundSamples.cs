using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace MediumD365.OrganizationServiceSamples
{
    public class LateboundSamples
    {
        #region | Private Definitions |

        IOrganizationService _organizationService;

        #endregion

        #region | Constructors |

        public LateboundSamples(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        #endregion

        #region | Public Methods |

        public Guid CreateAccount(string name, string url)
        {
            Entity account = new Entity("account");
            account["name"] = name;
            account.Attributes.Add("websiteurl", url);

            /*
             * INFO : 
             * account["name"] = name; ile account.Attributes.Add("websiteurl", url); arasında herhangi bir farklılık yoktur.
             */

            return _organizationService.Create(account);
        }

        public Guid CreateContact(string firstname, string lastname)
        {
            Entity contact = new Entity("contact");
            contact["firstname"] = firstname;
            contact["lastname"] = lastname;

            return _organizationService.Create(contact);
        }

        #endregion
    }
}
