using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestePratico_Trainee_ObaBox.Models;
using TestePratico_Trainee_ObaBox.ViewModel;

namespace TestePratico_Trainee_ObaBox.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CadastroCliente()
        {
            return View();
        }

        public ActionResult CadastroEndereco()
        {
             ViewBag.Cliente_id = new SelectList(
                new Cliente().SelecionarAtivos(true),
                "id",
                "nome");
            // https://www.eduardopires.net.br/2014/08/tecnica-simples-dropdownlist-asp-net-mvc/

            return View();
        }

        public ActionResult CadastroCompra()
        {
            ViewBag.lojasCadastradas = new SelectList(
                new Loja().Selecionar(),
                "id",
                "nome");

            ViewBag.clientesCadastrados = new SelectList(
                new Cliente().SelecionarAtivos(true),
                "id",
                "nome");

            return View();
        }

        public ActionResult VerClientes()
        {
            ViewBag.verClientes = new Cliente().ListarClientes();
            return View();
        }

        public ActionResult VerEnderecos()
        {
            ViewBag.verEnderecos = new Endereco().ListarEnderecos();
            return View();
        }
        
        public ActionResult VerCompras()
        {
            ViewBag.verCompras = new Compra().ListarCompras();
            return View();
        }

        public ActionResult EditarCliente(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Cliente cliente = new Cliente().Selecionar(id);
            if(cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dt_nasc = cliente.Dt_nasc.ToString("yyyy-MM-dd");
            return View(cliente);
        }
        
        public ActionResult EditarEndereco(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Endereco endereco = new Endereco().Selecionar(id);
            if(endereco == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cliente_id = new SelectList(
                new Cliente().SelecionarAtivos(true),
                "id",
                "nome",
                endereco.Cliente_id);
            return View(endereco);
        }

        [HttpPost]
        // https://www.completecsharptutorial.com/asp-net-mvc5/asp-net-mvc-5-httpget-and-httppost-method-with-example.php
        public ActionResult CadastroCliente(FormCollection fc)
        {
            Cliente novoCliente = new Cliente();
            novoCliente.Nome = fc["Nome"];
            novoCliente.Cpf = fc["Cpf"];
            novoCliente.Rg = fc["Rg"];
            novoCliente.Dt_nasc = Convert.ToDateTime(fc["Dt_nasc"]);

            novoCliente.Inserir(novoCliente);
            ViewBag.mensagem = "Cadastrado com sucesso.";
            return View();
        }
        [HttpPost]
        public ActionResult EditarCliente(Cliente cliente)
        {
            new Cliente().Alterar(cliente);
            return RedirectToAction("VerClientes");
        }
        [HttpPost]
        public ActionResult CadastroEndereco(Endereco endereco)
        {
            ViewBag.Cliente_id = new SelectList(
                new Cliente().SelecionarAtivos(true),
                "id",
                "nome");
            new Endereco().Inserir(endereco);
            ViewBag.mensagem = "Endereço cadastrado com sucesso.";
            return View();
        }
        [HttpPost]
        public ActionResult EditarEndereco(Endereco endereco)
        {
            new Endereco().Alterar(endereco);
            return RedirectToAction("VerEnderecos");
        }
        [HttpPost]
        public ActionResult CadastroCompra(FormCollection fc)
        {
            ViewBag.lojasCadastradas = new SelectList(
                new Loja().Selecionar(),
                "id",
                "nome");

            ViewBag.clientesCadastrados = new SelectList(
                new Cliente().SelecionarAtivos(true),
                "id",
                "nome");

            ViewBag.enderecosCadastrados = new SelectList(new List<string>());

            int lojaComprada = Convert.ToInt32(fc["lojasCadastradas"]);
            int clienteComprado = Convert.ToInt32(fc["clientesCadastrados"]);
            int enderecoComprado = Convert.ToInt32(fc["enderecosCadastrados"]);

            if (new Loja().VerificarCompra(lojaComprada, clienteComprado, enderecoComprado))
            {
                ViewBag.mensagem = "Este cliente já comprou nesta loja com esse endereço.";
                ViewBag.actionColor = "danger";
            }
            else
            {
                new Loja().CadastrarCompra(lojaComprada, clienteComprado, enderecoComprado);
                ViewBag.actionColor = "success";
                ViewBag.mensagem = "Compra cadastrada com sucesso.";
            }
            return View();
        }
        [HttpGet]
        public JsonResult ListarEnderecos(int clienteId)
        {
            return Json(new { data = new Endereco().SelecionarDoCliente(clienteId) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletarCliente(int id)
        {
            try
            {
                if(new Endereco().SelecionarDoCliente(id).Count > 0)
                {
                    return Json(new { icon = "error", title = "Erro!" , text = "Não é possível deletar o registro, ainda existem endereços cadastrados à esse cliente." }, JsonRequestBehavior.AllowGet);
                }
                new Cliente().Apagar(id);
                return Json(new {icon = "success", title = "Sucesso!", text = "Registro deletado com sucesso."}, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { icon = "error", title = "Erro!", text = $"Ocorreu um erro ao deletar o registro.<br>{ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeletarEndereco(int id)
        {
            try
            {
                if(new Compra().ListarEnderecos(id).Rows.Count > 0)
                {
                    return Json(new { icon = "error", title = "Erro!" , text = "Não é possível deletar o registro, ainda existem compras cadastrados à esse endereço." }, JsonRequestBehavior.AllowGet);
                }
                new Endereco().Apagar(id);
                return Json(new {icon = "success", title = "Sucesso!", text = "Registro deletado com sucesso."}, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { icon = "error", title = "Erro!", text = $"Ocorreu um erro ao deletar o registro.<br>{ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeletarCompra(int idLoja, int idCliente, int idEndereco)
        {
            try
            {
                new Compra().Apagar(idLoja, idCliente, idEndereco);
                return Json(new {icon = "success", title = "Sucesso!", text = "Registro deletado com sucesso."}, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { icon = "error", title = "Erro!", text = $"Ocorreu um erro ao deletar o registro.<br>{ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}