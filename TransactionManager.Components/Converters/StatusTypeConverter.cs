using AutoMapper;
using TransactionManager.DAL.Entities;
using System;
using StatusEnum = TransactionManager.Components.Domain.Enums.Status;

namespace TransactionManager.Components.Converters
{
    public class StatusTypeConverter : ITypeConverter<Status, char>
    {
        public char Convert(Status source, char destination, ResolutionContext context)
        {
            switch (source.Id) 
            {
                case (int)StatusEnum.Approved:
                    return 'A';
                case (int)StatusEnum.Failed:
                case (int)StatusEnum.Rejected:
                    return 'R';
                case (int)StatusEnum.Finished:
                case (int)StatusEnum.Done:
                    return 'D';
                default:
                    throw new NotImplementedException($"Convertion for status {source} is not implemented");
            }
        }
    }
}
