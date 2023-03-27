using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Shortener.Dtos;
using Shortener.Models;
using Shortener.Services;
using System;
using System.Collections;
using System.Diagnostics;

namespace Shortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IManageLinksService _linksService;
        private readonly IValidator<ShortenedLinkDto> _validator;

        public HomeController(ILogger<HomeController> logger, IManageLinksService linksService, IValidator<ShortenedLinkDto> validator)
        {
            _logger = logger;
            _linksService = linksService;
            _validator = validator;
        }

        public IActionResult DisplayGeneratedLink(ShortenedLink dto)
        {
            return View(dto);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("All")]
        public async Task<IActionResult> GetAllShortenedLinks()
        {
            IEnumerable<ShortenedLinkDto> dtos = await _linksService.GetAllShortenedLinks();

            return View(dtos);
        }


        [Route("Details/{str}")]
        public IActionResult GetLinkDetails(string str)
        {
            ShortenedLinkDto dto = _linksService.GetOriginalLink(str);
            _logger.LogInformation("{DT}:: Requested details of short link {str} ", DateTime.UtcNow.ToLongTimeString(), str);
            return View(dto);
        }

        public async Task<IActionResult> GenerateShortenedLink(string originalLink)
        {
            var dto = new ShortenedLinkDto { OriginalLink = originalLink };
            ValidationResult result = await _validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                // re-render the view when validation failed.
                return View("Index", dto);
            }

            var x = await _linksService.GenerateShortenedLink(dto);
            _logger.LogInformation("{DT}:: Generated short link {short} for {original} ", DateTime.UtcNow.ToLongTimeString(), x, originalLink);
            return View("DisplayGeneratedLink", dto);
        }

        [Route("{str}")]
        public async Task<IActionResult> RedirectToOriginal(string str)
        {
            ShortenedLinkDto dto = _linksService.GetOriginalLink(str);
            await _linksService.IncrementViews(dto);
            _logger.LogInformation("{DT}:: Redirecting from short link {str} to {original}", DateTime.UtcNow.ToLongTimeString(), str, dto.OriginalLink);
            return Redirect(dto.OriginalLink);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}