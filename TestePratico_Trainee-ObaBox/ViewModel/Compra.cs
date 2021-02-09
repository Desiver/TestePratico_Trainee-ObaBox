// https://pt.stackoverflow.com/questions/276308/uma-view-utilizando-dois-models-mvc
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TestePratico_Trainee_ObaBox.Models;

namespace TestePratico_Trainee_ObaBox.ViewModel
{
    public class Compra : Connection
    {
        public Compra()
        {
            Conectar();
        }

        public Loja Loja { get; set; }
        public Cliente Cliente { get; set; }
        public Endereco Endereco { get; set; }

        public DataTable ListarCompras()
        {
            DataTable dt = Consulta("SELECT * FROM verCompras", conn);
            return dt;
        }

        public DataTable ListarEnderecos(int id)
        {
            return Consulta($"SELECT * FROM loja_has_cliente WHERE endereco_id = {id}", conn);
        }

        public void Apagar(int idLoja, int idCliente, int idEndereco)
        {
            Comando($"CALL DeletarCompra({idLoja}, {idCliente}, {idEndereco})", conn);
        }
    }
}