﻿using AutoMapper;
using ClinicManager.Application.Commands.Doctor;
using ClinicManager.Core.Enums;
using ClinicManager.Core.IRepositories;
using ClinicManager.Core.ValueObjects;
using ClinicManager.Infrastructure.Persistence;
using Moq;

namespace ClinicManager.Tests.Application.Commands
{
    public class CreateDoctorCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldAddNewDoctorSuccessfully()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockDoctorRepository = new Mock<IDoctorRepository>();

            mockUnitOfWork.Setup(u => u.Doctors).Returns(mockDoctorRepository.Object);


            var createDoctorCommand = new CreateDoctorCommand("John", "Doe", new DateTime(1980, 1, 1),
                new PhoneNumber("31993087574"), new Email("john.doe@example.com"),new Cpf("12345678901"),
                EBloodType.APositive, "Cardiology", new Crm("12345"),
                new Address("123 Street", "City", "State", "12345-234","11",null));  

            var handler = new CreateDoctorCommandHandler(mockUnitOfWork.Object, mockMapper.Object);

            var result = await handler.Handle(createDoctorCommand, CancellationToken.None);

            Assert.True(result.IsSuccess);  
        }
    }
}
