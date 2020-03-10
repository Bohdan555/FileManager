using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TransactionManager.Components.Domain.Enums;
using TransactionManager.Components.Exceptions;
using TransactionManager.Components.Infrastructure;
using TransactionManager.Components.Models;
using TransactionManager.Components.Models.Requests;
using TransactionManager.Components.Services.Implementation;
using TransactionManager.Components.Services.Interfaces;
using TransactionManager.Components.Validators;
using TransactionManager.DAL.Repositories.Interfaces;

namespace TransactionManager.Tests.Unit
{
    [TestClass]
    public class TransactionManagerServiceTests
    {
        private static Mock<IRepository<DAL.Entities.Transaction>> _transactionRepositoryMock = 
            new Mock<IRepository<DAL.Entities.Transaction>>();

        private static Mock<IValidator<List<Transaction>>> _transactionValidatorMock =
            new Mock<IValidator<List<Transaction>>>();

        private static ITransactionManagerService _target;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            var mapperConfiguration = new MapperConfiguration(opts => { opts.AddProfile<TransactionManagerMapperProfile>(); });
            var mapper = mapperConfiguration.CreateMapper();

            _target = new TransactionManagerService(mapper, _transactionValidatorMock.Object, _transactionRepositoryMock.Object);
        }

        [TestMethod]
        public async Task ImportTransaction_GivenFileSizeIsIncorrect_ShouldThrowDomainException()
        {
            //Arrange
            var file = new Mock<IFormFile>();
            var fileName = "test.xml";

            file.Setup((f) => f.FileName).Returns(fileName);
            file.Setup((f) => f.Length).Returns(InvalidFileLength);

            var uploadFileRequest = new UploadFileRequest
            {
                File = file.Object
            };

            //Act
            Func<Task> actual = async () => await _target.ImportTransactions(uploadFileRequest);

            //Assert
            actual.Should().ThrowExactly<DomainException>();
        }

        [TestMethod]
        public async Task ImportTransaction_GivenFileExtensionIsIncorrect_ShouldThrowDomainException()
        {
            //Arrange
            var file = new Mock<IFormFile>();
            var fileName = "test.txt";

            file.Setup((f) => f.FileName).Returns(fileName);
            file.Setup((f) => f.Length).Returns(ValidFileLength);

            var uploadFileRequest = new UploadFileRequest
            {
                File = file.Object
            };

            //Act
            Func<Task> actual = async () => await _target.ImportTransactions(uploadFileRequest);

            //Assert
            actual.Should().ThrowExactly<DomainException>();
        }

        [TestMethod]
        public async Task ImportTransaction_GivenFileIsValid_ShouldImportTransactions()
        {
            //Arrange
            var file = new Mock<IFormFile>();
            var fileName = "test.xml";
            var expected = Times.Once();
            var fileContent = new XmlTransactions
            {
                Transactions = new List<XmlTransaction>
                {
                    new XmlTransaction
                    {
                        Id = "Id",
                        PaymentDetails = new PaymentDetails
                        {
                            Amount = "100",
                            CurrencyCode = "USD"
                        },
                        TransactionDate = DateTime.Now.ToString(ValidDateFormat),
                        Status = Status.Done.ToString()
                    }
                }
            };

            var memoryStream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(XmlTransactions));                        
            
            serializer.Serialize(memoryStream, fileContent);
            memoryStream.Flush();
            memoryStream.Position = 0;

            file.Setup((f) => f.OpenReadStream()).Returns(memoryStream);
            file.Setup((f) => f.FileName).Returns(fileName);
            file.Setup((f) => f.Length).Returns(ValidFileLength);
            _transactionValidatorMock.Setup(t => t.Validate(It.IsAny<List<Transaction>>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationItem>()));

            var uploadFileRequest = new UploadFileRequest
            {
                File = file.Object
            };

            //Act
            await _target.ImportTransactions(uploadFileRequest);

            //Assert
            _transactionRepositoryMock.Verify(r => r.CreateMany(It.IsAny<List<DAL.Entities.Transaction>>()), expected);
        }

        [TestMethod]
        public async Task GetTransactionsByDate_GivenDateIsCorrect_ShouldReturnTransactions()
        {
            //Arrange
            var startDate = DateTime.Now.ToString(ValidDateFormat);
            var endDate = DateTime.Now.AddHours(1).ToString(ValidDateFormat);

            var transactions = new List<DAL.Entities.Transaction>
                {
                    new DAL.Entities.Transaction
                    {
                        Id = 1,
                        TransactionId = "Id",
                        Amount = 100,
                        CurrencyCode = "USD",
                        TransactionDate = DateTime.Now,
                        StatusId = (int)Status.Approved,
                        Status = new DAL.Entities.Status 
                        {
                            Id = 1,
                            Name = "Approved"
                        }
                    }
                };
            var expected = new Components.Models.Response.Transaction
            {
                Id = $"{transactions.First().TransactionId}",
                Payment = $"{transactions.First().Amount} {transactions.First().CurrencyCode}",
                Status = 'A'
            };

            _transactionRepositoryMock.Setup(t => t.Find(It.IsAny<Func<DAL.Entities.Transaction,bool>>()))
                .ReturnsAsync(transactions);

            //Act
            var result = await _target.GetTransactions(startDate, endDate);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]        
        public async Task GetTransactionsByCurrencyCode_GivenCurrencyCodeIsCorrect_ShouldReturnTransactions()
        {
            //Arrange
            var currencyCode = "USD";

            var transactions = new List<DAL.Entities.Transaction>
                {
                    new DAL.Entities.Transaction
                    {
                        Id = 1,
                        TransactionId = "Id",
                        Amount = 100,
                        CurrencyCode = "USD",
                        TransactionDate = DateTime.Now,
                        StatusId = (int)Status.Approved,
                        Status = new DAL.Entities.Status
                        {
                            Id = 1,
                            Name = "Approved"
                        }
                    }
                };
            var expected = new Components.Models.Response.Transaction
            {
                Id = $"{transactions.First().TransactionId}",
                Payment = $"{transactions.First().Amount} {transactions.First().CurrencyCode}",
                Status = 'A'
            };

            _transactionRepositoryMock.Setup(t => t.Find(It.IsAny<Func<DAL.Entities.Transaction, bool>>()))
                .ReturnsAsync(transactions);

            //Act
            var result = await _target.GetTransactions(currencyCode);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task GetTransactionsByStatus_GivenStatusIsCorrect_ShouldReturnTransactions()
        {
            //Arrange
            var status = Status.Approved;

            var transactions = new List<DAL.Entities.Transaction>
                {
                    new DAL.Entities.Transaction
                    {
                        Id = 1,
                        TransactionId = "Id",
                        Amount = 100,
                        CurrencyCode = "USD",
                        TransactionDate = DateTime.Now,
                        StatusId = (int)Status.Approved,
                        Status = new DAL.Entities.Status
                        {
                            Id = 1,
                            Name = "Approved"
                        }
                    }
                };
            var expected = new Components.Models.Response.Transaction
            {
                Id = $"{transactions.First().TransactionId}",
                Payment = $"{transactions.First().Amount} {transactions.First().CurrencyCode}",
                Status = 'A'
            };

            _transactionRepositoryMock.Setup(t => t.Find(It.IsAny<Func<DAL.Entities.Transaction, bool>>()))
                .ReturnsAsync(transactions);

            //Act
            var result = await _target.GetTransactions(status);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task GetTransactionsByDate_GivenDateIsInvalid_ShouldThrowDomainException()
        {
            //Arrange
            var startDate = DateTime.Now.ToString(InvalidDateFormat);
            var endDate = DateTime.Now.AddDays(1).ToString(InvalidDateFormat);

            _transactionRepositoryMock.Setup(t => t.Find(It.IsAny<Func<DAL.Entities.Transaction, bool>>()))
                .ReturnsAsync(new List<DAL.Entities.Transaction>());

            //Act
            Func<Task> result = async () => await _target.GetTransactions(startDate, endDate);

            //Assert
            result.Should().Throw<DomainException>();
        }

        [TestMethod]
        public async Task GetTransactionsByCurrencyCode_GivenCurrencyCodeIsInvalid_ShouldThrowDomainException()
        {
            //Arrange
            string currencyCode = default;

            _transactionRepositoryMock.Setup(t => t.Find(It.IsAny<Func<DAL.Entities.Transaction, bool>>()))
                .ReturnsAsync(new List<DAL.Entities.Transaction>());

            //Act
            Func<Task> result = async () => await _target.GetTransactions(currencyCode);

            //Assert
            result.Should().Throw<DomainException>();
        }


        [TestMethod]
        public async Task GetTransactionsByStatus_GivenStatusIsInvalid_ShouldThrowDomainException()
        {
            //Arrange
            Status status = default;                        

            _transactionRepositoryMock.Setup(t => t.Find(It.IsAny<Func<DAL.Entities.Transaction, bool>>()))
                .ReturnsAsync(new List<DAL.Entities.Transaction>());

            //Act
            Func<Task> result = async () =>  await _target.GetTransactions(status);

            //Assert
            result.Should().Throw<DomainException>();
        }

        private const int InvalidFileLength = 1000001;
        private const int ValidFileLength = 1000000;
        private const string ValidDateFormat = "dd/MM/yyyy HH:mm:ss";
        private const string InvalidDateFormat = "dd/MM/yyyy HH:mm";
    }
}