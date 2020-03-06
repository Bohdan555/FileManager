﻿using AutoMapper;
using TransactionManager.Components.Converters;
using TransactionManager.Components.Models;
using TransactionManager.Components.Models.Csv;
using TransactionManager.DAL.Entities;
using System;
using StatusEnum = TransactionManager.Components.Domain.Enums.Status;

namespace TransactionManager.Components.Infrastructure
{
    public class TransactionManagerMapperProfile: Profile
    {
        public TransactionManagerMapperProfile()
        {
            CreateMap<XmlTransaction, Models.Transaction>()
                .ForMember(m => m.Amount, opt => opt.MapFrom(m => m.PaymentDetails.Amount))
                .ForMember(m => m.CurrencyCode, opt => opt.MapFrom(m => m.PaymentDetails.CurrencyCode))
                .ForMember(m => m.TransactionId, opt => opt.MapFrom(m => m.Id))
                .ForMember(m => m.TransactionDate, opt => opt.MapFrom(m => m.transactionDate));

            CreateMap<CsvTransaction, Models.Transaction>();            

            CreateMap<Models.Transaction, DAL.Entities.Transaction>()
                .ForMember(m => m.StatusId, opt => opt.MapFrom(m => Enum.Parse(typeof(StatusEnum),m.Status)))
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Status, opt => opt.Ignore());

            CreateMap<Status, char>().ConvertUsing<StatusTypeConverter>();

            CreateMap<DAL.Entities.Transaction, Models.Response.Transaction>()
                .ForMember(m => m.Id, opt => opt.MapFrom(m => m.TransactionId))
                .ForMember(m => m.Payment, opt => opt.MapFrom(m => string.Concat(m.Amount, " ", m.CurrencyCode)))
                .ForMember(m => m.Status, opt => opt.MapFrom(m => m.Status));            
        }        
    }
}
