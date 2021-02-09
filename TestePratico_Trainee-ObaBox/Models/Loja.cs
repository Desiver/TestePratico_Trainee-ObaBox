using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TestePratico_Trainee_ObaBox.Models
{
    public class Loja : Connection
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Loja()
        {
            Conectar();
        }
        public List<Loja> Selecionar()
        {
            DataTable dt = Consulta("SELECT * FROM loja", conn);
            return 
                (from DataRow row in dt.Rows 
                 select new Loja { Id = Convert.ToInt32(row["id"]), Nome = row["nome"].ToString() })
                 .ToList();
            // https://stackoverflow.com/a/8525024
        }

        public void CadastrarCompra(int loja, int cliente, int endereco)
        {
            Comando($"CALL InserirCompra('{loja}', '{cliente}', '{endereco}')", conn);
        }
        public bool VerificarCompra(int loja, int cliente, int endereco) 
            => 
            Consulta($"SELECT * FROM loja_has_cliente WHERE loja_id = {loja} AND cliente_id = {cliente} AND endereco_id = {endereco}", 
            conn).Rows.Count > 0;
    }
}