using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TestePratico_Trainee_ObaBox.Models
{
    public class Endereco : Connection
    {
        private string tabela = "endereco";

        public int Id { get; set; }
        public int Cliente_id { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }

        public Endereco()
        {
            Conectar();
        }
        public DataTable ListarEnderecos()
        {
            return Consulta("SELECT * FROM verEnderecos", conn);
        }
        public Endereco Selecionar(int? searchId)
        {
            if(searchId == null)
                throw new ArgumentException("Digite o ID a ser pesquisado");
            DataTable dt = Consulta($"SELECT * FROM {tabela} WHERE id = {searchId}", conn);
            DataRow rowResult = dt.Rows[0];
            return new Endereco {
                Id = Convert.ToInt32(rowResult["id"]),
                Cliente_id = Convert.ToInt32(rowResult["cliente_id"]),
                Logradouro = Convert.ToString(rowResult["logradouro"]),
                Numero = Convert.ToString(rowResult["numero"]),
                Complemento = Convert.ToString(rowResult["complemento"]),
                Cidade = Convert.ToString(rowResult["cidade"]),
                Uf = Convert.ToString(rowResult["uf"]),
                Cep = Convert.ToString(rowResult["cep"])
            };
        }
        public List<Endereco> SelecionarDoCliente(int? searchId)
        {
            DataTable dt = Consulta($"SELECT * FROM {tabela} WHERE cliente_id = {searchId}", conn);
            return (
                from DataRow row in dt.Rows 
                select new Endereco
            {
                Id = Convert.ToInt32(row["id"]),
                Cliente_id = Convert.ToInt32(row["cliente_id"]),
                Logradouro = Convert.ToString(row["logradouro"]),
                Numero = Convert.ToString(row["numero"]),
                Complemento = Convert.ToString(row["complemento"]),
                Cidade = Convert.ToString(row["cidade"]),
                Uf = Convert.ToString(row["uf"]),
                Cep = Convert.ToString(row["cep"])
            }).ToList();
        }
        public void Inserir(Endereco endereco) {
            Comando($@"INSERT INTO {tabela} 
                              VALUES(null, 
                                    '{endereco.Cliente_id}', 
                                    '{endereco.Logradouro}', 
                                    '{endereco.Numero}', 
                                    '{endereco.Complemento}', 
                                    '{endereco.Cidade}', 
                                    '{endereco.Uf}', 
                                    '{endereco.Cep}')", conn);
        }
        public void Alterar(Endereco endereco) {
            Comando($@"UPDATE {tabela} SET 
                                    cliente_id = {endereco.Cliente_id},
                                    logradouro = '{endereco.Logradouro}',
                                    numero = '{endereco.Numero}',
                                    complemento = '{endereco.Complemento}',
                                    cidade = '{endereco.Cidade}',
                                    uf = '{endereco.Uf}',
                                    cep = '{endereco.Cep}'
                        WHERE id = {endereco.Id}", conn);
        }
        public void Apagar(int identificador) {
            Comando($@"DELETE FROM {tabela} WHERE id = {identificador}", conn);
        }

    }
}