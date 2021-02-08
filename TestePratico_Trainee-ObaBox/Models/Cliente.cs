using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace TestePratico_Trainee_ObaBox.Models
{
    public class Cliente : Connection
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        [DataType(DataType.Date)]
        public DateTime Dt_nasc { get; set; }
        public bool Ativo { get; set; }
        public List<Loja> Lojas { get; set; }
        public List<Endereco> Enderecos { get; set; }

        private string tabela = "cliente";
        public Cliente()
        {
            Conectar();
        }
        public DataTable ListarClientes()
        {
            return Consulta("SELECT * FROM cliente", conn);
        }
        public List<Cliente> SelecionarAtivos(bool ativo)
        {
            DataTable dt = Consulta($"SELECT * FROM {tabela} WHERE ativo = {(ativo ? 1 : 0)}", conn);
            return (
                from DataRow row in dt.Rows
                select new Cliente
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nome = Convert.ToString(row["nome"]),
                    Cpf = Convert.ToString(row["cpf"]),
                    Rg = Convert.ToString(row["rg"]),
                    Dt_nasc = Convert.ToDateTime(row["dt_nasc"]),
                    Ativo = Convert.ToBoolean(row["ativo"])
                }).ToList();
        }
        public Cliente Selecionar(int? identificador)
        {
            if (identificador == null)
                throw new ArgumentException("Digite o ID a ser pesquisado");
            DataTable dt = Consulta($"SELECT * FROM {tabela} WHERE id = {identificador}", conn);
            DataRow dr = dt.Rows[0];
            return new Cliente
                {
                    Id = Convert.ToInt32(dr["id"]),
                    Nome = Convert.ToString(dr["nome"]),
                    Cpf = Convert.ToString(dr["cpf"]),
                    Rg = Convert.ToString(dr["rg"]),
                    Dt_nasc = Convert.ToDateTime(dr["dt_nasc"]),
                    Ativo = Convert.ToBoolean(dr["ativo"])
                };
        }
        public void Inserir(Cliente cliente)
        {
            Comando($@"INSERT INTO {tabela}(nome, cpf, rg, dt_nasc)
                              VALUES( 
                                    '{cliente.Nome}',
                                    '{cliente.Cpf}',
                                    '{cliente.Rg}',
                                    '{Convert.ToDateTime(cliente.Dt_nasc).ToString("yyyy-MM-dd HH:mm:ss")}')", conn);
        }
        public void Alterar(Cliente cliente)
        {
            Comando($@"UPDATE {tabela} SET 
                                    Nome = '{cliente.Nome}',
                                    Cpf = '{cliente.Cpf}',
                                    Rg = '{cliente.Rg}',
                                    Dt_nasc = '{cliente.Dt_nasc.ToString("yyyy-MM-dd HH:mm:ss")}',
                                    Ativo = {(cliente.Ativo ? 1 : 0)}
                        WHERE id = {cliente.Id}", conn);
        }
        public void Apagar(int identificador)
        {
            Comando($@"DELETE FROM {tabela} WHERE id = {identificador}", conn);
        }
    }
}