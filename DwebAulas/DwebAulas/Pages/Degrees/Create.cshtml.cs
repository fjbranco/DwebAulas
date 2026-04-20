using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aulas.Data.Model;
using DwebAulas.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace DwebAulas.Pages.Degrees
{
    public class CreateModel : PageModel
    {
        /// <summary>
        /// Representa o contexto da base de dados da aplicação
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public Degree Degree { get; set; } = default!;
        
        [BindProperty]
        public IFormFile ImageLogo { get; set; }= default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /* Algoritmo para guardar o ficheiro da imagem para o servidor
             * e colocar a propriedade ImageLogo para o objecto Degree
             *
             * se temos um ficheiro para fazer upload
             *     é preciso verificar se é uma imagem
             *         se é uma imagem, então é preciso especificar o nome da imagem
             *             definir onde para guardar o ficheiro
             *             atribui o nome da imagem para o objecto Degree
             *             guardar a imagem no servidor
             *     noutro caso,
             *         envoa uma mensagem de erro, indicando que não é uma imagem
             */

            //há um ficheiro
            if(ImageLogo==null || ImageLogo.Length == 0)
            {
                ModelState.AddModelError("ImageLogo", "Por favor, faça upload de um ficheiro de imagem");
                return Page();

            }

            // há um ficheiro de imagem
            if(!(ImageLogo.ContentType=="image/jpeg" || ImageLogo.ContentType == "image/png"))
            {
                // !(A && B) = !A || !B
                ModelState.AddModelError("ImageLogo", "Por favor, apenas JPEG ou PNG");
                return Page();
            }


            // processar o ficheiro de imagem
            // define o nome da imagem
            string imageName = Guid.NewGuid().ToString() 
                + Path.GetExtension(ImageLogo.FileName).ToLowerInvariant();
            // atribui o nome da imagem para o objecto Degree
            Degree.Logotype = imageName;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                _context.Degrees.Add(Degree);
                await _context.SaveChangesAsync();
                // guardar a imagemno servidor
                string imagePath = _webHostEnvironment.WebRootPath;
                imagePath=Path.Combine(imagePath, "images");
                if (!Directory.Exists(imagePath)) { 
                    Directory.CreateDirectory(imagePath);
                }

                //
                imagePath = Path.Combine(imagePath, imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                    await ImageLogo.CopyToAsync(stream);

                return RedirectToPage("./Index");

            }
            catch (Exception)
            {
                // throw;

                // em produção apresentar uma mensagem de erro amigável
                ModelState.AddModelError(string.Empty, "Ocorreu um erro");
                return Page();
            }
        }
    }
}
