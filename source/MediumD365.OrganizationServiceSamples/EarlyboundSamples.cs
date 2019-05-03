using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace MediumD365.OrganizationServiceSamples
{
    public class EarlyboundSamples
    {
        #region | Private Definitions |

        IOrganizationService _organizationService;

        #endregion

        #region | Constructors |

        public EarlyboundSamples(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        #endregion

        #region | Public Methods |

        public Guid CreateAccount(string name, string url)
        {
            Account account = new Account();
            account.Name = name;
            account.WebSiteURL = url;

            return _organizationService.Create(account);
        }

        public Guid CreateContact(string firstname, string lastname)
        {
            Contact contact = new Contact();
            contact.FirstName = firstname;
            contact.LastName = lastname;

            return _organizationService.Create(contact);
        }

        #endregion
    }
}
