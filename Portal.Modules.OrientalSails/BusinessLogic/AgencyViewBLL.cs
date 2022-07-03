using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class AgencyViewBLL
    {
        public AgencyRepository AgencyRepository { get; set; }
        public AgencyContractRepository AgencyContractRepository { get; set; }
        public ContractRepository ContractRepository { get; set; }
        public QuotationRepository QuotationRepository { get; set; }
        public AgencyViewBLL()
        {
            AgencyRepository = new AgencyRepository();
            AgencyContractRepository = new AgencyContractRepository();
            ContractRepository = new ContractRepository();
            QuotationRepository = new QuotationRepository();
        }

        public void Dispose()
        {
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
            if (AgencyContractRepository != null)
            {
                AgencyContractRepository.Dispose();
                AgencyContractRepository = null;
            }
            if (ContractRepository != null)
            {
                ContractRepository.Dispose();
                ContractRepository = null;
            }
            if (QuotationRepository != null)
            {
                QuotationRepository.Dispose();
                QuotationRepository = null;
            }
        }

        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.AgencyGetById(agencyId);
        }
        public void AgencyContractSaveOrUpdate(AgencyContract agencyContract)
        {
            AgencyContractRepository.SaveOrUpdate(agencyContract);
        }
        public IList<AgencyContract> AgencyContractGetAllByAgency(int agencyId)
        {
            return AgencyContractRepository.AgencyContractGetAllByAgency(agencyId);
        }
        public AgencyContract AgencyContractGetById(int agencyContractId)
        {
            return AgencyContractRepository.AgencyContractGetById(agencyContractId);
        }
        public IList<Contracts> ContractGetAll()
        {
            return ContractRepository.ContractGetAll();
        }
        public Contracts ContractGetById(int contractId)
        {
            return ContractRepository.ContractGetById(contractId);
        }
        public IList<Quotation> QuotationGetAll()
        {
            return QuotationRepository.QuotationGetAll();
        }

        public Quotation QuotationGetById(int quotationId)
        {
            return QuotationRepository.QuotationGetById(quotationId);
        }
    }
}