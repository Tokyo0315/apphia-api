using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository;
using System.Text;

namespace Apphia_Website_API.Controllers;

[Route("")]
[ApiController]
public class SitemapController(IConfiguration config, DatabaseContext dbContext) : ControllerBase
{
    [HttpGet("sitemap.xml")]
    [Produces("application/xml")]
    public IActionResult SitemapIndex()
    {
        var host = $"{Request.Scheme}://{Request.Host}";
        var sitemapUrls = new[]
        {
            $"{host}/sitemap-apphia.xml",
        };

        var xml = GenerateSitemapIndexXml(sitemapUrls);
        return Content(xml, "application/xml");
    }

    [HttpGet("sitemap-apphia.xml")]
    [Produces("application/xml")]
    public async Task<IActionResult> SitemapApphia()
    {
        var baseUrl = config["FrontEndDomains:Default"] ?? throw new InvalidDataException();
        return await Get(baseUrl);
    }

    private async Task<IActionResult> Get(string baseUrl)
    {
        var urls = new List<string>
        {
            $"{baseUrl}/",
            $"{baseUrl}/products",
            $"{baseUrl}/gallery",
            $"{baseUrl}/contact-us",
            $"{baseUrl}/privacy-policy",
        };

        var links = await dbContext.SectionFormattings.Where(s => s.IsActive == true).ToListAsync();

        string[] ignoreTab = ["Landing"];

        foreach (var link in links)
        {
            if (ignoreTab.Contains(link.Tab)) continue;
            urls.Add($"{baseUrl}/{link.Tab.ToLower().Replace(" ", "-")}/{link.Name.ToLower().Replace(" ", "-")}");
        }

        var sitemap = GenerateSitemapXml(urls);
        return Content(sitemap, "application/xml");
    }

    private string GenerateSitemapXml(IEnumerable<string> urls)
    {
        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

        foreach (var url in urls)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{url}</loc>");
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("    <changefreq>weekly</changefreq>");
            sb.AppendLine("    <priority>0.8</priority>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return sb.ToString();
    }

    private string GenerateSitemapIndexXml(IEnumerable<string> sitemapUrls)
    {
        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.AppendLine(@"<sitemapindex xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

        foreach (var sitemapUrl in sitemapUrls)
        {
            sb.AppendLine("  <sitemap>");
            sb.AppendLine($"    <loc>{sitemapUrl}</loc>");
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("  </sitemap>");
        }

        sb.AppendLine("</sitemapindex>");
        return sb.ToString();
    }
}
