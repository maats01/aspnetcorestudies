﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                CREATE PROCEDURE [dbo].[InsertPerson]
                (@PersonId uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(40), @DateOfBirth datetime2(7), @Gender varchar(10), @CountryId uniqueidentifier, @Address nvarchar(200), @ReceiveNewsLetters bit)
                AS BEGIN
                     INSERT INTO [dbo].[Persons](PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters)
                     VALUES (@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters)
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_DropProcedure = @"
                DROP PROCEDURE [dbo].[InsertPerson]
            ";
            migrationBuilder.Sql(sp_DropProcedure);
        }
    }
}
