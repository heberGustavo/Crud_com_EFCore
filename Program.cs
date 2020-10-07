using System;
using System.Collections.Generic;
using System.Linq;
using Curso.Data;
using Curso.Domain;
using Curso.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            RemoverDados();
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "123456789123",
                Valor = 10,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new ApplicationContext();
            db.Produto.Add(produto);
            var resultado = db.SaveChanges();

            Console.WriteLine($"Total de Registro(s): {resultado}");
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste massa",
                CodigoBarras = "123456789123",
                Valor = 10,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            /*var cliente = new Cliente
            {
                Nome = "Heber Gustavo",
                CEP = "13455190",
                Cidade = "Americana",
                Estado = "SP",
                Telefone = "19987087683"
            };*/

            var listaClientes = new[]
            {
                new Cliente{
                    Nome = "Gustavo",
                    CEP = "13455190",
                    Cidade = "Americana",
                    Estado = "SP",
                    Telefone = "19987087683"
                },
                new Cliente{
                    Nome = "T-Richards",
                    CEP = "13455190",
                    Cidade = "Americana",
                    Estado = "SP",
                    Telefone = "19987087683"
                }
            };

            using var db = new ApplicationContext();
            //db.AddRange(produto, cliente);
            db.Cliente.AddRange(listaClientes);

            var result = db.SaveChanges();
            Console.WriteLine($"Total de Registros: {result}");
        }

        private static void ConsultarDados()
        {
            using var db = new ApplicationContext();

            var consultaCliente = db.Cliente
                                    .Where(p => p.Id > 0)
                                    .OrderBy(p => p.Id)
                                    .ToList();

            foreach (var cliente in consultaCliente)
            {
                Console.WriteLine($"Consultando Cliente: {cliente}");
                db.Cliente.Find(cliente.Id); // Busca na memoria tmb
                db.Cliente.FirstOrDefault(x => x.Id == cliente.Id); //Busca só no banco
            }
        }

        private static void CadastrarPedido()
        {
            using var db = new ApplicationContext();

            var cliente = db.Cliente.FirstOrDefault();
            var produto = db.Produto.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Observacao teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new ApplicationContext();

            var pedidos = db.Pedidos
                            .Include(p => p.Itens)
                                .ThenInclude(p => p.Produto)
                            .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void AtualizarDados()
        {
            using var db = new ApplicationContext();

            var cliente = db.Cliente.Find(1);
            cliente.Nome = "Cliente Alterado 2";
            cliente.Estado = "MG";

            //db.Cliente.Update(cliente);
            db.SaveChanges();
        }

        private static void RemoverDados()
        {
            using var db = new ApplicationContext();

            var cliente = db.Cliente.Find(2); // Localiza e lista o dado
            //var clienteLocal = new Cliente { Id = 3 }; //Nao lista o cliente, remove direto

            db.Cliente.Remove(cliente);

            //db.Cliente.Remove(clienteLocal);

            //db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }
    }
}
