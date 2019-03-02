using System;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace MediumD365.Helper
{
    public class ConnectionHelper
    {
        #region | Private Definitions  |

        readonly string _connectionName = string.Empty;

        #endregion

        #region | Constructors |

        public ConnectionHelper(string connectionName)
        {
            _connectionName = connectionName;
        }

        #endregion

        #region | Public Methods |

        public IOrganizationService GetIOrganizationService(Guid? callerId = null)
        {
            IOrganizationService result = null;

            var crmConnection = GetCrmServiceClient(callerId);

            if (crmConnection != null && crmConnection.IsReady)
            {
                if (crmConnection.OrganizationServiceProxy != null)
                {
                    result = (IOrganizationService)crmConnection.OrganizationServiceProxy;
                }
                else if (crmConnection.OrganizationWebProxyClient != null)
                {
                    result = (IOrganizationService)crmConnection.OrganizationWebProxyClient;
                }

            }

            return result;
        }

        public CrmServiceClient GetCrmServiceClient(Guid? callerId = null)
        {
            CrmServiceClient result = null;

            var connectionString = Properties.Settings.Default[_connectionName].ToString();

            if (!string.IsNullOrEmpty(connectionString))
            {
                result = new CrmServiceClient(connectionString);

                if (callerId.HasValue && callerId.Value.Equals(Guid.Empty))
                {
                    result.CallerId = callerId.Value;
                }
            }

            return result;
        }

        #endregion
    }
}