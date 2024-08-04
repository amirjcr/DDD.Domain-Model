using Microsoft.VisualBasic;
using System;

namespace Common.Domain
{
    public class BaseDomainEvent
    {
        public BaseDomainEvent(string ocuuredEvent)
        {
            Event = ocuuredEvent;
            CreationDate = DateTime.Now;
        }

        public string Event { get; }
        public DateTime CreationDate { get; }

    }

}

