using Microsoft.EntityFrameworkCore.Migrations;

namespace ClienteAPI.Migrations
{
    public partial class PopulaDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Clientes(Documento, Nome, DataNascimento, Sexo, Endereco, Estado, Cidade)" +
                "Values('51536448397', 'Rafael Candeias', '1999-09-06 00:00:00.0000000', 'Masculino', 'Rua 1', 'Estado 2', 'Cidade 3')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Clientes");
        }
    }
}
