﻿using Microsoft.Extensions.Logging;
using SuperBike.Auth.Context;
using SuperBike.Business.Contracts;
using SuperBike.Business.UseCases.User;
using SuperBike.Business.UseCases.Validators;
using SuperBike.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SuperBike.Business.UseCases
{
    public abstract class UseCaseBase : UnitOfWorkScope
    {
        private readonly IDbConnection _dbConnection;
        private IDbConnection DbConnection => _dbConnection;
        protected UseCaseBase(ILogger<UseCaseBase> logger, IDbConnection dbConnection) : base(dbConnection)
        {
            LogUseCase.SetLogger(logger);            
        }
        
        public virtual ResultValidation Validate(object entity)
        {
            if (entity == null) return new ResultValidation(Enumerable.Empty<ValidationResult>().ToList());

            var valid = new ValidationContext(entity);
            var valids = new List<ValidationResult>();
            var result = Validator.TryValidateObject(entity, valid, valids, true);            
            
            return new ResultValidation(valids);
        }
    }
    internal static class LogUseCase
    {
        public static void SetLogger<T>(ILogger<T> logger) => _logger = logger;
        private static ILogger? _logger;        
        public static void LogInf(this string message, params object?[] args) => _logger?.LogInformation(message, args);
        public static void LogWrn(this string message, params object?[] args) => _logger?.LogWarning(message, args);        
        public static void LogErr(this string message, params object?[] args) => _logger?.LogError(message, args);
        public static void LogErr(this Exception exc, params object?[] args) => _logger?.LogError(exc, exc.Message, args);
    }
}
