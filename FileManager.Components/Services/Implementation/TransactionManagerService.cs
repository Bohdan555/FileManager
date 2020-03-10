using AutoMapper;
using CsvHelper;
using TransactionManager.Components.Domain.Enums;
using TransactionManager.Components.Exceptions;
using TransactionManager.Components.Models;
using TransactionManager.Components.Models.Csv;
using TransactionManager.Components.Models.Requests;
using TransactionManager.Components.Services.Interfaces;
using TransactionManager.Components.Validators;
using TransactionManager.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ResponseTransaction = TransactionManager.Components.Models.Response.Transaction;

namespace TransactionManager.Components.Services.Implementation
{
    public class TransactionManagerService : ITransactionManagerService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<List<Transaction>> _validator;
        private readonly IRepository<DAL.Entities.Transaction> _transactionRepository;

        public TransactionManagerService(IMapper mapper, IValidator<List<Transaction>> validator, IRepository<DAL.Entities.Transaction> transactionRepository)
        {
            _mapper = mapper;
            _validator = validator;
            _transactionRepository = transactionRepository;
        }

        public async Task ImportTransactions(UploadFileRequest request)
        {
            var file = request.File;

            GuardFileType(file);
            GuardFileSize(file);

            var deserializedFile = await DeserializeFile(file);

            await GuardFileIsValid(deserializedFile);

            var transactions = _mapper.Map<List<DAL.Entities.Transaction>>(deserializedFile);

            await _transactionRepository.CreateMany(transactions);
            await _transactionRepository.Save();                        
        }

        public async Task<List<ResponseTransaction>> GetTransactions(string startDate, string endDate)
        {
            GuardArgumentIsNotNull(startDate, nameof(startDate));
            GuardArgumentIsNotNull(startDate, nameof(endDate));
            
            var parsedStartDate = TryParseDate(startDate);
            var parseEndDate = TryParseDate(endDate);

            var transactions = await _transactionRepository.Find(p => p.TransactionDate >= parsedStartDate && p.TransactionDate <= parseEndDate);
            return _mapper.Map<List<ResponseTransaction>>(transactions);
        }

        public async Task<List<ResponseTransaction>> GetTransactions(Status status)
        {
            GuardStatusIsValid(status);
            var transactions = await _transactionRepository.Find(p => p.StatusId == (int)status);
            return _mapper.Map<List<ResponseTransaction>>(transactions);
        }

        public async Task<List<ResponseTransaction>> GetTransactions(string currencyCode)
        {
            GuardArgumentIsNotNull(currencyCode, nameof(currencyCode));
            var transactions = await _transactionRepository.Find(p => p.CurrencyCode == currencyCode);
            return _mapper.Map<List<ResponseTransaction>>(transactions);
        }

        private static void GuardStatusIsValid(Status status)
        {
            if (status == default)
                throw new DomainException("Status is invalid");
        }

        private async Task<List<Transaction>> DeserializeFile(IFormFile file)
        {
            var fileType = GetFileType(file);            

            switch (fileType)
            {
                case FileType.Xml:
                    var deserializedXmlFile = await DeserializeXml(file.OpenReadStream());
                    return _mapper.Map<List<Transaction>>(deserializedXmlFile.Transactions);
                case FileType.Csv:
                    var deserializedCsvFile = await DeserializeCsv(file.OpenReadStream());
                    return _mapper.Map<List<Transaction>>(deserializedCsvFile);
                default:
                    throw new Exception("FileType is not supported");
            }
        }

        private async Task<XmlTransactions> DeserializeXml(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(XmlTransactions));
            XmlTransactions transactions;
            using (stream)
            {
                transactions = (XmlTransactions)serializer.Deserialize(stream);
            }

            return await Task.FromResult(transactions);
        }

        private async Task<List<CsvTransaction>> DeserializeCsv(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Configuration.MissingFieldFound = default;
                    csvReader.Configuration.HeaderValidated = default;
                    var records = csvReader.GetRecords<CsvTransaction>().ToList();                    
                    return await Task.FromResult(records);
                }
            }
        }

        private static void GuardFileSize(IFormFile formFile)
        {
            if (formFile.Length < MinFileSize)
                throw new DomainException("File is too small");

            if (formFile.Length > MaxFileSize)
                throw new DomainException("Upload unsuccessful. The file is too large. The maximum file size allowed is 1 Mb");
        }

        private static void GuardFileType(IFormFile formFile)
        {
            var isValidType = ValidTypeExtensions.Any(format =>
                    formFile.FileName.EndsWith(format, StringComparison.OrdinalIgnoreCase));

            if (!isValidType)
                throw new DomainException("Unknown format");
        }        

        private static FileType GetFileType(IFormFile file)
        {
            var fileExtension = file.FileName.Substring(file.FileName.IndexOf(".") + 1);
            return (FileType)Enum.Parse(typeof(FileType), fileExtension, true);
        }

        private async Task GuardFileIsValid(List<Transaction> file)
        {
            var validationResult = await _validator.Validate(file);

            validationResult.ThrowIfHasErrors();
        }

        private static void GuardArgumentIsNotNull<T>(T argument, string name)
        {
            if (argument == null)
                throw new DomainException($"Argument {name} is null");
        }

        private static DateTime TryParseDate(string date)
        {
            DateTime parsedDate;
            if (!DateTime.TryParseExact(date, ValidDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                throw new DomainException("Date is invalid");

            return parsedDate;
        }

        private const int MinFileSize = 1;
        private const int MaxFileSize = 1000000;

        private static readonly string[] ValidTypeExtensions = new string[] { ".csv", ".xml" };
        private static readonly string[] ValidDateFormats = new string[] { "yyyy-MM-ddTHH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
    }
}
