using System;

namespace Elsa.SKS.Package.BusinessLogic.Entities.Events
{
    public class ParcelEventArgs : EventArgs
    {
        public Parcel Parcel { get; set; }
    }
}