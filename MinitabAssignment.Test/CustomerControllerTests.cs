using Microsoft.AspNetCore.Mvc;
using Minitab_Assignment.Controllers;
using Moq;
using NUnit.Framework;

namespace Minitab_Assignment.Test
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private Mock<ICrmRepository> mockCrmRepo;
        private Mock<IAddressValidator> mockAddressValidator;
        private CustomersController testController;

        [SetUp]
        public void SetUp()
        {
            mockCrmRepo = new Mock<ICrmRepository>();
            mockAddressValidator = new Mock<IAddressValidator>();
            testController = new CustomersController(mockAddressValidator.Object, mockCrmRepo.Object);
        }

        [Test]
        public void TestNoAddress()
        {
            mockAddressValidator.Setup(mock => mock.ValidateAddress(It.IsAny<Address>()));
            mockCrmRepo.Setup(mock => mock.UpsertCustomer(It.IsAny<Customer>()));

            var result = testController.Create(new Customer("First", "Last"));
            Assert.IsInstanceOf<EmptyResult>(result);

            mockAddressValidator.Verify(mock => mock.ValidateAddress(It.IsAny<Address>()), Times.Never());
            mockCrmRepo.Verify(mock => mock.UpsertCustomer(It.Is<Customer>(c => c.Address == null)), Times.Once());
        }

        [Test]
        public void TestInvalidAddress()
        {
            mockAddressValidator.Setup(mock => mock.ValidateAddress(It.IsAny<Address>())).Returns(false);
            mockCrmRepo.Setup(mock => mock.UpsertCustomer(It.IsAny<Customer>()));

            var result = testController.Create(new Customer("First", "Last") { Address = new Address("", "", "", "", "") });
            Assert.IsInstanceOf<EmptyResult>(result);

            mockAddressValidator.Verify(mock => mock.ValidateAddress(It.IsAny<Address>()), Times.Once());
            mockCrmRepo.Verify(mock => mock.UpsertCustomer(It.Is<Customer>(c => c.Address == null)), Times.Once());
        }

        [Test]
        public void TestValidAddress()
        {
            mockAddressValidator.Setup(mock => mock.ValidateAddress(It.IsAny<Address>())).Returns(true);
            mockCrmRepo.Setup(mock => mock.UpsertCustomer(It.IsAny<Customer>()));

            var result = testController.Create(new Customer("First", "Last") { Address = new Address("a", "b", "c", "d", "e") });
            Assert.IsInstanceOf<EmptyResult>(result);

            mockAddressValidator.Verify(mock => mock.ValidateAddress(It.IsAny<Address>()), Times.Once());
            mockCrmRepo.Verify(mock => mock.UpsertCustomer(It.Is<Customer>(c => c.Address != null && c.Address.Line1 == "a")), Times.Once());
        }
    }
}