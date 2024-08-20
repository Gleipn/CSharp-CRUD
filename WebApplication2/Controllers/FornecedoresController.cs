using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public FornecedoresController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var fornecedores = context.Fornecedores.ToList();
            return View(fornecedores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FornecedorDto fornecedorDto)
        {
            if (!ModelState.IsValid)
            {
                return View(fornecedorDto);
            }

            string imagemCaminho = SalvarImagem(fornecedorDto.Imagem);

            Fornecedor fornecedor = new Fornecedor()
            {
                Nome = fornecedorDto.Nome,
                Cnpj = fornecedorDto.Cnpj,
                Segmento = fornecedorDto.Segmento,
                Cep = fornecedorDto.Cep,
                Endereco = fornecedorDto.Endereco,
                ImagemCaminho = imagemCaminho,
                CreatedAt = DateTime.Now,
            };

            context.Fornecedores.Add(fornecedor);
            context.SaveChanges();

            return RedirectToAction("Index", "Fornecedores");
        }

        private string SalvarImagem(IFormFile? imagem)
        {
            if (imagem == null)
            {
                return "/images/266033.png";
            }

            try
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(imagem.FileName);
                string imagemCaminho = Path.Combine(environment.WebRootPath, "images", newFileName);

                using (var stream = new FileStream(imagemCaminho, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }

                return $"/images/{newFileName}";
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("Erro ao salvar a imagem.", ex);
            }
        }

        public IActionResult Edit(int id)
        {
            var fornecedor = context.Fornecedores.Find(id);

            if (fornecedor == null)
                {
                    return RedirectToAction("Index", "Fornecedores");
                }

                var fornecedorDto = new FornecedorDto()
                {
                    Nome = fornecedor.Nome,
                    Cnpj = fornecedor.Cnpj,
                    Segmento = fornecedor.Segmento,
                    Cep = fornecedor.Cep,
                    Endereco = fornecedor.Endereco,
                };

                ViewData["FornecedorId"] = fornecedor.Id;
                ViewData["ImagemCaminho"] = fornecedor.ImagemCaminho;
                ViewData["CreatedAt"] = fornecedor.CreatedAt.ToString("dd/MM/yyyy");

                return View(fornecedorDto);
            
        }


        [HttpPost]
        public IActionResult Edit(int id, FornecedorDto fornecedorDto)
        {
            var fornecedor = context.Fornecedores.Find(id);

            if (fornecedor == null)
            {
                return RedirectToAction("Index", "Fornecedores");
            }

            if (!ModelState.IsValid)
            {
                ViewData["FornecedorId"] = fornecedor.Id;
                ViewData["ImagemCaminho"] = fornecedor.ImagemCaminho;
                ViewData["CreatedAt"] = fornecedor.CreatedAt.ToString("dd/MM/yyyy");
                return View(fornecedorDto);
            }

            string newFileName = fornecedor.ImagemCaminho;
            if (fornecedorDto.Imagem != null)
            {
                try
                {
                    newFileName = SalvarImagem(fornecedorDto.Imagem);
                    RemoverImagemAntiga(fornecedor.ImagemCaminho);
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("", "Erro ao processar a imagem. " + ex.Message);
                    ViewData["FornecedorId"] = fornecedor.Id;
                    ViewData["ImagemCaminho"] = fornecedor.ImagemCaminho;
                    ViewData["CreatedAt"] = fornecedor.CreatedAt.ToString("dd/MM/yyyy");
                    return View(fornecedorDto);
                }
            }

            fornecedor.Nome = fornecedorDto.Nome;
            fornecedor.Cnpj = fornecedorDto.Cnpj;
            fornecedor.Segmento = fornecedorDto.Segmento;
            fornecedor.Cep = fornecedorDto.Cep;
            fornecedor.Endereco = fornecedorDto.Endereco;
            fornecedor.ImagemCaminho = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index", "Fornecedores");
        }

        private void RemoverImagemAntiga(string imagemCaminho)
        {
            if (!string.IsNullOrEmpty(imagemCaminho))
            {
                string oldImagemCaminho = Path.Combine(environment.WebRootPath, imagemCaminho);

                if (System.IO.File.Exists(oldImagemCaminho))
                {
                    try
                    {
                        System.IO.File.Delete(oldImagemCaminho);
                    }
                    catch (Exception ex)
                    {
                        // Logar o erro e/ou informar ao usuário
                        throw new InvalidOperationException("Erro ao deletar a imagem antiga.", ex);
                    }
                }
            }
        }

        public IActionResult Delete(int id)
        {
            var fornecedor = context.Fornecedores.Find(id);
            if (fornecedor == null)
            {
                return RedirectToAction("Index", "Fornecedores");
            }
            string imagemCaminho = environment.WebRootPath + fornecedor.ImagemCaminho;
            System.IO.File.Delete(imagemCaminho);

            context.Fornecedores.Remove(fornecedor);
            context.SaveChanges();

            return RedirectToAction("Index", "Fornecedores");
        }

    }
}
