using Microsoft.AspNetCore.Mvc;
using EvaluationNexaQuanta.Models; // Replace with your actual namespace
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class ProductController : Controller
{
    private readonly ProductRepository _repository;
    private readonly IWebHostEnvironment _environment;

    public ProductController(ProductRepository repository, IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    // GET: /Product
    public async Task<IActionResult> Index()
    {
        var products = await _repository.GetAllAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _repository.GetAllAsync();
            return Json(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving products.", error = ex.Message });
        }
    }

    // GET: /Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: /Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    // POST: /Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            await _repository.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: /Product/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    // POST: /Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult BulkUpload()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> BulkUpload(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0)
        {
            ViewBag.Message = "Please select a valid CSV file.";
            return View();
        }
        if (!csvFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.Message = "Only .csv files are allowed.";
            return View();
        }
        var filePath = Path.Combine(_environment.WebRootPath, "uploads", Guid.NewGuid() + Path.GetExtension(csvFile.FileName));

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await csvFile.CopyToAsync(stream);
        }

        var products = CsvParser.ParseCsv<Product>(filePath);

        foreach (var product in products)
        {
            
            await _repository.AddAsync(product);
        }

        ViewBag.Message = $"Uploaded {products.Count} products successfully.";
        return View();
    }
}
