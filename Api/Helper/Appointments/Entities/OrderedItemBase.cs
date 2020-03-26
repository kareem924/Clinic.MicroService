using System.Collections.Generic;
using System.Linq;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.Core.Payments.Entities;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public abstract class OrderedItemBase : FullAuditedEntity
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();

        public User Patient { get; protected set; }

        public int ReferenceNumber { get; protected set; }

        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public Transaction Transaction => //LastTransaction =>
            Transactions
                .OrderBy(transaction => transaction.CreationDate)
                .LastOrDefault();

        public void AddTransaction(Transaction newTransaction)
        {
            _transactions.Add(newTransaction);
            // TODO: (asafan)use this event firing instead
            //AddDomainEvent(new PaymentTransactionAddedEvent(newtransaction.Id, this.Id));
        }
        public abstract IReadOnlyCollection<ServiceBase> GetServices();
    }
}