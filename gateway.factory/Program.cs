using System;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Bogus;

using gateway.dal;
using gateway.domain.Entities;


namespace gateway.factory
{
    class Program
    {
        /// <summary>Faker (Bogus) lib static object</summary>
        private static readonly Faker F = new Faker("en");                // c# port of faker.js lib
        private static readonly Random _rnd = new Random();                     // random auxiliary
        
        static void Main(string[] args)
        {
            #region ============================ SETUP ============================================

            using var _context = new DbContextFactory().CreateDbContext(new[] { "" });                  // database context
            
            #endregion ============================================================================
            
            #region ============================ SEEDERS ==========================================
            
            if (!HouseKeeping(_context)) return;                                // if we can't clean the database we aren't going to seed
            
            var _gatways = new List<Gateway>();                                 // temporally save the new created gateways  
            
            // ---  seeding gateways ---
            ConsoleShowInfo("-> running SEEDERS");
            var gCount = 10;                                                     // gCount == gateway count
            do
            {
                var newGateway = new Gateway 
                {
                    Name = F.Address.City(),
                    SerialNumber = F.Random.Hash(12, true),
                    IpAddress = F.Internet.Ip()
                };

                _context.Gateways.Add(newGateway);
                _context.SaveChanges();
                _gatways.Add(newGateway);
                
                gCount--;
            } while (gCount > 0);

            // ---  seeding peripherals ---
            foreach (var gateway in _gatways)
            {
                var pCount = F.Random.UInt(1, 10);                          // pCount == peripherals count
                for (int i = 0; i < pCount; i++)
                {
                    
                    var newPeripheral = new Peripheral
                    {
                        Uid = F.Random.Uuid(),
                        Vendor = F.Company.CompanyName(),
                        IsOnline = F.Random.Bool(),
                        Gateway = gateway,
                    };

                    _context.Peripherals.Add(newPeripheral);
                    _context.SaveChanges();
                }
            }
            
            ConsoleShowSuccess("SEEDERS");
            ConsoleShowSuccess();                                               // all good

            #endregion ============================================================================
        }
        
        /// <summary>
        /// Clean the database to seed / populated with new data
        /// </summary>
        /// <param name="dbContext">Database context to handle db connections</param>
        /// <returns>True in case the data was cleaned successfully</returns>
        private static bool HouseKeeping(ADbContext dbContext)
        {
            try
            {
                var sqlScript =
                    @"ALTER TABLE Peripherals DROP CONSTRAINT FK_Peripherals_Gateways_GatewayId; TRUNCATE TABLE Peripherals; TRUNCATE TABLE Gateways; ALTER TABLE Peripherals ADD CONSTRAINT FK_Peripherals_Gateways_GatewayId FOREIGN KEY (GatewayId) REFERENCES Gateways(id) ON DELETE CASCADE;";
                
                dbContext.Database.ExecuteSqlRaw(sqlScript);
                return true;
            }
            catch (SqlException e)
            {
                ConsoleProblem(e, "Something wrong with db cleaning");
                return false;
            }
        }
        
        private static void ConsoleProblem(Exception error, string msg = null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(msg ?? error.Message);
                    Console.ResetColor();
                }
        
        private static void ConsoleShowSuccess(string stage = null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            
            if (stage != null) Console.WriteLine("-> {0} complete successfully", stage); 
            else Console.WriteLine("Success!");
            
            Console.ResetColor();
        }

        private static void ConsoleShowInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(info);
            Console.ResetColor();
        }
    }
}