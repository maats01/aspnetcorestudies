﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_UpdatePerson = @"
                CREATE PROCEDURE [dbo].[UpdatePerson]
                (@PersonId uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(40), @DateOfBirth datetime2(7), @Gender varchar(10), @CountryId uniqueidentifier, @Address nvarchar(200), @ReceiveNewsLetters bit)
                AS BEGIN
                     UPDATE [dbo].[Persons]
                     SET PersonName = @PersonName, Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender, CountryId = @CountryId, Address = @Address, ReceiveNewsLetters = @ReceiveNewsLetters
                     WHERE PersonId = @PersonId
                END
            ";
            
            migrationBuilder.Sql(sp_UpdatePerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_DropProcedure = @"
                DROP PROCEDURE [dbo].[UpdatePerson]
            ";

            migrationBuilder.Sql(sp_DropProcedure);
        }
    }
}
