using Elsa.SKS.Backend.BusinessLogic.Entities;
using System;

namespace Elsa.SKS.Backend.BusinessLogic.Entities.Events
{
    public class ParcelEventArgs : EventArgs
    {
        public Parcel Parcel { get; set; }
    }
}