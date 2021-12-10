using Elsa.SKS.Package.Services.DTOs;
using FizzWare.NBuilder;

namespace Elsa.SKS.Package.IntegrationTests.Data
{
    public class ParcelData
    {
        public static Parcel Parcel => GenerateParcel();

        private static Parcel GenerateParcel()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(p => p.Recipient = 
                    Builder<Recipient>
                        .CreateNew()
                        .With(x => x.Street = "Werner-von-Siemens-Ring 14")
                        .With(x => x.PostalCode = "85630")
                        .With(x => x.City = "Grasbrunn")
                        .With(x => x.Country = "Germany")
                        .Build()
                )
                .With(p => p.Sender = 
                    Builder<Recipient>
                        .CreateNew()
                        .With(x => x.Street = "Am Europlatz 3")
                        .With(x => x.PostalCode = "A-1120")
                        .With(x => x.City = "Wien")
                        .With(x => x.Country = "Austria")
                        .Build()
                )
                .Build();

            return parcel;
        }
    }
}