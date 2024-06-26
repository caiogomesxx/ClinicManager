﻿using ClinicManager.Application.Abstractions;
using ClinicManager.Application.ViewModels;
using ClinicManager.Core.Enums;
using ClinicManager.Core.IRepositories;
using ClinicManager.Core.IServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManager.Application.Commands.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginViewModel>>
    {
        private IAuthService _authService;
        private IPatientRepository _patientRepository;
        private IDoctorRepository _doctorRepository;

        public LoginCommandHandler(IAuthService authService, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _authService = authService;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<Result<LoginViewModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            Core.Entities.BasePersonEntity user; 
              
            if(request.Role == ERoleEnum.Doctor)
                user = await _doctorRepository.GetByEmailAndPasswordAsync(request.Email, passwordHash);
            else
                user = await _patientRepository.GetByEmailAndPasswordAsync(request.Email, passwordHash);

            if (user is null)
                return Result<LoginViewModel>.NotFound("Email ou senha inválidos."); 

            var token = _authService.GenerateJwtToken(user.Id, request.Role.ToString());

            var viewModel = new LoginViewModel(user.Id, token, request.Role.ToString());

            return Result<LoginViewModel>.Success(viewModel, "Login realizado com sucesso!");
        }
    }
}
