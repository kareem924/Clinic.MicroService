using Appointment.Core.Entities;
using Appointment.Infrastructure.Dto;
using Appointment.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Infrastructure.Data
{
    public class AppointmentDbContext : IAppointmentDbContext
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase database;


        public AppointmentDbContext(IOptions<MongoDbConfig> dbConfig)
        {
            mongoClient = new MongoClient(dbConfig.Value.ConnectionString);
            database = mongoClient.GetDatabase(dbConfig.Value.Database);
        }

        public IMongoCollection<Session> Session
        {
            get
            {
                return database.GetCollection<Session>("Session");
            }
        }

        public IMongoCollection<Core.Entities.Appointment> Appointment
        {
            get
            {
                return database.GetCollection<Core.Entities.Appointment>("Appointment");
            }
        }
    }
}

